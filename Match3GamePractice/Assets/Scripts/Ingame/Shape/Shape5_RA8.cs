using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape5_RA8 : Shape
{
    //  o
    //  o
    //oox
    //  o
    public Shape5_RA8()
    {
        Add_Point(new Vector2Int(-2, 0));
        Add_Point(new Vector2Int(-1, 0));
        Add_Point(new Vector2Int(0, 2));
        Add_Point(new Vector2Int(0, 1));
        Add_Point(new Vector2Int(0, -1));
    }
}
