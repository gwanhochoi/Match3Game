using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape5_Column2 : Shape
{

    //o
    //o
    //ox
    //o
    //o
    public Shape5_Column2()
    {
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
    }
}
