using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellButton : HUDButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Tower.SellTower();
    }
}
