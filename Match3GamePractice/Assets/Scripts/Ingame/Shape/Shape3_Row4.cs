using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Row4 : Shape
{
    //o
    //xoo
    public Shape3_Row4()
    {
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(1, 0));
        Add_Point(new Vector2Int(2, 0));
    }
}
