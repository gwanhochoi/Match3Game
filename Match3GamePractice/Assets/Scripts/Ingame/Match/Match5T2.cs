using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5T2 : Match
{
    //  o
    //oox
    //  o
    public Match5T2()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(-2, 0));
        Add_Point(new Vector2Int(0, -1));
        bomb = Bomb.Dynamite;
    }
}
