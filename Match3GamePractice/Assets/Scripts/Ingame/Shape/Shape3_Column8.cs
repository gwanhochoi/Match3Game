using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Column8 : Shape
{
    // o
    // o
    //ox
    public Shape3_Column8()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(-1, 0));
    }
}
