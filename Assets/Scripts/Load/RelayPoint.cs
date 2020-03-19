using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayPoint : MonoBehaviour
{
    [SerializeField] RelayPoint nextPoint = null;
    [SerializeField] RelayPoint shortCut = null;

    public int ObstacleCount { get; set; } = 0;
    public bool HaveShortCut { get => shortCut != null; }
    public bool CanGoShortCut { get => ObstacleCount == 0; }
    public bool CanGo { get; set; } = false;
    public RelayPoint NextPoint { get => nextPoint; }
    public RelayPoint ShortCut { get => shortCut; }
}
