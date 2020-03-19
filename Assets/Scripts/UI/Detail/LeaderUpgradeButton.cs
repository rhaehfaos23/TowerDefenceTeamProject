using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderUpgradeButton : MonoBehaviour
{
    public enum UpgradeType { Attack, Supply, Buy }
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] Transform levelBar;

    LeaderData data;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => { return LeaderDetailManger.Instance != null; });
        LeaderDetailManger.Instance.OnTargetChanged += TargetChanged;

        GetComponent<Button>().interactable = false;
    }

    public void TargetChanged()
    {
        string leaderName = LeaderDetailManger.Instance.Target;
        if (PlayerPrefs.HasKey(leaderName))
        {
            string json = PlayerPrefs.GetString(leaderName);
            Debug.Log("LeaderDetail Target Changed ::::" + json);
            data = JsonUtility.FromJson<LeaderData>(json);
        }
        else
        {
            Debug.Log("LeaderUpgradeButton ::: No has key!!");
        }

        LevelBarInit();
    }

    void LevelBarInit()
    {
        int level = 0;
        switch (upgradeType)
        {
            case UpgradeType.Attack:
                level = data.attackLevel;
                break;
            case UpgradeType.Buy:
                level = data.buyLevel;
                break;
            case UpgradeType.Supply:
                level = data.supplyLevel;
                break;
        }

        for (int i = 0; i < levelBar.childCount; ++i)
        {
            Destroy(levelBar.GetChild(i).gameObject);
        }

        for (int i = 0; i < level; ++i)
        {
            AddLevelStick();
        }

        GetComponent<Button>().interactable = true;
    }

    public void LeaderUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.Attack:
                ++data.attackLevel;
                break;
            case UpgradeType.Buy:
                ++data.buyLevel;
                break;
            case UpgradeType.Supply:
                ++data.supplyLevel;
                break;
        }

        PlayerPrefs.SetString(LeaderDetailManger.Instance.Target, JsonUtility.ToJson(data));
        AddLevelStick();
    }

    void AddLevelStick()
    {
        GameObject temp = new GameObject("levelstick");
        temp.AddComponent<UnityEngine.UI.Image>();
        temp.transform.SetParent(levelBar);
    }
}
