using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Column2 : Match
{
    //o
    //x
    //o
    public Match3Column2()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
    }
}
