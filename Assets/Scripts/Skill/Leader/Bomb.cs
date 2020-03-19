using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Spine.Unity;

public class Bomb : LeaderSkill
{
    [SerializeField] float damage;
    [SerializeField] int fallCount;
    [SerializeField] int count;
    [SerializeField] float timeBetweenAttack;
    [SerializeField] float range;
    [SerializeField] GameObject b;
    [SerializeField] GameObject bombPoint;
    [SerializeField] float addDamage;
    [SerializeField] float addfallCount;
    [SerializeField] float addRange;

    GameObject bombRangeAnim;

    protected override void Start()
    {
        base.Start();

    }

    public override void Active()
    {
        if (!CanUse) return;
        bombRangeAnim = Instantiate(bombPoint);
        data = JsonUtility.FromJson<SkillData>(PlayerPrefs.GetString(SkillName));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << 9))
        {
            StartCoroutine(DropBomb(hit.point));
        }
        else
        {
            return;
        }

        base.Active();
    }

    IEnumerator DropBomb(Vector3 point)
    {
        int remainCount = fallCount + Mathf.CeilToInt(data.skillLevel * addfallCount);
        bombRangeAnim.transform.position = point + new Vector3(0f, 0.5f, 0f);
        float r = range + data.skillLevel * addRange;
        while (remainCount > 0)
        {
            for (int i = 0; i < count; ++i)
            {
                float x = Random.Range(0, r);
                float z = Random.Range(0, r);

                Vector3 dropPoint = point + new Vector3(x, 0f, z);
                Instantiate(b, dropPoint + Vector3.forward * 5f, Quaternion.Euler(90f, 0f, 0f));
            }

            Collider[] enemise = Physics.OverlapSphere(point, r);

            for (int i=0; i<enemise.Length; ++i)
            {
                if (enemise[i].tag == "Enemy")
                {
                    enemise[i].GetComponent<Enemy>().TakeDamage(damage + data.skillLevel * addDamage);
                    new WaitForSeconds(0.125f);
                }
            }
            remainCount--;
            yield return new WaitForSeconds(timeBetweenAttack);
        }
        GameObject.Destroy(bombRangeAnim.gameObject);
    }

    public override string GetSkillUpgradeDesc()
    {
        string result = base.GetSkillUpgradeDesc();

        result = string.Format("목표지점에 폭격을 요청한다.\n" +
            "현재 레벨 : {0}\n" +
            "공격력 : {1}\n" +
            "폭격 횟수 : {2}\n" +
            "스킬범위 : {3}\n" +
            data.skillLevel,
            damage + data.skillLevel * addDamage,
            fallCount + Mathf.CeilToInt(data.skillLevel * addfallCount),
            range + data.skillLevel * addRange);

        return result;
    }
}
