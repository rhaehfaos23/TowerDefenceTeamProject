using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Enemy enemy;

    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    public void Attack()
    {
        enemy.Attack();
    }

    public void AnimEnd()
    {
        enemy.AnimEnd();
    }
}
