using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowLevelGrid : MonoBehaviour
{
    [SerializeField] GridLayoutGroup group;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        RectTransform rect = GetComponent<RectTransform>();
        float cellWidth = rect.rect.width / 17f;
        group.cellSize = new Vector2(cellWidth, rect.rect.height * .7f);
        int leftPadding = Mathf.RoundToInt((rect.rect.width - cellWidth * 10f - 27f) / 2f);
        group.padding = new RectOffset(leftPadding, 0, 0, 0);

    }
}
