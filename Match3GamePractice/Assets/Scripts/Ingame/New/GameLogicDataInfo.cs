using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicDataInfo
{

    private Dictionary<int, int> m_TileSnakeCheckPoint_Dic;
    public Dictionary<int, int> TileSnakeCheckPoint_Dic
    {
        get { return m_TileSnakeCheckPoint_Dic; }
        set { m_TileSnakeCheckPoint_Dic = value; }
    }

    public void Add_TileSnakeCheckPoint(Vector2Int pos)
    {
        if(!m_TileSnakeCheckPoint_Dic.ContainsKey(pos.x))
        {
            m_TileSnakeCheckPoint_Dic[pos.x] = pos.y;
            return;
        }

        if(m_TileSnakeCheckPoint_Dic[pos.x] > pos.y)
        {
            m_TileSnakeCheckPoint_Dic[pos.x] = pos.y;
        }
    }


    private List<Vector2Int> m_ClearCoordinate_List;
    public List<Vector2Int> ClearCoordinate_List
    {
        get { return m_ClearCoordinate_List; }
        set
        {
            foreach (var child in value)
            {
                if (!m_ClearCoordinate_List.Contains(child))
                {
                    m_ClearCoordinate_List.Add(child);
                }
            }
            //m_ClearCoordinate_List = value;
        }
    }

    public void Add_ClearCoordinate(Vector2Int coordinate)
    {
        if (!m_ClearCoordinate_List.Contains(coordinate))
        {
            m_ClearCoordinate_List.Add(coordinate);
        }
    }

    private List<Brick> m_MovedBricksList;
    public List<Brick> MovedBricksList
    {
        get { return m_MovedBricksList; }
        set
        {
            foreach(var child in value)
            {
                if(!m_MovedBricksList.Contains(child))
                {
                    m_MovedBricksList.Add(child);
                }
            }
            //m_MovedBricksList = value;
        }
    }


    public GameLogicDataInfo()
    {
        m_ClearCoordinate_List = new List<Vector2Int>();
        m_MovedBricksList = new List<Brick>();
        //m_ClearPosX_List = new List<int>();
        m_TileSnakeCheckPoint_Dic = new Dictionary<int, int>();
    }

    public void Clear()
    {
        m_ClearCoordinate_List.Clear();
        m_MovedBricksList.Clear();
        m_TileSnakeCheckPoint_Dic.Clear();
    }
}
