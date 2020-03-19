using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoNextStageButton : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitUntil(() => { return StageManager.Instance != null; });

        if (StageManager.Instance.NextStageName == "None")
        {
            GetComponent<Button>().interactable = false;
        }

        GetComponent<LoadSceneButton>().SceneName = StageManager.Instance.NextStageName;
    }
}
