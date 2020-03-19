using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 적 공장 생성
/// 라이프 베슬 생성
/// 맵의 릴레이 포인트 관리
/// </summary>
public class LoadManger : MonoBehaviour
{
    static public LoadManger Instance { get; private set; } = null;
    [SerializeField] Transform spawnerPos;
    [SerializeField] RelayPoint startPoint;
    [SerializeField] GameObject lifevessel;
    [SerializeField] GameObject enemySpawner;
    [Tooltip("라이프가 처음 생성되기까지 걸린시간")][SerializeField] float lifeVesselTime;
    [Tooltip("라이프 생성 후 공장이 생성되는데 걸리는 시간")] [SerializeField] float monsterFactoryTime;

    RelayPoint CurrentPoint { get; set; } = null;

    private IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
        }

        yield return new WaitUntil(() => { return LeaderManager.Instance != null; });
        CurrentPoint = startPoint;

        yield return new WaitForSeconds(lifeVesselTime);
        var lifeVessel = Instantiate(lifevessel, CurrentPoint.transform.position, Quaternion.identity);
        lifeVessel.tag = "LifeVessel";

        yield return new WaitForSeconds(monsterFactoryTime);
        Instantiate(enemySpawner, spawnerPos.position, Quaternion.identity);
    }

    public RelayPoint GetNextPoint()
    {
        return CurrentPoint.CanGoShortCut && CurrentPoint.HaveShortCut ?
            CurrentPoint.ShortCut : CurrentPoint.NextPoint;
    }

    public void MoveNext()
    {
        CurrentPoint = CurrentPoint.CanGoShortCut && CurrentPoint.HaveShortCut ?
            CurrentPoint.ShortCut : CurrentPoint.NextPoint;
    }
}
