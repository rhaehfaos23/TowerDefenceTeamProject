using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBall : Bullet
{
    [SerializeField] EffectObject effectObject;
    public override void AttackTarget(Collider col)
    {
        Instantiate(effectObject, col.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
