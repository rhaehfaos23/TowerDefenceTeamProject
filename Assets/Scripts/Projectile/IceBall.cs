using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : Bullet
{
    [Tooltip("이동 속도 15%를 깍는다고 하면 0.85라고 입력할것")] [SerializeField] float debuffAmount;
    [SerializeField] float buffTime;
    public override void AttackTarget(Collider col)
    {
        var debuff = col.gameObject.AddComponent<SpeedDownDebuff>();
        debuff.Rate = debuffAmount;
        debuff.LifeTime = buffTime;
        base.AttackTarget(col);
    }
}
