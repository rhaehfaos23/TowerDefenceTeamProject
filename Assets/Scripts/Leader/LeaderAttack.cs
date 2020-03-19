using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderAttack : MonoBehaviour
{
    LeaderInfo info;
    Enemy enemy = null;
    Animator animator;
    Transform rangeImg;
    GameObject fireEffect;
    Transform firePosition;

    private void Awake()
    {
        info = GetComponent<LeaderInfo>();
        animator = info.animator;
        rangeImg = info.rangeImg;
        fireEffect = info.fireEffect;
        firePosition = info.firePosition;
    }

    private void Start()
    {
        rangeImg.localScale = Vector3.one * info.CurrentRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemy == null)
        {
            Enemy target = other.GetComponent<Enemy>();
            if (target != null)
            {
                enemy = target;
                StartCoroutine(CO_AttackEnemy());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemy == null)
        {
            Enemy target = other.GetComponent<Enemy>();
            if (target != null)
            {
                enemy = target;
                StartCoroutine(CO_AttackEnemy());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy target = other.GetComponent<Enemy>();

        if (target != null && target == enemy)
        {
            enemy = null;
            StopAllCoroutines();
        }
    }

    IEnumerator CO_AttackEnemy()
    {
        while(true)
        {
            if (enemy != null && !info.IsMoving)
            {
                animator.SetBool("IsAttack", true);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Attack()
    {
        if (enemy != null)
        {
            enemy.TakeDamage(info.CurrentDamage);
        }

        Instantiate(fireEffect, firePosition.position, Quaternion.Euler(90f, 0f, 0f));
    }
}
