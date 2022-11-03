using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5RA1 : Match
{
    //xoo
    //o
    //o
    public Match5RA1()
    {
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
        bomb = Bomb.Dynamite;
    }
}
