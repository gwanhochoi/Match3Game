using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape4_Column4 : Shape
{
    //o
    //o
    //xo
    //o
    public Shape4_Column4()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(0, -1));
    }
}
