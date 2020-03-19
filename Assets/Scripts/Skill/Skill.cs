using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 공용 속성")]
    [SerializeField] string skillName;
    [SerializeField] string skillDesc;
    protected string skillUpgradeDesc;

    int skillLevel = 0;

    public string SkillName { get => skillName; }
    public string SkillDesc { get => skillDesc; }
    public string SkillUpgradeDesc { get => skillUpgradeDesc; }
    protected SkillData data;

    virtual protected void Start()
    {
        data = JsonUtility.FromJson<SkillData>(
            PlayerPrefs.GetString(SkillName));
    }

    public virtual string GetSkillUpgradeDesc()
    {
        data = JsonUtility.FromJson<SkillData>(
            PlayerPrefs.GetString(SkillName));

        return "";
    }
}
