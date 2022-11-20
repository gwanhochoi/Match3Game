using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController
{

    public TileController()
    {

    }

    public void EraseBrick(List<Tile> list)
    {
        foreach(var tile in list)
        {
            tile.BrickScript = null;
        }
    }
}
