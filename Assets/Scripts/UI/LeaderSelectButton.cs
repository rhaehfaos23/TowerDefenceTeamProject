using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSelectButton : MonoBehaviour
{


    LeaderController leaderController;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => {
            return (leaderController = FindObjectOfType<LeaderController>()) != null;
        });
    }

    public void OnButtonClick()
    {
        leaderController.IsSelected = true;
    }
}
