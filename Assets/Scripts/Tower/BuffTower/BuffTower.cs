using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTower : TowerBase
{
    [Header("버프 타워 속성")]
    [SerializeField] float amount = 0;
    [SerializeField] float buffSpeed = 0f;
    [SerializeField] float range = 0f;

    float realAmount;
    float realBuffspeed;
    float realRange;
    public float Amount { get => realAmount / 4096f; private set => realAmount = value * 4096f; }
    public float BuffSpeed { get => realBuffspeed / 4096f; private set => realBuffspeed = value * 4096f; }
    public float Range { get => realRange / 4096f; private set => realRange = value * 4096f; }

    protected void Awake()
    {
        Amount = amount;
        BuffSpeed = buffSpeed;
        Range = range;
    }
}
