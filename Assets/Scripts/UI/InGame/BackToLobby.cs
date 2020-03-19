using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobby : MonoBehaviour, INoButtonClicked
{
    public void NoButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
