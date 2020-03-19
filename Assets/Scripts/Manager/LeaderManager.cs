using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderManager : MonoBehaviour
{
    static public LeaderManager Instance { get; private set; } = null;
    [SerializeField] GameObject[] leaders;

    public int LeaderCount { get => leaders.Length; }
    public GameObject this[int i]
    {
        get
        {
            if (i < 0 || i >= leaders.Length)
            {
                return null;
            }
            else
            {
                return leaders[i];
            }
        }
    }

    public int SelectedLeaderIndex { get; set; } = 0; // 선택된 리더의 인덱스 -1이면 선택된 리더 없음

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i=0; i<leaders.Length; ++i)
        {
            if (!PlayerPrefs.HasKey(leaders[i].GetComponent<LeaderInfo>().LeaderName))
            {
                string jsonData = JsonUtility.ToJson(new LeaderData());
                Debug.Log("Leader Initialize ::::" + jsonData);
                PlayerPrefs.SetString(leaders[i].GetComponent<LeaderInfo>().LeaderName,
                    jsonData);
            }
        }
    }
}
