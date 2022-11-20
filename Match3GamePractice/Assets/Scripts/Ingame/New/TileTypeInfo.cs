using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeInfo
{
    //public Tile tile;
    public Tile_Type tile_Type;
    public ObjectType objectType;
    public BrickType brickType;
    public Vector2 pos;
    

    public TileTypeInfo(Tile_Type tile_Type, ObjectType objectType, BrickType brickType, Vector2 pos)
    {
        this.tile_Type = tile_Type;
        this.objectType = objectType;
        this.brickType = brickType;
        this.pos = pos;

    }


}
