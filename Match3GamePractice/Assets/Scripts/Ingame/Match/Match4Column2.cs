using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match4Column2 : Match
{
    //o
    //o
    //x
    //o
    public Match4Column2()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
        bomb = Bomb.VerticalBomb;
    }
}
