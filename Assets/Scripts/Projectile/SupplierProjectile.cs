using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplierProjectile : MonoBehaviour
{
    [SerializeField] float flyingTime;
    [SerializeField] GameObject effect;
    public Vector3 StartPos { get; set; }
    public Vector3 EndPos { get; set; }

    float t;

    private void Update()
    {
        t += Time.deltaTime;

        transform.position = Vector3.Lerp(StartPos, EndPos, t / flyingTime);

        if (t >= flyingTime)
        {
            Instantiate(effect, EndPos + new Vector3(0f, 0f, 0.4f),
                Quaternion.Euler(90f, 0f, 0f));
            Destroy(gameObject);
        }
    }
}
