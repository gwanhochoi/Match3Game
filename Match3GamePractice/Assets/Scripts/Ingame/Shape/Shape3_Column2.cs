using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Column2 : Shape
{
    //o
    //o
    //x
    //o
    public Shape3_Column2()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
    }
}
