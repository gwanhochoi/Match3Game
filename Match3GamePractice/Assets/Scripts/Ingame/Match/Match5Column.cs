using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5Column : Match
{
    //o
    //o
    //x
    //o
    //o
    public Match5Column()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
        bomb = Bomb.ColorBomb;
    }
}
