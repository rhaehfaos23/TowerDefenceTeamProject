using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : MonoBehaviour
{
    public float DamageBuffRate { get; set; }
    public float DamageBuffDuration { get; set; }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var attackTower = GetComponent<AttackTower>();
        if (attackTower == null) Destroy(this);

        attackTower.AddDamageBuff(DamageBuffRate);

        yield return new WaitForSeconds(DamageBuffDuration);

        attackTower.DeleteDamageBuff(DamageBuffRate);

        Destroy(this);
    }

}
