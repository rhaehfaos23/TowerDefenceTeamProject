using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackBuffTowerRange : BuffTowerRange
{
    private void OnTriggerEnter(Collider other)
    {
        IBuffDamage target = other.GetComponent<IBuffDamage>();

        if (target != null)
        {
            target.AddDamageBuff(tower.Amount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IBuffDamage target = other.GetComponent<IBuffDamage>();
        if (target != null)
        {
            target.DeleteDamageBuff(tower.Amount);
        }
    }
}
