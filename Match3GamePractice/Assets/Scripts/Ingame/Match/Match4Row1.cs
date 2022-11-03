using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match4Row1 : Match
{
    //oxoo
    public Match4Row1()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
        bomb = Bomb.HorizontalBomb;
    }
}
