using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MatchResult
{
    private List<Vector2Int> m_ClearCoordinateList;
    public List<Vector2Int> ClearCoordinateList
    {
        get { return m_ClearCoordinateList; }
    }
    private Bomb m_bomb;
    public Bomb bomb
    {
        get { return m_bomb; }
        set
        {
            if(m_bomb < value)
            {
                m_bomb = value;
            }

        }
    }

    public MatchResult()
    {
        m_ClearCoordinateList = new List<Vector2Int>();
        m_bomb = Bomb.None;
    }

    public void Add_ClearCoorinatePos(List<Vector2Int> list)
    {
        foreach(var child in list)
        {
            if(m_ClearCoordinateList.Contains(child))
            {
                continue;
            }
            m_ClearCoordinateList.Add(child);
        }
    }

    public int Count()
    {
        return m_ClearCoordinateList.Count;
    }

    public void Clear()
    {
        m_ClearCoordinateList.Clear();
        m_bomb = Bomb.None;
    }

}
