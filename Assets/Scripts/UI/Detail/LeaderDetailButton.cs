using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderDetailButton : MonoBehaviour
{
    public int LeaderIndex { get; set; } = -1;
    private LeaderInfo leaderInfo;

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => { return LeaderIndex == -1; });
        leaderInfo = LeaderManager.Instance[LeaderIndex].GetComponent<LeaderInfo>();
        GetComponent<Button>().onClick.AddListener(ChangeLeaderDetail);
        GetComponent<Image>().sprite = Resources.Load<Sprite>(leaderInfo.ID);
        GetComponent<Image>().preserveAspect = true;
    }

    void ChangeLeaderDetail()
    {
        LeaderDetailManger.Instance.TargetChange(leaderInfo.LeaderName, LeaderIndex);
    }
}
