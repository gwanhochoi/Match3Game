#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;



[ExecuteInEditMode]//편집모드에서도 monobehaviour 이벤트 발생하게 해준다.
public class TileMap : MonoBehaviour
{

    private int cellWidth_count = 9;
    private int cellHeight_count = 9;

    private int cellWidth_Size = 9;
    private int cellHeight_Size = 9;

    private float start_x;
    private float start_y;
    private float end_x;
    private float end_y;

    private Vector2Int? Selected_Tile_Pos;

    private MapData m_mapData;
    public MapData mapData
    {
        get { return m_mapData; }
        set { m_mapData = value; }
    }

    private Dictionary<Vector2Int?, GameObject> Map_Tile_Dic = new Dictionary<Vector2Int?, GameObject>();


    private void Awake()
    {
        cellWidth_count = EditorData.WidthCount;
        cellHeight_count = EditorData.HeightCount;
        cellWidth_Size = EditorData.CellWidth;
        cellHeight_Size = EditorData.CellHeight;

        m_mapData = new MapData(cellWidth_count, cellHeight_count);
    }


    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }


    public void Reset()
    {
        foreach (var child in Map_Tile_Dic)
        {
            var temp = child.Value;
            GameObject.DestroyImmediate(temp);
        }
        Map_Tile_Dic.Clear();
    }

    private void OnDestroy()
    {
        Reset();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        

        //씬뷰 툴모드 적용 안되게
        //brush, erase mode
        if(EditorData.SelectedTool != PaintTool.Default)
        {
            sceneView.in2DMode = true;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            HandleUtility.Repaint();
        }
        

        //selected tile draw
        //모드에 따라서 그리거나 지우거나 선택하거나

        //마우스 좌표로 타일 선택
        Vector2 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        mousePosition = ray.origin;


        if(Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown: case EventType.MouseDrag:

                    //타일 좌표 가져오기
                    //left bottom 기준 (0,0)
                    Selected_Tile_Pos = Get_CellPos(mousePosition);

                    if (Selected_Tile_Pos == null)
                        break;
                    switch (EditorData.SelectedTool)
                    {
                        
                        case PaintTool.Brush:
                            //맵에디터에서 선택한 타일을 씬뷰에 생성하자
                            Create_Tile();
                            
                            break;
                        case PaintTool.Erase:
                            EraseTile();
                            break;
                    }
                    
                    break;
            }
        }

    }

    private void EraseTile()
    {
        if (!Map_Tile_Dic.ContainsKey(Selected_Tile_Pos.Value))
            return;

        var temp = Map_Tile_Dic[Selected_Tile_Pos.Value];
        Map_Tile_Dic.Remove(Selected_Tile_Pos.Value);
        DestroyImmediate(temp);
    }

    private void Create_Tile()
    {

        //같은곳 선택하면 기존꺼는 지우고 새로 생성
        if (Map_Tile_Dic.ContainsKey(Selected_Tile_Pos.Value))
        {
            //같은곳에 똑같은 타일 있으면 무시
            if(EditorData.Selected_Tile.name == Map_Tile_Dic[Selected_Tile_Pos.Value].name)
            {
                //Debug.Log("Same");
                return;
            }

            //delete
            var temp = Map_Tile_Dic[Selected_Tile_Pos.Value];

            Map_Tile_Dic.Remove(Selected_Tile_Pos.Value);

            GameObject.DestroyImmediate(temp);
        }

        if (EditorData.Selected_Tile != null)
        {
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(EditorData.Selected_Tile);
            obj.name = EditorData.Selected_Tile.name;
            
            obj.GetComponent<SpriteRenderer>().size = new Vector2(cellWidth_Size, cellHeight_Size);
            //obj.transform.localScale = new Vector3(cellWidth_Size, cellHeight_Size);
            obj.transform.parent = transform;
            obj.transform.position = Get_CellCenterPos(Selected_Tile_Pos);

            Map_Tile_Dic[Selected_Tile_Pos.Value] = obj;
            
        }
    }

    public MapData Get_Mapdata()
    {
        mapData.ClearMapData();
        foreach(var child in Map_Tile_Dic)
        {
            mapData.Add_CellData(child.Key.Value.x, child.Key.Value.y, child.Value.name);
        }

        return mapData;
    }

    //draw grid view
    private void OnDrawGizmos()
    {
        //draw grid

        start_x = -(cellWidth_count * cellWidth_Size / 2.0f) + transform.position.x;
        start_y = -(cellHeight_count * cellHeight_Size / 2.0f) + transform.position.y;
        end_x = cellWidth_count * cellWidth_Size / 2.0f + transform.position.x;
        end_y = cellHeight_count * cellHeight_Size / 2.0f + transform.position.y;

        Gizmos.color = new Color(1, 1, 1, 0.25f);
        //draw raw
        for (int i = 0; i < cellHeight_count + 1; i++)
        {
            Gizmos.DrawLine(new Vector3(start_x, start_y + i * cellHeight_Size) ,
                new Vector3(end_x, start_y + i * cellHeight_Size) );
        }
        //draw column
        for (int i = 0; i < cellWidth_count + 1; i++)
        {
            Gizmos.DrawLine(new Vector3(start_x + i * cellWidth_Size, start_y) ,
                new Vector3(start_x + i * cellWidth_Size, end_y) );
        }

        //selected tile draw
        if(Selected_Tile_Pos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Get_CellCenterPos(Selected_Tile_Pos), new Vector3(cellWidth_Size, cellHeight_Size));
        }

    }

    private Vector2Int? Get_CellPos(Vector2 mousePos)
    {
        //float offeset_x = transform.position.x;
        //float offset_y = transform.position.y;

        if (mousePos.x < start_x || mousePos.x > end_x
            || mousePos.y < start_y || mousePos.y > end_y)
        {
            return null;
        }


        int posx = (int)((mousePos.x - start_x) / cellWidth_Size);
        int posy = (int)((mousePos.y - start_y) / cellHeight_Size);

        return new Vector2Int(posx, posy);
    }

    private Vector3 Get_CellCenterPos(Vector2Int? cell)
    {
        float x = start_x + cellWidth_Size * (cell.Value.x + 1 - (1.0f / 2));
        float y = start_y + cellHeight_Size * (cell.Value.y + 1 - (1.0f / 2));

        return new Vector3(x, y);
    }
}

#endif