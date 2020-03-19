using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] protected Enemy info;
    protected Collider target;

    protected virtual void Start()
    {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        SphereCollider myCollider = GetComponent<SphereCollider>();

        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        myCollider.center = Vector3.zero;
        myCollider.radius = info.CurrentRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if (target == null)
        {
            target = other;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (target == null)
        {
            target = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (target != null)
        {
            target = null;
        }
    }

    public virtual void Attack()
    {

    }
}
