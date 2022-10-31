using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape3_Row8 : Shape
{
   //  o
   //oox
    public Shape3_Row8()
    {
        Add_Point(new Vector2Int(-2, 0));
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, 1));
    }
}
