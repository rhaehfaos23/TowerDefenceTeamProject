using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    static public SkillManager Instance { get; private set; } = null;

    List<LeaderSkill> skills;

    public int SkillCount { get => skills.Count; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        skills = new List<LeaderSkill>(GetComponents<LeaderSkill>());

        for (int i=0; i<skills.Count; ++i)
        {
            if (!PlayerPrefs.HasKey(skills[i].SkillName))
            {
                PlayerPrefs.SetString(skills[i].SkillName,
                    JsonUtility.ToJson(new SkillData()));
            }
        }
    }

    public void UseSkill(int idx)
    {
        skills[idx].Active();
    }

    public Skill GetSkill(int idx)
    {
        return skills[idx];
    }
}
