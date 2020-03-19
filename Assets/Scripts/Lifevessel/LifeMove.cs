using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Spine.Unity;
using Spine;

[RequireComponent(typeof(NavMeshAgent))]
public class LifeMove : MonoBehaviour, ISpeedBuff
{
    [SerializeField] Animator animator;
    [SerializeField] SkeletonMecanim skeletonMecanim;
    Lifevessel lifevessel = null;
    RelayPoint curDest = null;
    Rigidbody myRigidBody = null;
    NavMeshAgent agent = null;
    Skeleton skeleton;
    
    // Start is called before the first frame update
    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody>();
        lifevessel = GetComponent<Lifevessel>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        skeleton = skeletonMecanim.skeleton;
        curDest = LoadManger.Instance.GetNextPoint();
        LoadManger.Instance.MoveNext();
        agent.SetDestination(curDest.transform.position);
        StartCoroutine(CO_Move());
    }

    IEnumerator CO_Move()
    {
        animator.SetBool("IsCollide", false);
        while (true)
        {
            if (curDest != null)
            {
                Vector3 dir = curDest.transform.position - transform.position;
                if (dir.sqrMagnitude <= 0.7f)
                {
                    curDest = LoadManger.Instance.GetNextPoint();
                    LoadManger.Instance.MoveNext();
                    agent.SetDestination(curDest.transform.position);
                }
            }

            if (curDest.transform.position.x > transform.position.x)
            {
                skeleton.ScaleX = -1;
            }
            else
            {
                skeleton.ScaleX = 1;
            }
            yield return new WaitForSeconds(.05f);
        }
    }


    public void AddSpeedBuff(float amount)
    {
        agent.speed *= amount;
    }

    public void DeleteSpeedBuff(float amount)
    {
        agent.speed /= amount;
    }
}
