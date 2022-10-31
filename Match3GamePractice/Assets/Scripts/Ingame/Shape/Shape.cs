using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    private List<Vector2Int> m_d_List;
    public List<Vector2Int> d_list
    {
        get { return m_d_List; }
        set { m_d_List = value; }
    }

    public Shape()
    {
        m_d_List = new List<Vector2Int>();
    }

    protected void Add_Point(Vector2Int point)
    {
        m_d_List.Add(point);
    }
}
