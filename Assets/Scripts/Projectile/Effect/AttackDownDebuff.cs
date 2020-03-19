using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDownDebuff : MonoBehaviour
{
    public float DamageDownRate { get; set; } = -1f;
    public float LifeTime { get; set; }
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => { return DamageDownRate >= 0; });
        Enemy my = GetComponent<Enemy>();
        my.BaseDamage *= DamageDownRate;
    }

    IEnumerator EndBuff(Enemy my)
    {
        yield return new WaitForSeconds(LifeTime);
        my.BaseDamage /= DamageDownRate;
        Destroy(this);
    }
}
