using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDetailManager : MonoBehaviour
{
    static public TowerDetailManager Instance { get; private set; } = null;

    public event System.Action OnTargetChanged;
    public enum TowerType { Attack = 1, Defence = 2, Buff = 4, Miner = 8, Base = 16 }
    [SerializeField] Button towerDetailButton; // 추가시킬 버튼 프리팹
    [SerializeField] Transform parentTarget; // 버튼들을 자식들로 추가시킬 타겟 스크롤뷰

    string target;
    public string Target
    {
        get => target;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            target = value;
        }
    }
    public TowerType TargetType { get; set; } = TowerType.Base;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }

        Target = TowerManager.inst.GetTower(0).TowerName;
    }

    bool first = true;

    public void ShowTowerDetail()
    {
        if (!first) return;
        RectTransform rectTransform = parentTarget.GetComponent<RectTransform>();
        for (int i = 0, size = TowerManager.inst.GetTowerLength(), count = 0; i < size; ++i)
        {
            if (TowerManager.inst.GetTower(i) == null) continue;
            var btn = Instantiate(towerDetailButton).GetComponent<TowerDetailButton>();
            btn.transform.SetParent(parentTarget);
            btn.TowerIdx = i;
            count++;
            rectTransform.sizeDelta =
                    new Vector2(100 * count, rectTransform.sizeDelta.y);
        }
        first = false;
    }

    private void OnDestroy()
    {
        if (Instance != null) Instance = null;
    }

    public void TargetChange(string targetName, int index)
    {
        var tb = TowerManager.inst.GetTower(index);
        if (tb is AttackTower)
        {
            TargetType = TowerType.Attack;
        }
        else if (tb is DefenceTower)
        {
            TargetType = TowerType.Defence;
        }
        else if (tb is BuffTower)
        {
            TargetType = TowerType.Buff;
        }
        else
        {
            TargetType = TowerType.Miner;
        }

        Target = targetName;

        OnTargetChanged?.Invoke();
    }
}
