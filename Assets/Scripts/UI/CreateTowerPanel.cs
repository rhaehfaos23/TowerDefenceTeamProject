using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTowerPanel : MonoBehaviour
{
    const float tartgetY = 40f;
    const float speed = 300f;

    RectTransform rt;
    float oriY;

    public bool IsClicked { get; set; } = true;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        oriY = rt.position.y;
    }

    public void ShowPanel()
    {
        StopAllCoroutines();
        StartCoroutine(CO_Show());
    }

    public void HidePanel()
    {
        StopAllCoroutines();
        StartCoroutine(CO_Hide());
    }

    IEnumerator CO_Show()
    {
        rt.SetSiblingIndex(2);
        while (true)
        {
            rt.position += new Vector3(0f, speed * Time.deltaTime, 0f);

            if (rt.position.y >= tartgetY)
            {
                rt.position = new Vector3(rt.position.x, tartgetY, rt.position.z);
                break;
            }
            yield return null;
        }
        IsClicked = true;
        while (IsClicked)
        {
            IsClicked = false;
            yield return new WaitForSeconds(5f);
        }
        UIManager.Instance.ChangePanel(null);
    }

    IEnumerator CO_Hide()
    {
        while (true)
        {
            rt.position -= new Vector3(0f, speed * Time.deltaTime, 0f);

            if (rt.position.y <= oriY)
            {
                rt.position = new Vector3(rt.position.x, oriY, rt.position.z);
                break;
            }
            yield return null;
        }
    }
}
