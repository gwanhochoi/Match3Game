using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape5_RA2 : Shape
{
    //o
    //xoo
    //o
    //o
    public Shape5_RA2()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
        Add_Point(new Vector2Int(0, -1));
        Add_Point(new Vector2Int(0, -2));
    }
}
