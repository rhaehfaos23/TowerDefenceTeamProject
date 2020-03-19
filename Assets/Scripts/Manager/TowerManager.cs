using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    static public TowerManager inst;
    [SerializeField] TowerBase[] towers;
    private TowerBase selectedTower = null;

    public bool CanMove { get; set; } = true;

    private void Start()
    {
        if (inst == null)
        {
            inst = this;
        }
        else if (inst != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        TowerUpgradeData temp = new TowerUpgradeData();
        for (int i=0; i<towers.Length; ++i)
        {
            if (towers[i] == null) continue;
            if (!PlayerPrefs.HasKey(towers[i].TowerName))
            {
                string data = JsonUtility.ToJson(temp);
                Debug.Log("Tower Initialize ::::" + data);
                PlayerPrefs.SetString(towers[i].TowerName, data);
            }
        }


        DontDestroyOnLoad(gameObject);
    }

    bool isTowerMoving = false;

    private void Update()
    {
        if (selectedTower != null)
        {
            Vector3 destPos = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                selectedTower.SetPrevPosition();
                
                if (Physics.Raycast(mouseRay, out RaycastHit hit, 10000f, 1 << 10) && CanMove)
                {
                    isTowerMoving = selectedTower.TowerMoveBegin();
                    
                }
            }

            if (isTowerMoving)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out RaycastHit hit2,
                    10000f, 1 << 8))
                {
                    selectedTower.TowerMove(hit2.point);
                }
            }

            if (Input.GetMouseButtonUp(0) && isTowerMoving)
            {
                selectedTower.TowerMoveEnd();
                selectedTower.DeselectTower();
                selectedTower = null;
                isTowerMoving = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(mouseRay, out RaycastHit hit, 100000, 1 << 10, QueryTriggerInteraction.Collide))
                {
                    TowerBase tower = hit.collider.GetComponent<TowerBase>();
                    if (tower != null)
                    {
                        SelectTower(tower);
                    }
                }
            }
        }
    }

    public void SetTower(int index, TowerBase tower)
    {
        towers[index] = tower;
    }

    public TowerBase GetTower(int index)
    {
        return towers[index];
    }

    /// <summary>
    /// 생성할수 있는 타워 갯수
    /// </summary>
    /// <returns>타워 갯수</returns>
    public int GetTowerLength()
    {
        return towers.Length;
    }

    public void SelectTower(TowerBase tower)
    {
        if (tower == selectedTower)
        {
            Debug.Log("같은 타워 클릭");
            return;
        }
        StartCoroutine(ChageSelect(tower));
    }

    IEnumerator ChageSelect(TowerBase tower)
    {
        yield return null;

        selectedTower = tower;
        selectedTower?.SelectTower();
    }
}
