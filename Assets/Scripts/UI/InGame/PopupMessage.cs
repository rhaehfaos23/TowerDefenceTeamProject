using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    [SerializeField] GameObject message;

    public void PopUpMessage()
    {
        message.SetActive(true);
    }
}
