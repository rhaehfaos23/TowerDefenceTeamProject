using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 이 StageManger의 역할
/// 리더의 시작위치를 찾아와서 리더를 생성
/// 스테이지 관리
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; set; }

    [SerializeField] Enemy[] creatableEnemy;
    [SerializeField] Transform leaderStartPosition;
    [Tooltip("빌드에 넣어둔 이름 그대로 넣기\n" +
        "e.g. scenes/SelectStage/Underground/01")]
    [SerializeField] string currentStageName;
    [Tooltip("빌드에 넣어둔 이름 그대로 넣기\n" +
        "e.g. scenes/SelectStage/Underground/01\n" +
        "없으면 None을 넣어주길 바람")]
    [SerializeField] string nexStageName;
    [Tooltip("승리 화면 UI")]
    [SerializeField] GameObject victoryView;
    [Tooltip("패배 화면 UI")]
    [SerializeField] GameObject defeatedView;

    Lifevessel lifevessel = null;

    public string CurrentStageName { get => currentStageName; }
    public string NextStageName { get => nexStageName; }


    private IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }


        StartCoroutine(CO_FindLifeVessel());

        // 리더를 리더스타팅위치에 생성하기
        yield return new WaitUntil(() => { return LeaderManager.Instance != null; });

        var leader = LeaderManager.Instance[LeaderManager.Instance.SelectedLeaderIndex];
        if (leader != null)
        {
            var createdleader = Instantiate(leader);
            leader.transform.position = leaderStartPosition.position;
            leader.transform.rotation = Quaternion.identity;
        }
    }

    // 라이프베슬을 찾아 게임 클리어와 게임 오버에 관련된 이벤트 추가
    IEnumerator CO_FindLifeVessel()
    {
        yield return new WaitForEndOfFrame();
        while (lifevessel == null)
        { 
            Lifevessel temp = FindObjectOfType<Lifevessel>();
            if (temp != null)
            {
                lifevessel = temp;
            }

            yield return new WaitForSeconds(.1f);
        }

        lifevessel.GameClear += GameClear;
        lifevessel.GameOver += GameOver;
    }

    void GameClear()
    {
        victoryView.SetActive(true);
        GameManager.Instance.GameClear = true;
        Time.timeScale = 0f;
    }

    void GameOver()
    {
        defeatedView.SetActive(true);
        GameManager.Instance.GameOver = true;
        Time.timeScale = 0f;
    }

    // 현 스테이지에서 생성할수 있는 적 얻기
    public Enemy GetEnemey(int index)
    {
        return index < 0 || index >= creatableEnemy.Length 
            ? null : creatableEnemy[index];
    }

    // 현 스테이지에서 생성하는 적의 숫자
    public int GetCreatableEnemyCount()
    {
        return creatableEnemy.Length;
    }
}
