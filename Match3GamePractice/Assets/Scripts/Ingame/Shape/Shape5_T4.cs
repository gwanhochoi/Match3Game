using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape5_T4 : Shape
{

    // o
    //oxoo
    // o
    public Shape5_T4()
    {
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
    }
}
