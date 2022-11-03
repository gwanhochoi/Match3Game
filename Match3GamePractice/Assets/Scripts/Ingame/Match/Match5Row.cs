using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match5Row : Match
{
    //ooxoo
    public Match5Row()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(-2, 0));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
        bomb = Bomb.ColorBomb;
    }
}
