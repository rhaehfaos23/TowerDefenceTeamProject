using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] string sceneName = null;
    [SerializeField] GameObject loadingCanvas = null;

    public string SceneName { get => sceneName; set => sceneName = value; }

    Slider progressBar = null;

    private void Start()
    {
        progressBar = loadingCanvas.GetComponentInChildren<Slider>();
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnButtonClick());
        }
    }

    public void OnButtonClick()
    {
        StartCoroutine(LoadingScene());
    }

    IEnumerator LoadingScene()
    {
        if (SceneName == "None")
        {
            Debug.Log("씬 이름이 비어있습니다");
            yield break;
        }
        loadingCanvas.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            progressBar.value = asyncOperation.progress;

            if (asyncOperation.progress >= 0.9f)
            {
                progressBar.value = 1f;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
