using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class AttackTowerRange : MonoBehaviour
{
    [Tooltip("공격타워의 정보를 들고 있는 스크립트")] [SerializeField] AttackTower tower = null;
    [SerializeField] Animator animator = null;
    [SerializeField] Bullet bullet = null;
    [SerializeField] GameObject rangeImage;

    Enemy target = null;
    bool IsCanAttack { get => !(tower.IsCreating || tower.IsDraging || tower.IsMoving); }

    private void Start()
    {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        SphereCollider mySphereCollider = GetComponent<SphereCollider>();

        // 리지드바디 설정
        myRigidbody.isKinematic = true;
        myRigidbody.useGravity = false;
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // 콜라이더 설정
        mySphereCollider.isTrigger = true;
        mySphereCollider.radius = tower.Range;
        mySphereCollider.center = Vector3.zero;

        //거리 표시 이미지 사이즈 설정
        rangeImage.transform.localScale = Vector3.one * tower.Range;
        animator.SetFloat("AttackSpeed", 1.0f + tower.UpgradeData.AttackSpeedLevel * tower.MinusAttackSpeedPerLevel);
        StartCoroutine(CO_Attack());
    }

    IEnumerator CO_Initialize()
    {
        yield return new WaitForEndOfFrame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                target = enemy;
                target.OnDie += EnemyDie;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                target = enemy;
                target.OnDie += EnemyDie;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null && enemy == target)
        {
            enemy.OnDie -= EnemyDie;
            target = null;
            animator.SetBool("IsFire", false);
        }
    }

    private void OnDestroy()
    {
        target.OnDie -= EnemyDie;
    }

    private void EnemyDie()
    {
        target = null;
        if (animator == null) return;
        animator.SetBool("IsFire", false);
    }
    
    public virtual void Fire(Vector3 firePosition)
    {
        Vector3 dir = Vector3.zero;
        if (target != null)
        {
            dir = target.transform.position - firePosition;
            float ry = -Mathf.Atan2(dir.z, dir.x);
            Quaternion r = Quaternion.Euler(0, ry * 180f / Mathf.PI, 0);
            
            Bullet obj = Instantiate(bullet);
            Vector3 position = new Vector3(firePosition.x, 0f, firePosition.z);
            obj.transform.position = position;
            obj.transform.rotation = r;
            obj.Damage = tower.Damage;
            tower.SubSupply(tower.Reduce);
        }
    }

    IEnumerator CO_Attack()
    {
        while (true)
        {
            if (target != null && IsCanAttack && tower.CurrentSupply >= tower.Reduce)
            {
                if (target.transform.position.x < transform.position.x)
                {
                    tower.FlipYSkeleton(1);
                }
                else
                {
                    tower.FlipYSkeleton(-1);
                }
                animator.SetBool("IsFire", true);
                yield return new WaitForSeconds(tower.AttackSpeed);
            }
            else
            {
                tower.FlipYSkeleton(1);
                animator.SetBool("IsFire", false);
                yield return new WaitForSeconds(0.033333333333333f);
            }
        }
    }
}
