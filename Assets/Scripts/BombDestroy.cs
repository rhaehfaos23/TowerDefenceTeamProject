using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDestroy : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, -speed * Time.deltaTime, 0f);

        if ((startPos - transform.position).sqrMagnitude >= 25f)
        {
            Destroy(gameObject);
        }
    }
}
