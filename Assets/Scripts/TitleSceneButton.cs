using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneButton : MonoBehaviour
{
    [SerializeField] string sceneName = "";
    [SerializeField] Image image = null;
    [SerializeField] GameObject loadingView = null;

    public void LoadScene()
    {
        loadingView.SetActive(true);
        StartCoroutine(LoadingSceneCoroutine());
    }

    IEnumerator LoadingSceneCoroutine()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        loadScene.allowSceneActivation = false;
        while (!loadScene.isDone)
        {
            image.fillAmount = loadScene.progress;

            if (loadScene.progress >= 0.9f)
            {
                image.fillAmount = 1.0f;
                loadScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
