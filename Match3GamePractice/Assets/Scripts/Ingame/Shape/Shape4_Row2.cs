using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape4_Row2 : Shape
{
    //oxoo
    // o
    public Shape4_Row2()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
    }
}
