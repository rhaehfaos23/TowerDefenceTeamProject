using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : HUDButton
{
    [Tooltip("업그레이드 할려는 타워의 인덱스")] [SerializeField] int towerIndex = 0;
    public override void OnPointerClick(PointerEventData eventData)
    {
        Tower.UpgradeTower(towerIndex);
    }
}
