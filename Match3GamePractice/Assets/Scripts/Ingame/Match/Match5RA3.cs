using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5RA3 : Match
{
    //  o
    //  o
    //oox
    public Match5RA3()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(-2, 0));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, 2));
        bomb = Bomb.Dynamite;
    }
}
