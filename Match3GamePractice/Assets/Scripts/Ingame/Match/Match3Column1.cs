using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Column1 : Match
{
    //x
    //o
    //o
    public Match3Column1()
    {
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
    }

}
