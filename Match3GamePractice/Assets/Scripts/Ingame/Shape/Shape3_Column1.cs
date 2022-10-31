using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Column1 : Shape
{
    //o
    //x
    //o
    //o
    public Shape3_Column1()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
    }
}
