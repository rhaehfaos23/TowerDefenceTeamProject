using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeButton : MonoBehaviour
{
    [SerializeField] SkillUpgradeTextUpdate text;
    SkillData data;
    // Start is called before the first frame update
    void Start()
    {
        SkillDetailManager.Instance.OnTargetChanged += () =>
        {
            data = JsonUtility.FromJson<SkillData>(
                PlayerPrefs.GetString(SkillDetailManager.Instance.Target));
        };
    }

    public void SkillUpgrade()
    {
        if (data.skillLevel >= 10) return;
        data.skillLevel++;

        PlayerPrefs.SetString(SkillDetailManager.Instance.Target,
            JsonUtility.ToJson(data));
        text.UpdateText();
    }
}
