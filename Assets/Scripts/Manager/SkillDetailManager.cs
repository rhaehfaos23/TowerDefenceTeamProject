using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDetailManager : MonoBehaviour
{
    public static SkillDetailManager Instance { get; private set; } = null;
    public event System.Action OnTargetChanged;
    [SerializeField] SkillDetailButton button;
    [SerializeField] Transform skillDetailButtons;
    // Start is called before the first frame update

    public string Target { get; set; } = "";
    public int TargetIndex { get; set; } = -1;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }

        
    }

    bool first = true;
    public void InitializeSkillDetail()
    {
        if (!first) return;

        int count = 0;
        for (int i = 0, size = SkillManager.Instance.SkillCount; i < size; ++i)
        {
            var btn = Instantiate(button);
            btn.transform.SetParent(skillDetailButtons);
            btn.SkillIndex = i;
            count++;
        }

        var rect = skillDetailButtons.GetComponent<RectTransform>();
        rect.sizeDelta =
            new Vector2(100 * count, rect.sizeDelta.y);
        first = false;
    }

    public void TargetChange(string targetName, int index)
    {
        Target = targetName;
        TargetIndex = index;
        OnTargetChanged?.Invoke();
    }
}
