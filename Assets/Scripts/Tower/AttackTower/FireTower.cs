using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : MonoBehaviour
{
    [SerializeField] Transform firePosition = null;
    [SerializeField] AttackTowerRange towerRange = null;

    public void Fire()
    {
        towerRange.Fire(firePosition.position);
    }
}
