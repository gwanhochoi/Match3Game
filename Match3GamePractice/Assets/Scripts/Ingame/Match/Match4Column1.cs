using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match4Column1 : Match
{
    //o
    //x
    //o
    //o
    public Match4Column1()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
        bomb = Bomb.VerticalBomb;
    }
}
