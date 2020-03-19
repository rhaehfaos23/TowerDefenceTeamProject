using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDownDebuff : MonoBehaviour
{
    public float Rate { get; set; } = -1f;
    public float LifeTime { get; set; }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => { return Rate >= 0; });
        Enemy my = GetComponent<Enemy>();
        my.AddSpeedBuff(Rate);
    }

    IEnumerator EndBuff(Enemy my)
    {
        yield return new WaitForSeconds(LifeTime);
        my.DeleteSpeedBuff(Rate);
        Destroy(this);
    }
}
