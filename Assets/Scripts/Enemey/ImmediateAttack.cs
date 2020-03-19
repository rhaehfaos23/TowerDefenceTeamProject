using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediateAttack : EnemyAttackRange
{
    public override void Attack()
    {
        Lifevessel lifevessel = target.GetComponent<Lifevessel>();

        if (lifevessel != null)
        {
            lifevessel.TakeDamage(info.CurrentDamage);
        }

        Debug.Log("Enemy Attack");
    }
}
