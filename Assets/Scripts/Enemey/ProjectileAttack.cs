using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : EnemyAttackRange
{
    [SerializeField] Bullet bullet = null;

    public override void Attack()
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.z, dir.x);
        Instantiate(bullet, transform.position, Quaternion.Euler(0, angle, 0));
    }
}
