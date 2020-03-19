using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("발사체의 속도입력")] [SerializeField] float speed = 0f;
    [Tooltip("발사체가 부딪혀야 할 상대")] [SerializeField] LayerMask targetLayer = new LayerMask();
    [Tooltip("발사체가 명중했을때 나오는 이펙트")] [SerializeField] GameObject hittingEffect = null;
    public float Damage { get; set; }
    float lifeTime = 3f;

    private void Update()
    {
        float dist = speed * Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, dist, targetLayer.value))
        {
            AttackTarget(hit.collider);
        }

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        transform.position += transform.right * dist;
    }

    public virtual void AttackTarget(Collider col)
    {
        float randomX = UnityEngine.Random.Range(0f, 0.5f);
        float randomY = UnityEngine.Random.Range(0f, 0.5f);
        if (hittingEffect != null)
        {
            Instantiate(hittingEffect, col.transform.position + new Vector3(randomX, 0f, randomY),
                Quaternion.Euler(90f, 0f, 0f),
                col.transform);
        }
        ITakeDamage target = col.GetComponent<ITakeDamage>();
        target?.TakeDamage(Damage);
        Destroy(gameObject);
    }
}
