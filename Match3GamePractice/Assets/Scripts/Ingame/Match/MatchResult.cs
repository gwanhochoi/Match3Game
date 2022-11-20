using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MatchResult
{
    private List<Tile> m_ClearTileList;
    public List<Tile> ClearTileList
    {
        get { return m_ClearTileList; }
    }

    //private List<Vector2Int> m_ClearCoordinateList;
    //public List<Vector2Int> ClearCoordinateList
    //{
    //    get { return m_ClearCoordinateList; }
    //}
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
        m_ClearTileList = new List<Tile>();
        //m_ClearCoordinateList = new List<Vector2Int>();
        m_bomb = Bomb.None;
    }

    public void Add_ClearTile(List<Tile> list)
    {
        foreach(var child in list)
        {
            if(m_ClearTileList.Contains(child))
            {
                //Debug.Log("contain");
                continue;
            }
            //Debug.Log("Add");
            m_ClearTileList.Add(child);
        }
    }
    //public void Add_ClearCoorinatePos(List<Vector2Int> list)
    //{
    //    foreach(var child in list)
    //    {
    //        if(m_ClearCoordinateList.Contains(child))
    //        {
    //            continue;
    //        }
    //        m_ClearCoordinateList.Add(child);
    //    }
    //}

    public int Count()
    {
        return m_ClearTileList.Count;
    }

    public void Clear()
    {
        m_ClearTileList.Clear();
        //m_ClearCoordinateList.Clear();
        m_bomb = Bomb.None;
    }

}
