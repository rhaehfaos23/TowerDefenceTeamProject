using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("장애물 속성")]
    [SerializeField] int id;
    [SerializeField] string ObstacleName;
    [SerializeField] float maxHp;
    [SerializeField] RelayPoint point;

    int workingMinerCount = 0;
    float currentHp;
    public float CurrentHp { get => currentHp / 16384f; set => currentHp = value * 16384f; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = maxHp;
        if (point != null)
        {
            ++point.ObstacleCount;
        }
    }

    public bool StartMine(float damage, float time, GameObject miner)
    {
        if (workingMinerCount == 0)
        {
            ++workingMinerCount;
            StartCoroutine(StartMineCoroutine(damage, time, miner));
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator StartMineCoroutine(float damage, float time, GameObject miner)
    {
        while (CurrentHp > 0)
        {
            yield return new WaitForSeconds(time);
            TakeDamage(damage);
            Debug.Log("Take Damge : " + damage.ToString());
            Debug.Log("Remaing HP : " + CurrentHp.ToString());
        }

        Destroy(miner);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (point != null)
        {
            --point.ObstacleCount;
        }
    }

    void TakeDamage(float damage)
    {
        CurrentHp -= damage;
    }
}
