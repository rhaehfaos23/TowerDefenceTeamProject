using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsClear : MonoBehaviour
{
    // Start is called before the first frame update
    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs Clear");
    }
}
