using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encourage : LeaderBuffSkill
{
    [Tooltip("고무 버프 공격력 수치 증가량(비율)")] [SerializeField] float damageBuffRate;
    [Tooltip("고무 버프 지속시간")] [SerializeField] float damageBuffDuration;
    [Tooltip("고무 버프 범위")] [SerializeField] float range;
    [SerializeField] float addDamageRate;
    [SerializeField] float addTime;
    [SerializeField] float addRange;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CO_BtnInit());
    }

    IEnumerator CO_BtnInit()
    {
        yield return new WaitForEndOfFrame();
    }

    public override void Active()
    {
        data = JsonUtility.FromJson<SkillData>(PlayerPrefs.GetString(SkillName));
        LeaderInfo leader = FindObjectOfType<LeaderInfo>();
        Animator leaderAnimator = leader.GetComponentInChildren<Animator>();

        Vector3 pos = leader != null ? leader.transform.position : Vector3.zero;

        Collider[] colliders = Physics.OverlapSphere(pos, range + data.skillLevel * addRange);

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].tag == "Tower")
            {
                var tempAttackBuff = colliders[i].GetComponent<AttackBuff>();
                if (tempAttackBuff != null) Destroy(tempAttackBuff);
                var attackBuff = colliders[i].gameObject.AddComponent<AttackBuff>();
                attackBuff.DamageBuffRate = 1f + damageBuffRate + data.skillLevel * addDamageRate;
                attackBuff.DamageBuffDuration = damageBuffDuration + data.skillLevel * addTime;
            }
        }

        leaderAnimator.SetTrigger("IsSkill");

        base.Active();
    }

    public override string GetSkillUpgradeDesc()
    {
        string result = base.GetSkillUpgradeDesc();

        result = string.Format("근처 아군 포탑의 공격력을 잠시 증가 시켜 준다.\n" +
            "현재 레벨 : {0}\n" +
            "공격력 증가량 : {1}\n" +
            "지속시간 : {2}\n" +
            "스킬범위 : {3}", 
            data.skillLevel, 
            1f + damageBuffRate + data.skillLevel * addDamageRate,
            damageBuffDuration + data.skillLevel * addTime,
            range + data.skillLevel * addRange);

        return result;
    }
}
