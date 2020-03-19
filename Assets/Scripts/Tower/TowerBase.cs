using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class TowerBase : MonoBehaviour
{
    #region Event
    // 타워가 생성됨
    public static event System.Action<TowerBase> OnCreatedTower;
    // 타워 이동
    public event System.Action OnCreateFail;
    #endregion

    const int MAX_LEVEL = 10;

    #region Inspector
    [Header("타워 기본 속성")]
    [SerializeField] string id = "";
    [SerializeField] string towerName = null;
    [SerializeField] string decs = null;
    [SerializeField] float maxHP = 0f;
    [SerializeField] int buyPrice = 0;
    [SerializeField] int sellPrice = 0;
    [SerializeField] int[] upgradePrice = null;
    [SerializeField] TowerBase[] upgradeTowers = null;
    [SerializeField] int maxSupply = 0;
    [SerializeField] int reduceAmountSuppliyPerAttack = 0;
    [Tooltip("타워를 설치후 같은 타워를 다시 철치 할때까지 걸리는 시간")]
    [SerializeField] float rebuildTime = 0f;
    [Tooltip("타워 재배치 비용")] [SerializeField] int movePrice = 0;
    [Tooltip("업그레이드로 지을수 있는 타워 인가?")] [SerializeField] bool createByUpgrade = false;
    [Tooltip("타워가 이동할수 있는 타일 종류")] [SerializeField] int canGo;
    [SerializeField] [Tooltip("타워 이동후 다시 공격하기까지 걸리는 시간")] float waitTimeAfterMove = 0f;

    [Header("타워 강화")]
    [Tooltip("레벨당 얼마만큼 최대보급품량이 증가할것인가(비율, 소수로)")] [SerializeField] float AddMaxSupplyPerLevel;
    [Tooltip("레벨당 얼마만큼 타워 구입비용이 줄어들것인가(비율, 소수로)")] [SerializeField] float MinusBuyPricePerLevel;
    [Tooltip("레벨당 얼마만큼 타워 업그레이드 비용이 줄어들것인가(비율, 소수로)")] [SerializeField] float MinusUpgradePricePerLevel;

    [Header("UI관련")]
    [SerializeField] GameObject panel = null;
    [SerializeField] Image supplyImage = null;

    [Header("생성 관련")]
    [Tooltip("공격 범위 트리거 콜라이더")] [SerializeField] Collider rangeTrigger = null;
    #endregion

    SkeletonMecanim skeletonMecanim;
    Skeleton skeleton;
    SpriteRenderer spriteRenderer;
    cakeslice.Outline outline;
    int currentHirePrice;
    int currentSupply;
    float currentHP;
    protected bool creating = false;
    protected bool creatable = true;
    bool moving = false;
    bool near = false; // 타워를 생성할려고 하는 장소에 다른 타워가 있는가?

    public TowerUpgradeData UpgradeData { get; set; } = null;
    public float RebuildTime { get => rebuildTime; }
    public string ID { get => id; }
    public float MaxHP { get => maxHP; }
    public string TowerName { get => towerName; }
    public string Description { get => decs; }
    public int BuyPrice { get => Mathf.CeilToInt(buyPrice * (HireBuffAmount - UpgradeData.BuyLevel * MinusBuyPricePerLevel)); }
    public int SellPrice { get => sellPrice; }
    public int Reduce { get => reduceAmountSuppliyPerAttack; }
    public int MaxSupply { get => Mathf.CeilToInt(maxSupply * (MaxSupplyBuffAmount + UpgradeData.SupplyLevel * AddMaxSupplyPerLevel));}
    
    public int CurrentHirePrice { get => currentHirePrice ^ -1; set => currentHirePrice = value ^ -1; }
    public int CurrentSupply { get => currentSupply ^ -1; set => currentSupply = value ^ -1; }
    public float CurrentHP { get => currentHP / 2; set => currentHP = value * 2; }
    public float HireBuffAmount { get; set; } = 1f;
    public float MaxSupplyBuffAmount { get; set; } = 1f;
    
    public TowerBase GetUpgradeTower(int num)
    {
        return upgradeTowers[num];
    }

    virtual protected void Start()
    {
        // test
        if (!PlayerPrefs.HasKey(TowerName))
        {
            PlayerPrefs.SetString(TowerName,
                JsonUtility.ToJson(new TowerUpgradeData()));
        }
        OnCreatedTower?.Invoke(this);

        UpgradeData = JsonUtility.FromJson<TowerUpgradeData>(PlayerPrefs.GetString(TowerName));
        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
        if (skeletonMecanim != null) skeleton = skeletonMecanim.skeleton;
        else spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CurrentHirePrice = BuyPrice;
        CurrentHP = MaxHP;
        CurrentSupply = MaxSupply;

        // 그래픽 관련
        outline = transform.GetComponentInChildren<cakeslice.Outline>();
        outline.enabled = false;

        if (createByUpgrade)
        {
            creating = false;
            DeselectTower();
        }
        else
        {
            creating = true;
            StartCoroutine(CreatedTower());
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Tower" && creating)
        {
            near = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tower" && creating)
        {
            near = false;
        }
    }

    /// <summary>
    /// 보급 버프를 받음
    /// </summary>
    /// <param name="amount">버프 받을 양</param>
    public void AddSupply(int amount)
    {
        if (creating || moving) return;
        CurrentSupply += amount;

        if (CurrentSupply > MaxSupply)
        {
            CurrentSupply = MaxSupply;
        }

        supplyImage.fillAmount = CurrentSupply / MaxSupply;
    }

    /// <summary>
    /// 보급량이 amount만큼 줄어듬
    /// </summary>
    /// <param name="amount"></param>
    public void SubSupply(int amount)
    {
        if (creating || moving) return;

        CurrentSupply -= amount;

        if (CurrentSupply < 0)
        {
            CurrentSupply = 0;
        }

        supplyImage.fillAmount = (float)CurrentSupply / MaxSupply;
    }

    /// <summary>
    /// 피 회복
    /// </summary>
    /// <param name="amount">피 회복량</param>
    public void RecoveryHP(float amount)
    {
        if (creating || moving) return;
        CurrentHP += amount;

        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }

    private Vector3 prevPos;
    private bool isPrevPosSetted;

    public void SetPrevPosition()
    {
        if (isPrevPosSetted) return;
        prevPos = transform.position;
        isPrevPosSetted = true;
    }

    public void BackPrevPosition()
    {
        transform.position = prevPos;
        isPrevPosSetted = false;
    }


    public bool IsDraging { get; set; } = false;

    /// <summary>
    /// 타워 이동
    /// </summary>
    /// <param name="destination"></param>
    /// <returns>타워가 지금 이동중인가 아닌가</returns>
    public bool TowerMove(Vector3 destination)
    {
        destination.y = 0.05f;
        if (creating || moving) return false;
        if (ResourceManager.Instance.Gold < movePrice) return false;
        IsDraging = true;
        //ResourceManager.Instance.DecreaseGold(movePrice);
        Vector3 origin = destination + Vector3.up * 10;
        
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 100, 1 << 8 | 1 << 9 | 1 << 10, QueryTriggerInteraction.Collide))
        {
            canMove = hit.collider.gameObject.layer == canGo;
            if (canMove)
            {
                SetGraphicColor(new Color(1f, 1f, 1f, 1f));
                Debug.Log("You can move this position");
            }
            else
            {
                SetGraphicColor(new Color(1f, 0f, 0f, 0.5f));
                Debug.Log("You can not move this position");
            }
            transform.position = destination;
        }

        return true;
    }
    public bool TowerMoveBegin()
    {
        if (ResourceManager.Instance.Gold < movePrice) return false;
        GetComponent<Collider>().enabled = false;
        return true;
    }

    private bool canMove = false;

    public void TowerMoveEnd()
    {
        if (canMove) StartCoroutine(WaitForMove());
        else
        {
            BackPrevPosition();
        }
        GetComponent<Collider>().enabled = true;
        IsDraging = false;
    }

    
    public bool IsMoving { get; set; } = false;
    IEnumerator WaitForMove()
    {
        IsMoving = true;
        ResourceManager.Instance.DecreaseGold(movePrice);
        Image progress = transform.GetComponentInChildren<Image>();
        progress.enabled = true;
        progress.fillAmount = 0.0f;
        float t = 0.0f;
        moving = true;

        while(t < waitTimeAfterMove)
        {
            t += Time.deltaTime;
            progress.fillAmount = t / waitTimeAfterMove;
            yield return null;
        }

        progress.enabled = false;

        moving = false;
        isPrevPosSetted = false;
        IsMoving = false;
    }

    void SetGraphicColor(Color color)
    {
        if (skeleton != null) skeleton.SetColor(color);
        else spriteRenderer.color = color;
    }

    public bool IsCreating { get; set; } = false;

    protected virtual IEnumerator CreatedTower()
    {
        IsCreating = true;
        if (rangeTrigger!= null) rangeTrigger.enabled = false;
        Color originalColor;
        if (skeleton != null) originalColor = skeleton.GetColor();
        else originalColor = spriteRenderer.color;
        while (creating)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(mouseRay, 1000, 1 << 8 | 1 << 9 | 1 << 10 | 1 << 12);
            int length = hits.Length;
            if (length > 0)
            {
                creatable = false;
                if (length > 2)
                {
                    SetGraphicColor(new Color(1f, 0f, 0f, 0.5f));
                }
                else
                {
                    for (int i = 0; i < length; ++i)
                    {
                        if (hits[i].collider.gameObject.layer == canGo) creatable |= true;
                    }
                    if (!creatable)
                    {
                        SetGraphicColor(new Color(1f, 0f, 0f, 0.5f));
                    }
                    else
                    {
                        SetGraphicColor(originalColor);
                    }
                }
                transform.position = hits[0].point + new Vector3(0f, 0.05f, 0f);
            }
            yield return null;
        }

        SetGraphicColor(originalColor);
        if (rangeTrigger != null) rangeTrigger.enabled = true;
        IsCreating = false;
    }

    public virtual void CreateComplite()
    {
        if (!creatable)
        {
            OnCreateFail?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            if (ResourceManager.Instance.Gold < CurrentHirePrice)
            {
                OnCreateFail?.Invoke();
                Destroy(gameObject);
            }
            TowerManager.inst.SelectTower(null);
            DeselectTower();
            creating = false;
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            ResourceManager.Instance.DecreaseGold(CurrentHirePrice);
        }
    }

    public void OutLineHide()
    {
        outline.enabled = false;
    }

    public void OutLineShow()
    {
        outline.enabled = true;
    }

    public void SelectTower()
    {
        panel.SetActive(true);
        OutLineShow();
    }

    public void DeselectTower()
    {
        panel.SetActive(false);
        OutLineHide();
    }

    public void UpgradeTower(int index)
    {
        int totalUpgradePrice = Mathf.CeilToInt(upgradePrice[index] 
            * (1f - UpgradeData.UpgradeLevel * MinusUpgradePricePerLevel));
        if (ResourceManager.Instance.Gold < totalUpgradePrice)
        {
            Debug.Log("골드가 부족하여 업그레이드 불가능");
            return;
        }

        ResourceManager.Instance.DecreaseGold(totalUpgradePrice);
        TowerManager.inst.SelectTower(null);
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Destroy(gameObject);
        TowerBase tower = Instantiate(upgradeTowers[index], position, rotation);
    }

    public void SellTower()
    {
        ResourceManager.Instance.IncreaseGold(SellPrice);
        Destroy(gameObject);
    }


    /// <summary>
    /// 스켈레톤을 좌우 반전 시킬것인가
    /// </summary>
    /// <param name="value">1이면 그대로 -1이면 좌우반전</param>
    public void FlipYSkeleton(int value)
    {
        skeleton.ScaleX = value;
    }
}
