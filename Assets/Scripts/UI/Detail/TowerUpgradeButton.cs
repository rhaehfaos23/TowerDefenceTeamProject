using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeButton : MonoBehaviour
{
    public enum UpgradeType { Attack, AttackSpeed, Range,Supply, Buy, Upgrade };
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] Transform levelBar;

    TowerUpgradeData data;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => { return TowerDetailManager.Instance != null; });

        TowerDetailManager.Instance.OnTargetChanged += TargetChanged;
        GetComponent<Button>().interactable = false;
    }

    void LevelBarInit()
    {
        if (PlayerPrefs.HasKey(TowerDetailManager.Instance.Target))
        {
            data = JsonUtility.FromJson<TowerUpgradeData>(PlayerPrefs.GetString(TowerDetailManager.Instance.Target));

            int level = 0;
            switch (upgradeType)
            {
                case UpgradeType.Attack:
                    level = data.AttackLevel;
                    break;

                case UpgradeType.AttackSpeed:
                    level = data.AttackSpeedLevel;
                    break;

                case UpgradeType.Buy:
                    level = data.BuyLevel;
                    break;

                case UpgradeType.Range:
                    level = data.RangeLevel;
                    break;

                case UpgradeType.Supply:
                    level = data.SupplyLevel;
                    break;

                case UpgradeType.Upgrade:
                    level = data.UpgradeLevel;
                    break;
            }

            for (int i=0; i<levelBar.childCount; ++i)
            {
                Destroy(levelBar.GetChild(i).gameObject);
            }

            for (int i = 0; i < level; ++i)
            {
                AddLevelStick();
            }
        }
        else
        {
            Debug.Log(TowerDetailManager.Instance.Target + "에 대한 키를 가지고 있지 않습니다...");
        }
    }

    public void TargetChanged()
    {
        if (PlayerPrefs.HasKey(TowerDetailManager.Instance.Target))
        {
            data = JsonUtility.FromJson<TowerUpgradeData>(PlayerPrefs.GetString(TowerDetailManager.Instance.Target));
        }

        LevelBarInit();

        if (TowerDetailManager.Instance.TargetType != TowerDetailManager.TowerType.Attack)
        {
            if (upgradeType != UpgradeType.Buy && upgradeType != UpgradeType.Upgrade)
            {
                GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    public void TowerUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.Attack:
                ++data.AttackLevel;
                break;

            case UpgradeType.AttackSpeed:
                ++data.AttackSpeedLevel;
                break;

            case UpgradeType.Buy:
                ++data.BuyLevel;
                break;

            case UpgradeType.Range:
                ++data.RangeLevel;
                break;

            case UpgradeType.Supply:
                ++data.SupplyLevel;
                break;

            case UpgradeType.Upgrade:
                ++data.UpgradeLevel;
                break;
        }

        PlayerPrefs.SetString(TowerDetailManager.Instance.Target, JsonUtility.ToJson(data));
        AddLevelStick();
        Debug.Log(TowerDetailManager.Instance.Target + "의 " + upgradeType.ToString() + "를 업그레이드 합니다.");
        Debug.Log(PlayerPrefs.GetString(TowerDetailManager.Instance.Target));
    }

    void AddLevelStick()
    {
        GameObject temp = new GameObject("levelstick");
        temp.AddComponent<UnityEngine.UI.Image>();
        temp.transform.SetParent(levelBar);
    }
}
