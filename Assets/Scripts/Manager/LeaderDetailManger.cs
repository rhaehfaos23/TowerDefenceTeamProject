using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderDetailManger : MonoBehaviour
{
    static public LeaderDetailManger Instance { get; private set; } = null;
    public event System.Action OnTargetChanged;
    [SerializeField] LeaderDetailButton leaderDetailButton;
    [SerializeField] Transform contents;

    public string Target { get; set; } = "";
    public int TargetIndex { get; set; } = -1;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    bool first = true;

    public void InitializeLeaderDetail()
    {
        if (!first) return;
        int count = 0;
        for (int i = 0, size = LeaderManager.Instance.LeaderCount; i < size; ++i)
        {
            var btn = Instantiate(leaderDetailButton);
            btn.transform.SetParent(contents);
            btn.LeaderIndex = i;
            count++;
        }
        var rect = contents.GetComponent<RectTransform>();
        rect.sizeDelta =
            new Vector2(100 * count, rect.sizeDelta.y);
        first = false;
    }

    public void TargetChange(string targetName, int index)
    {
        Target = targetName;
        TargetIndex = index;
        OnTargetChanged?.Invoke();
    }
}
