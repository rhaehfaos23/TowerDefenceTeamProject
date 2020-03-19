using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillUpgradeTextUpdate : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        SkillDetailManager.Instance.OnTargetChanged += UpdateText;
    }

    public void UpdateText()
    {
        text.text = SkillManager.Instance.GetSkill(SkillDetailManager.Instance.TargetIndex).GetSkillUpgradeDesc();
    }
}
