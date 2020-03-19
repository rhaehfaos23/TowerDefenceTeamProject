using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, ISpeedBuff, ITakeDamage
{
    public event System.Action OnDie;

    #region Inspector
    [Header("기초 속성")]
    [Tooltip("시작 레벨")] [SerializeField] int startLevel = 0;
    [Tooltip("기본 공격력")] [SerializeField] float baseDamage = 0f;
    [Tooltip("기본 방어력")] [SerializeField] float baseArmor = 0f;
    [Tooltip("기본 회피율")] [SerializeField] float baseEvasion = 0f;
    [Tooltip("기본 최대 체력")] [SerializeField] float baseMaxHP = 0f;
    [Tooltip("기본 이동 속도")] [SerializeField] float baseSpeed = 0f;
    [Tooltip("기본 사정거리")] [SerializeField] float baseRange = 0f;
    [Tooltip("적 리스폰 시간")] [SerializeField] float baseRespawnTime = 0f;

    [Header("레벨업 증가치")]
    [Tooltip("공격력 증가치")] [SerializeField] float damageStep = 0f;
    [Tooltip("방어력 증가치")] [SerializeField] float armorStep = 0f;
    [Tooltip("회피율 증가치")] [SerializeField] float evasionStep = 0f;
    [Tooltip("최대체력 증가치")] [SerializeField] float maxHPStep = 0f;
    #endregion

    Lifevessel life = null;
    ITakeDamage target = null;
    bool nearTarget = false;
    int level = 0;
    float curHp = 0f;
    float damageRate = 1f;
    Spine.Unity.SkeletonMecanim skeletonMecanim;
    Spine.Skeleton skeleton;
    Animator animator;

    UnityEngine.UI.Image hpBar;

    public float RespawnTime { get => baseRespawnTime; }
    public float CurrentHP { get => curHp / 1024f; set => curHp = value * 1024f; }
    public int Level { get => level ^ -1; set => level = value ^ -1; }
    public float BaseDamage { get => baseDamage; set => baseDamage = value; }
    public float CurrentDamage { get => baseDamage + ((Level - 1) * damageStep); }
    public float CurrentArmor { get => baseArmor + ((Level - 1) * armorStep); }
    public float CurrentEvasion { get => baseEvasion + ((Level - 1) * evasionStep); }
    public float CurrentMaxHP { get => baseMaxHP + ((Level - 1) * maxHPStep); }
    public float CurrentRange { get => baseRange; }


    enum EnemyState
    {
        Trace,
        Attack
    }

    NavMeshAgent agent;
    int relayIndex = 1;
    EnemyState state = EnemyState.Trace;
    int currentWayPointsIndex = 0;
    Vector3 boxCenter;
    Vector3 boxHalfExtents;

    private void Awake()
    {
        Level = startLevel;
        CurrentHP = CurrentMaxHP;

        agent = GetComponent<NavMeshAgent>();
        BoxCollider collider = GetComponent<BoxCollider>();
        boxCenter = collider.center;
        boxHalfExtents = collider.size + new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        hpBar = GetComponentInChildren<UnityEngine.UI.Image>();
        skeletonMecanim = GetComponentInChildren<Spine.Unity.SkeletonMecanim>();
        skeleton = skeletonMecanim.skeleton;
        life = GameObject.FindGameObjectWithTag("LifeVessel").GetComponent<Lifevessel>();
        life.GameOver += () => { Destroy(gameObject); };
        StartCoroutine(TracingLifeVessel());
        StartCoroutine(CheckAround());

        hpBar.fillAmount = CurrentHP / baseMaxHP;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHP <= 0) return;
        CurrentHP -= damage;

        hpBar.fillAmount = CurrentHP / baseMaxHP;

        if (CurrentHP <= 0)
        {
            GetComponent<BoxCollider>().enabled = false;
            OnDie?.Invoke();
            animator.SetTrigger("IsDeath");
            agent.speed = 0;
            agent.isStopped = true;
            Destroy(gameObject, 1f);
        }
    }

    IEnumerator TracingLifeVessel()
    {
        while (true)
        {
            agent.SetDestination(life.transform.position);

            if (life.transform.position.x > transform.position.x)
            {
                skeleton.ScaleX = -1;
            }
            else
            {
                skeleton.ScaleX = 1;
            }

            yield return new WaitForSeconds(0.2f);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "LifeVessel" && other.tag != "DefenceTower") return;
        if (other.tag == "DefenceTower")
        {
            other.GetComponent<DefenceTower>().OnDead += TargetDead;
        }
        agent.isStopped = true;
        animator.SetBool("IsAttack", true);
        target = other.GetComponent<ITakeDamage>();
        nearTarget = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "LifeVessel" && other.tag != "DefenceTower") return;
        animator.SetBool("IsAttack", false);
        nearTarget = false;
    }

    public void TargetDead()
    {
        target = null;
        animator.SetBool("IsAttack", false);
        nearTarget = false;
    }


    // 주변에 공격해야할 대상이 있는지 확인
    IEnumerator CheckAround()
    {
        while (true)
        {
            if (!nearTarget)
            {
                agent.isStopped = false;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Attack()
    {
        if (target != null)
        {
            target.TakeDamage(CurrentDamage);
        }
    }

    public void AnimEnd()
    {
        if (!nearTarget)
        {
            agent.isStopped = false;
        }
    }
}
