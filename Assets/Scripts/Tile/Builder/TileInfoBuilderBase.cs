using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileInfoBuilderBase
{
    public TileInfo tile;

    public TileInfo Get()
    {
        return tile;
    }

    public abstract TileInfoSetter Build();
}
