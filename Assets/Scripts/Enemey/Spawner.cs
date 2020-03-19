using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Spawner : MonoBehaviour, ITakeDamage
{
    [Tooltip("광폭 모드까지 들어가는데 걸리는 시간(단위: 초)")] [SerializeField] float madModeTime = 0f;
    [SerializeField] float maxHP = 0f;
    [Tooltip("광폭 모드에 들어가면 몇번 적을 생성할 것인가")][SerializeField] int madModeCreateCount = 10; // 몇번 생성할것인가
    [Tooltip("광폭 모드에 들어가서 한번 생성할때 최소 몇마리의 적을 생성할것인가")][SerializeField] int madModeCreateEnemyCount = 5;
    [Tooltip("광폭모드에서 카운트가 적으로 변환되는 퍼센트(소수로)")] [SerializeField] float madModeCountPercent = 0f;
    [Tooltip("체력이 0이 되었을때 줄어들 스피드 퍼센트(1+소수로)")][SerializeField] float slowPercent =0.2f;
    [Tooltip("체력이 0이 되었을때 늘어날 리스폰 속도(1+소수로)")][SerializeField] float respawnSlowPercent = .2f;


    [SerializeField] Animator animator;
    // 가스통 채우기
    [SerializeField] Transform zero;
    [SerializeField] Transform full;
    [SerializeField] Transform gasBarrelBackGroundImage;
    NavMeshAgent agent = null;
    Lifevessel lifeVessel = null;

    float curHP = 0f;
    int count = 0; // 광폭모드 카운트(?)
    bool isBreak = false;
    float remainTime = 0f; // 광폭모드들어가는데 남은 시간
    int madModeCount = 0;
    int respawnSlowLevel = 0;

    public bool IsMadMode { get; private set; } = false;
    public bool CanMadMode { get => CurrentHP < 0 || remainTime <= 0; }
    public float CurrentHP { get => curHP / 8192f; set => value = curHP * 8192f; }

    private void Awake()
    {
        CurrentHP = maxHP;
        remainTime = madModeTime;
        agent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator Start()
    {
        gasBarrelBackGroundImage.localPosition = zero.localPosition;

        // 라이프 쫒아가기
        lifeVessel = FindObjectOfType<Lifevessel>();
        if (lifeVessel != null)
        {
            StartCoroutine(CO_TraceLifeVessel());
        }


        yield return new WaitForEndOfFrame();

        // 적생성 초기화
        if (StageManager.Instance == null) yield break;
        for (int i = 0, size = StageManager.Instance.GetCreatableEnemyCount(); i < size; ++i)
        {
            StartCoroutine(CO_EnemySpawn(StageManager.Instance.GetEnemey(i)));
        }
    }

    void Update()
    {
        if (!IsMadMode && remainTime > 0)
        {
            remainTime -= Time.deltaTime;

            if (remainTime <= 0)
            {
                StartMadMode();
            }
        }
    }

    IEnumerator CO_TraceLifeVessel()
    {
        while (true)
        {
            agent.SetDestination(lifeVessel.transform.position);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator CO_EnemySpawn(Enemy enemy)
    {
        yield return new WaitForSeconds(enemy.RespawnTime / 4f);
        while (!GameManager.Instance.GameOver || !GameManager.Instance.GameClear)
        {
            if (IsMadMode) yield return new WaitForSeconds(0.5f);
            else
            {
                Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z)
                    + agent.velocity.normalized;
                Enemy temp = Instantiate(enemy, pos, Quaternion.identity);
                ++count;
                yield return new WaitForSeconds(CurrentHP <= 0 ? enemy.RespawnTime : enemy.RespawnTime * Mathf.Pow(respawnSlowPercent, respawnSlowLevel));
            }
        }
    }

    public void StartMadMode()
    {
        if (IsMadMode)
        {
            StopCoroutine(CO_MadMode());

            StartCoroutine(CO_MadMode());
        }
        else
        {
            StartCoroutine(CO_MadMode());
        }
    }

    IEnumerator CO_MadMode()
    {
        agent.isStopped = true;
        IsMadMode = true;
        madModeCount += madModeCreateCount;

        animator.SetBool("IsMad", true);

        while(madModeCount > 0 && (!GameManager.Instance.GameOver || !GameManager.Instance.GameClear))
        {
            for (int j = 0, size = madModeCreateEnemyCount + (int)(count * madModeCountPercent); j < size; ++j)
            {
                int idx = UnityEngine.Random.Range(0, StageManager.Instance.GetCreatableEnemyCount());
                Enemy enemy = StageManager.Instance.GetEnemey(idx);

                Instantiate(enemy, transform.position + agent.velocity.normalized, Quaternion.identity);
                yield return new WaitForSeconds(0.3f);
            }
            madModeCount--;
            yield return new WaitForSeconds(1f);
        }

        IsMadMode = false;
        agent.isStopped = false;
        count = 0;
        remainTime = madModeTime;
        animator.SetBool("IsMad", false);
    }

    public void TakeDamage(float damage)
    {
        if (IsMadMode) return;
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            if (IsMadMode)
            {
                madModeCount += madModeCreateCount;
            }
            else
            {
                StartCoroutine(CO_MadMode());
            }
            agent.speed *= slowPercent;
            CurrentHP = maxHP;
            ++respawnSlowLevel;
        }
    }
}