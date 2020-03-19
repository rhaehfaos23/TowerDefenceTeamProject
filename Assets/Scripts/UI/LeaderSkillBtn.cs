using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LeaderSkillBtn : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected int skillIndex;
    protected Image coolTimeImg;
    protected Button btn;
    protected LeaderSkill skill;

    public virtual void Awake()
    {
        coolTimeImg = GetComponent<Image>();
        btn = GetComponent<Button>();
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => SkillManager.Instance != null);
        skill = SkillManager.Instance.GetSkill(skillIndex) as LeaderSkill;
        
        StartCoroutine(Co_ImgUpdate());
    }

    public IEnumerator Co_ImgUpdate()
    {
        while (true)
        {
            coolTimeImg.fillAmount = skill.CoolTime;
            btn.interactable = skill.CanUse;

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.Instance.UseSkill(skillIndex);
    }
}
