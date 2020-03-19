using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    [Tooltip("공격력")][SerializeField] float damage;
    [Tooltip("몇초마다 데미지를 줄것인가?")] [SerializeField] float timeBetweenAttack;
    [Tooltip("지속시간")][SerializeField] float lifeTime;
    [Tooltip("범위")] [SerializeField] float range;

    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (lifeTime >= 0)
        {
            lifeTime -= timeBetweenAttack;
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i].tag == "Enemy")
                {
                    colliders[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(timeBetweenAttack);
        }

        Destroy(gameObject);
    }
}
