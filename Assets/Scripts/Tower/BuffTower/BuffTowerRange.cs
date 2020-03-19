using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BuffTowerRange : MonoBehaviour
{
    [Tooltip("버프타워 정보를 가지고 있는 스크립트")]
    [SerializeField] protected BuffTower tower;
    [SerializeField] protected Transform rangeImage;

    protected virtual void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();

        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;

        sphereCollider.isTrigger = true;
        sphereCollider.center = Vector3.zero;
        sphereCollider.radius = tower.Range;

        rangeImage.localScale = Vector3.one * tower.Range;
    }
}
