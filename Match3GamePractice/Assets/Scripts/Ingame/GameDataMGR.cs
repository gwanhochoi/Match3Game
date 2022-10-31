using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMGR : MonoBehaviour
{

    private MapData m_mapData;
    public MapData mapData
    {
        get { return m_mapData; }
        set { m_mapData = value; }
    }

    //private int m_MaxWidthCount;
    public int MaxWidthCount
    {
        get { return mapData.width_Count; }
        set { mapData.width_Count = value; }
    }
    //private int m_MaxHeightCount;
    public int MaxHeightCount
    {
        get { return mapData.height_Count; }
        set { mapData.height_Count = value; }
    }

    public static GameDataMGR Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
