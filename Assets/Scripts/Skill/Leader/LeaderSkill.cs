using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSkill : Skill
{
    [Header("리더스킬 공용속성")]
    [Tooltip("쿨타임")] [SerializeField] float coolTime = 0f;
    [Tooltip("재화 소모량")] [SerializeField] int needGold = 0;

    public float CurCoolTime { get; private set; }
    public float CoolTime { get => CurCoolTime / coolTime; }
    public bool CanUse {
        get {
            return CurCoolTime >= coolTime && 
                ResourceManager.Instance.Gold >= needGold;
        }
    }

    virtual protected void Update()
    {
        CurCoolTime += Time.deltaTime;
    }

    public virtual void Active()
    {
        CurCoolTime = 0f;
        ResourceManager.Instance.DecreaseGold(needGold);
    }
}
