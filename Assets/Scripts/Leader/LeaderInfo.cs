using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LeaderController))]
[RequireComponent(typeof(LeaderAttack))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class LeaderInfo : MonoBehaviour
{
    const float MAX_DISPLAY_AMOUNT = 5f;
    [SerializeField] string id;
    [Header("리더 속성")]
    [SerializeField] string leaderName;
    [SerializeField] string leaderDesc;
    [Tooltip("리더 버프\n" +
        "index = 0 -> 고용 버프 (1 + 소수로)\n" +
        "index = 1 -> 공격 버프\n" +
        "index = 2 -> 보급 버프 (1 + 소수로)")]
    [SerializeField] float[] leaderBonus = null;
    [SerializeField] float damage = 0f;
    [Tooltip("몇초에 한번씩 때릴 것인가?")][SerializeField] float attackSpeed = 0f;
    [SerializeField] float range = 0f;
    [SerializeField] float addDamagePerLevel;
    [SerializeField] float addBuyPerLevel;
    [SerializeField] float addSupplyPerLevel;
    public Animator animator;
    public Transform rangeImg;
    public GameObject fireEffect;
    public Transform firePosition;
    public cakeslice.Outline outline;
    public GameObject hud;


    LeaderData data;
    int leaderLevel = 0;
    float curMoveSpeed = 0f;
    float curDamage = 0f;
    float curRange = 0f;
    float curAttackSpeed = 0f;

    public string ID { get => id; }
    public int LeaderLevel
    {
        get => leaderLevel ^ -1;
        private set
        {
            if (value < 0) return;
            leaderLevel = value ^ -1;
        }
    }
    public string LeaderName { get => leaderName; }
    public string LeaderDesc { get => leaderDesc; }
    public float HireBuffAmount { get => leaderBonus[0] + data.buyLevel * addBuyPerLevel; }
    public float DamageBuffAmount { get => leaderBonus[1] + data.attackLevel * addDamagePerLevel; }
    public float SupplyBuffAmount { get => leaderBonus[2] + data.supplyLevel * addSupplyPerLevel; }
    public float CurrentDamage { get => curDamage / 1024f; set => curDamage = value * 1024f; }
    public float CurrentRange { get => curRange / 1024f; set => curRange = value * 1024f; }
    public float CurrentAttackSpeed { get => curAttackSpeed / 1024f; set => curAttackSpeed = value * 1024f; }
    public bool IsMoving { get; set; } = false;

    private void Awake()
    {
        CurrentDamage = damage;
        CurrentAttackSpeed = attackSpeed;
        CurrentRange = range;
        
        TowerBase.OnCreatedTower += TowerBase_OnCreatedTower;
    }

    private void TowerBase_OnCreatedTower(TowerBase obj)
    {
        if (obj is AttackTower attackTower)
        {
            attackTower.HireBuffAmount = HireBuffAmount;
            attackTower.DamageBuffAmount = DamageBuffAmount;
            attackTower.MaxSupplyBuffAmount = SupplyBuffAmount;
        }
        else if (obj is BuffTower buffTower)
        {
            buffTower.HireBuffAmount = HireBuffAmount;
            buffTower.MaxSupplyBuffAmount = SupplyBuffAmount;
        }
        else if (obj is DefenceTower defenceTower)
        {
            defenceTower.HireBuffAmount = HireBuffAmount;
        }
    }

    protected void Start()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        SphereCollider s = GetComponent<SphereCollider>();
        s.radius = CurrentRange;
        s.center = Vector3.zero;

        data = JsonUtility.FromJson<LeaderData>(PlayerPrefs.GetString(LeaderName));
    }

    public void LeaderLevelUp()
    {
        ++LeaderLevel;
    }
}
