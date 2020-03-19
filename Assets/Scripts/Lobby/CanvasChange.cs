using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasChange : MonoBehaviour
{
    [SerializeField] GameObject currntCanvas = null;
    [SerializeField] GameObject nextCanvas = null;

    public void ChangeCanvas()
    {
        if (currntCanvas != null)
        {
            currntCanvas?.SetActive(false);
        }
        if (nextCanvas != null)
        {
            nextCanvas?.SetActive(true);
        }
    }
}
