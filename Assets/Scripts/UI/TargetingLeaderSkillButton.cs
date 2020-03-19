using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
public class TargetingLeaderSkillButton : LeaderSkillBtn,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] string pointName;
    GameObject point;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!btn.interactable) return;
        point.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        point.SetActive(false);
        SkillManager.Instance.UseSkill(skillIndex);
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => SkillManager.Instance != null);
        skill = SkillManager.Instance.GetSkill(skillIndex) as LeaderSkill;

        StartCoroutine(Co_ImgUpdate());
        StartCoroutine(FindPoint());
    }

    IEnumerator FindPoint()
    {
        while (point == null)
        {
            point = GameObject.Find(pointName);
            yield return null;
        }

        point.SetActive(false);
    }

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << 8))
        {
            if (point != null) point.transform.position = new Vector3(hit.point.x,
                0f,
                hit.point.z);
        }
    }
}
