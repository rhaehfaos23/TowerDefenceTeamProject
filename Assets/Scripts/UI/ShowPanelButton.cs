using UnityEngine;
using UnityEngine.UI;

// 각각의 타워 생성 버튼 패널을 보여주는 버튼
public class ShowPanelButton : MonoBehaviour
{
    [SerializeField] CreateTowerPanel target;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowPanel);
    }

    public void ShowPanel()
    {
        UIManager.Instance.ChangePanel(target);
    }
}
