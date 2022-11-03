using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5T3 : Match
{
    //oxo
    // o
    // o
    public Match5T3()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
        bomb = Bomb.Dynamite;
    }
}
