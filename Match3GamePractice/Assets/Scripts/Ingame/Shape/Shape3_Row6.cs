using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Row6 : Shape
{
    // o
    //oxo
    public Shape3_Row6()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(1, 0));
    }
}
