using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Column7 : Shape
{
    //o
    //o
    //xo
    public Shape3_Column7()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(1, 0));
    }
}
