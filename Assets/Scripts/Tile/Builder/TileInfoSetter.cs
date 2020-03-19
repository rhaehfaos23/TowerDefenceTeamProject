using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoSetter : TileInfoBuilderBase
{
    public TileInfoSetter(TileInfo info)
    {
        tile = info;
    }

    public override TileInfoSetter Build()
    {
        return this;
    }

    public TileInfoSetter DamageOverTime(int damage = 0)
    {
        tile.damageOverTime = damage;
        return this;
    }

    public TileInfoSetter IncreaseDamageRate(float rate = 0)
    {
        tile.increaseDamageRate = rate;
        return this;
    }

    public TileInfoSetter IncreaseDamageType(DamageType type = DamageType.Melee)
    {
        tile.increaseDamageType = type;
        return this;
    }

    public TileInfoSetter DecreaseDamageRate(float rate = 0f)
    {
        tile.decreaseDamageRate = rate;
        return this;
    }

    public TileInfoSetter DecreaseDamageType(DamageType type = DamageType.Melee)
    {
        tile.decreaseDamageType = type;
        return this;
    }

    public TileInfoSetter TileType(TileType type = global::TileType.PlayerWalkable)
    {
        tile.tileType = type;
        return this;
    }

    public TileInfoSetter Buildable(BuildableTowerType type = BuildableTowerType.None)
    {
        tile.buildable = type;
        return this;
    }

    public TileInfoSetter SlowRate(float rate = 0f)
    {
        tile.slowRate = rate;
        return this;
    }

    public TileInfoSetter DodgeRate(float rate = 0f)
    {
        tile.dodgeRate = rate;
        return this;
    }
}
