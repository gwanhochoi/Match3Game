#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public enum PaintTool
{
    Default = 0,
    Brush,
    Erase
}

public static class EditorData
{
    private static GameObject m_selected_Tile;

    public static GameObject Selected_Tile
    {
        get
        {
            return m_selected_Tile;
        }
        set
        {
            m_selected_Tile = value;
        }
    }


    private static int m_WidthCount;
    public static int WidthCount
    {
        get { return m_WidthCount; }
        set { m_WidthCount = value; }
    }

    private static int m_HeightCount;
    public static int HeightCount
    {
        get { return m_HeightCount; }
        set { m_HeightCount = value; }
    }

    private static int m_CellWidth;
    public static int CellWidth
    {
        get { return m_CellWidth; }
        set { m_CellWidth = value; }
    }

    private static int m_CellHeight;
    public static int CellHeight
    {
        get { return m_CellHeight; }
        set { m_CellHeight = value; }
    }

    private static Tool preTool;

    private static PaintTool m_SelectedTool;
    public static PaintTool SelectedTool
    {
        get { return m_SelectedTool; }

        set
        {
            m_SelectedTool = value;

            if(m_SelectedTool == PaintTool.Default)
            {
                Tools.current = preTool;
            }

            else
            {
                Tools.current = Tool.None;
            }
        }
    }

}
#endif