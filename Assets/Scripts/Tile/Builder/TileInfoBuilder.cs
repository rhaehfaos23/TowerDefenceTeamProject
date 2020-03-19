using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoBuilder : TileInfoBuilderBase
{
    public TileInfoBuilder()
    {
        tile = new TileInfo();
    }

    public override TileInfoSetter Build()
    {
        return new TileInfoSetter(tile);
    }
}
