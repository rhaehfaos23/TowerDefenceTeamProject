using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] TowerBase tower;

    protected TowerBase Tower { get => tower; }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TowerManager.inst.CanMove = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TowerManager.inst.CanMove = true;
    }
}
