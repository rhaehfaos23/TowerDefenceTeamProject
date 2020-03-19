using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : LeaderSkill
{
    [SerializeField] float damage;
    [SerializeField] float addDamage;
    
    public override void Active()
    {
        if (!CanUse) return;
        data = JsonUtility.FromJson<SkillData>(PlayerPrefs.GetString(SkillName));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << 11))
        {
            hit.collider.GetComponent<Enemy>().TakeDamage(damage + data.skillLevel * addDamage);
        }
        else
        {
            return;
        }

        base.Active();
    }

    public override string GetSkillUpgradeDesc()
    {
        string result = base.GetSkillUpgradeDesc();

        result = string.Format("한 타겟을 저격해서 큰 데미지를 준다\n" +
            "현재 레벨 : {0}\n" +
            "공격력 : {1}\n",
            data.skillLevel,
            damage + data.skillLevel * addDamage);

        return result;
    }
}
