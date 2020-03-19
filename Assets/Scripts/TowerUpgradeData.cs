using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerUpgradeData
{
    public int attackLevel = 0;
    public int supplyLevel = 0;
    public int buyLevel = 0;
    public int attackSpeedLevel = 0;
    public int rangeLevel = 0;
    public int upgradeLevel = 0;

    public int AttackLevel
    {
        get => attackLevel;
        set
        {
            if (value > 10 || value < 0) return;
            attackLevel = value;
        }
    }

    public int SupplyLevel
    {
        get => supplyLevel;
        set
        {
            if (value > 10 || value < 0) return;
            supplyLevel = value;
        }
    }

    public int BuyLevel
    {
        get => buyLevel;
        set
        {
            if (value > 10 || value < 0) return;
            buyLevel = value;
        }
    }

    public int AttackSpeedLevel
    {
        get => attackSpeedLevel;
        set
        {
            if (value > 10 || value < 0) return;
            attackSpeedLevel = value;
        }
    }

    public int RangeLevel
    {
        get => rangeLevel;
        set
        {
            if (value > 10 || value < 0) return;
            rangeLevel = value;
        }
    }

    public int UpgradeLevel
    {
        get => upgradeLevel;
        set
        {
            if (value > 10 || value < 0) return;
            upgradeLevel = value;
        }
    }
}
