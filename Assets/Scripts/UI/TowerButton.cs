using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] int towerIndex = 0;
    TowerBase createTower = null;  // 버튼이 생성해야할 타워
    TowerBase tempTower = null;    // 현재 생성중인 타워
    float rebuildTime = 0f;             // 타워 재설치까지 걸리는 시간
    float totalTime = 0f;               // 현재 흐른시간
    Image image;
    Button myBtn;
    CreateTowerPanel parent;
    bool createSuccess;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => { return TowerManager.inst != null; });
        createTower = TowerManager.inst.GetTower(towerIndex);
        myBtn = GetComponent<Button>();
        if (createTower != null)
        {
            rebuildTime = createTower.RebuildTime;
            totalTime = rebuildTime + 1f;
            image = GetComponent<Image>();
            image.type = Image.Type.Filled;
            image.fillAmount = 1f;
            parent = transform.parent.GetComponent<CreateTowerPanel>();
            StartCoroutine(CO_CheckCanInteractable());
        }
        else
        {
            myBtn.interactable = false;
        }
    }

    void FixedUpdate()
    {
        if (createTower != null)
        {
            totalTime += Time.fixedDeltaTime;
            image.fillAmount = Mathf.Clamp01(totalTime / rebuildTime);
        }
        
    }

    IEnumerator CO_CheckCanInteractable()
    {
        while (true)
        {
            myBtn.interactable = totalTime >= rebuildTime;
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (createTower.CurrentHirePrice > ResourceManager.Instance.Gold) return;
        createSuccess = true;
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(mouseRay, out float distance) && totalTime >= rebuildTime)
        {
            tempTower = Instantiate(createTower, mouseRay.GetPoint(distance), Quaternion.identity);
            parent.IsClicked = true;
            tempTower.OnCreateFail += () => { createSuccess = false; };
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (totalTime >= rebuildTime && tempTower != null)
        {
            tempTower.CreateComplite();

            if (createSuccess)
            {
                totalTime = 0f;
            }
        }

        tempTower = null;
    }
}
