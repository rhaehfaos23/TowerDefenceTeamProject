using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingClose : MonoBehaviour
{
    [SerializeField] GameObject setting;

    public void CloseSetting()
    {
        setting.SetActive(false);
    }
}
