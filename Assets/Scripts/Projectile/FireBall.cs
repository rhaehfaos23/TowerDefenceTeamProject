using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{
    [Tooltip("공격력 15%를 깍는다고 하면 0.85라고 입력할것")][SerializeField] float debuffAmount;
    [SerializeField] float buffTime;
    public override void AttackTarget(Collider col)
    {
        var debuff = col.gameObject.AddComponent<AttackDownDebuff>();
        debuff.DamageDownRate = debuffAmount;
        debuff.LifeTime = buffTime;
        base.AttackTarget(col);
    }
}
