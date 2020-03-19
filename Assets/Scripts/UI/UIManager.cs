using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager Instance { get; set; } = null;

    CreateTowerPanel selectedPanel = null;

    private void Start()
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

    public void ChangePanel(CreateTowerPanel panel)
    {
        if (selectedPanel == panel)
        {
            selectedPanel.HidePanel();
            selectedPanel = null;
        }
        else
        {
            selectedPanel?.HidePanel();
            selectedPanel = panel;
            selectedPanel?.ShowPanel();
        }
    }
}
