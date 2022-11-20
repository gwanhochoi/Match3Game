using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnake
{
    private List<Tile> m_TileList;
    public List<Tile> TileList
    {
        get { return m_TileList; }
    }
    public TileSnake()
    {
        m_TileList = new List<Tile>();
    }

    public bool ContainsBrick(Brick brick)
    {
        //if (brick.coordinate != null)
        //{
        //    Debug.Log("brick not null");
        //}
        //else
        //{
        //    Debug.Log("brick null");
        //}

        foreach (var tile in m_TileList)
        {
            
            
            if(tile.Coordinate == brick.coordinate)
            {
                return true;
            }
        }
        return false;
    }

    public void Add_Tile(Tile tile)
    {
        //중복 타일은 안됨
        
        if (m_TileList.Contains(tile))
            return;

        m_TileList.Add(tile);
    }

    public int Count()
    {
        return m_TileList.Count;
    }



}
