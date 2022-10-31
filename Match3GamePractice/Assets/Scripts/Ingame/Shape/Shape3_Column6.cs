using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Column6 : Shape
{
    // o
    //ox
    // o
    public Shape3_Column6()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, -1));
    }
}
