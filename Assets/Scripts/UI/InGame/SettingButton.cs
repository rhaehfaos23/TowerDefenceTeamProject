using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] GameObject settingCanvas = null;
    bool isPause = false;

    

    private void Start()
    {
        settingCanvas.SetActive(false);
    }

    public void OnClickSettingButton()
    {
        isPause = !isPause;

        if (isPause)
        {
            Time.timeScale = 0f;
            settingCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            settingCanvas.SetActive(false);
        }
    }

    public void OpenSetting()
    {
        settingCanvas.SetActive(true);
    }
}
