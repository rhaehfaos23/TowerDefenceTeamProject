using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailButton : MonoBehaviour
{
    public int SkillIndex { get; set; } = -1;
    Skill skill = null;

    IEnumerator Start()
    {
        yield return new WaitWhile(() => { return SkillIndex == -1; });
        skill = SkillManager.Instance.GetSkill(SkillIndex);

        GetComponent<Button>().onClick.AddListener(ChangeSkillDetail);
        GetComponent<Image>().sprite = Resources.Load<Sprite>(skill.SkillName);
    }

    void ChangeSkillDetail()
    {
        SkillDetailManager.Instance.TargetChange(skill.SkillName, SkillIndex);
    }
}
