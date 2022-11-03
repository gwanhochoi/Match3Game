using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5T4 : Match
{
    // o
    // o
    //oxo
    public Match5T4()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, 2));
        bomb = Bomb.Dynamite;
    }
}
