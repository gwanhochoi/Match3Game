using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapController : MonoBehaviour
{


    private MapData mapData;

    //cell default size 100
    private const int CELL_SIZE = 100;
    //맵 최대 크기는 가로 9 세로 13
    private int MAX_WIDTHCOUNT = 9;
    private int MAX_HEIGHTCOUNT = 13;

    private GameObject[][] tile_Map;
    private Dictionary<string, Sprite> sprite_dic;

    public GameObject Cell_Prefab;

    public GameObject[] BrickPrefabs;


    //create map

    //touch event controll

    private void Awake()
    {
        //load map data
        LoadMapData();

        

        sprite_dic = new Dictionary<string, Sprite>();

    }


    private void Start()
    {
        MAX_WIDTHCOUNT = mapData.width_Count;
        MAX_HEIGHTCOUNT = mapData.height_Count;

        tile_Map = new GameObject[MAX_WIDTHCOUNT][];
        for (int i = 0; i < MAX_WIDTHCOUNT; i++)
        {
            tile_Map[i] = new GameObject[MAX_HEIGHTCOUNT];
        }


        for (int i = 0; i < MAX_WIDTHCOUNT; i++)
        {
            for (int j = 0; j < MAX_HEIGHTCOUNT; j++)
            {
                tile_Map[i][j] = Instantiate(Cell_Prefab);
                tile_Map[i][j].transform.SetParent(transform);
                tile_Map[i][j].transform.localPosition = new Vector3(
                    (MAX_WIDTHCOUNT * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * i,
                    (MAX_HEIGHTCOUNT * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * j);
            }
        }

        Sprite []sprites = Resources.LoadAll<Sprite>("Textures");

        foreach(var child in sprites)
        {
            sprite_dic[child.name] = child;
        }
    }

    private void Update()
    {
        
    }


    public void CreateBlock_InField()
    {
        //원래는 블럭 놓을수 있는 위치에만 생성해야하지만 임시로 일단 맵 최대크기 다 사용한다고 가정하고 생성하자.
        //일단 타일에 따른 처리는 아직 안하므로 높이를 4개 줄인상태로 9x9로 한다
        GetComponent<GameField>().Fill_Bricks(MAX_WIDTHCOUNT, MAX_HEIGHTCOUNT - 4);
        
    }

    public void CreateGameField()
    {
        //sprites 읽어오기?


        //읽어온 맵데이터 기반으로 오브젝트 생성

        int width_count = mapData.width_Count;
        int height_count = mapData.height_Count;

        //일단 맵데이터 기준으로 생성

        foreach(var child in mapData.m_cellData_List)
        {
            tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().sprite = sprite_dic[child.name];
            tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().size = new Vector2(CELL_SIZE, CELL_SIZE);
        }



    }

    private void LoadMapData()
    {
        //TextAsset []mapJson = Resources.LoadAll<TextAsset>("MapJsonData");

        TextAsset mapJson = Resources.Load<TextAsset>("MapJsonData/mapdata_ 0");

        if(mapJson == null)
        {
            //Debug.Log("null");
            return;
        }

        mapData = JsonUtility.FromJson<MapData>(mapJson.ToString());

        if(mapData == null)
        {
            //Debug.Log("mapData null");
            return;
        }

    }

}
