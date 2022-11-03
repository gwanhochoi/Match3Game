using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match4Row2 : Match
{
    //ooxo
    public Match4Row2()
    {
        Add_Point(new Vector2Int(-2, 0));
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(1, 0));
        bomb = Bomb.HorizontalBomb;
    }
}
