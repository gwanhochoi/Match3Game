using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Row2 : Match
{
    //oxo
    public Match3Row2()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(1, 0));
    }
}
