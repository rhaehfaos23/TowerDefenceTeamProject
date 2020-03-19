using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadModePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("공장 광폭모드 활성화!!!!!");

        if (other.tag != "LifeVessel") return;

        Spawner spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
        {
            spawner.StartMadMode();
            Destroy(gameObject);
        }
    }
}
