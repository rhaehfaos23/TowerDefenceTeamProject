using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGrid : MonoBehaviour
{
    [SerializeField] GridLayoutGroup group;

    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        group.cellSize =
            new Vector2(rect.rect.width / group.constraintCount, rect.rect.height / 3f);
    }
}
