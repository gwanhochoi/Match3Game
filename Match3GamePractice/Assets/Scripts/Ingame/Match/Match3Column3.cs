using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Column3 : Match
{
    //o
    //o
    //x
    public Match3Column3()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, 2));
    }
}
