using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//맵데이터를 토대로 타일맵 생성

public enum Tile_Type
{
    Back = 0,
    Ground,
    Charge,
    End
}

public class TileMapCreator : MonoBehaviour
{
    public GameObject[] Tile_Prefab;

    private const int CELL_SIZE = 100;

    private int m_max_widthCount;
    public int max_widthCount
    {
        get { return m_max_widthCount; }
        set { m_max_widthCount = value; }
    }

    private int m_max_heightCount;
    public int max_heightCount
    {
        get { return m_max_heightCount; }
        set { m_max_heightCount = value; }
    }

    private Dictionary<string, Sprite> sprite_dic;

    //private Tile[][] m_Tiles;
    //public Tile[][] Tiles
    //{
    //    get { return m_Tiles; }
    //}

    private void Awake()
    {
        sprite_dic = new Dictionary<string, Sprite>();
    }

    private void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures");

        foreach (var child in sprites)
        {
            sprite_dic[child.name] = child;
        }
    }


    //맵데이터를 받아와서 생성한다
    public Tile[][] CreateTileMap(MapData mapData)
    {
        max_widthCount = mapData.width_Count;
        max_heightCount = mapData.height_Count;

        Tile[][] m_Tiles;


        m_Tiles = new Tile[max_widthCount][];

        for(int i = 0; i < max_widthCount; i++)
        {
            m_Tiles[i] = new Tile[max_heightCount];
        }

        //for (int i = 0; i < max_widthCount; i++)
        //{
        //    m_Tiles[i] = new Tile[max_heightCount];
        //    for (int j = 0; j < max_heightCount; j++)
        //    {
                
        //        GameObject obj = Instantiate(Tile_Prefab[0]);
        //        m_Tiles[i][j] = obj.GetComponent<Tile>();
        //        m_Tiles[i][j].transform.SetParent(transform);
        //        m_Tiles[i][j].transform.localPosition = new Vector3(
        //            (max_widthCount * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * i,
        //            (max_heightCount * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * j);


        //        m_Tiles[i][j].Coordinate = new Vector2Int(i, j);
        //    }

        //}

        foreach (var child in mapData.m_cellData_List)
        {
            
            //원래는 맵에디터에서 그라운드인지 배경인지 속성부여도 해야하는데 일단 임시로 이름가지고 속성 부여하자
            if (child.name == "ground1" || child.name == "ground2" || child.name.Contains("path"))
            {
                m_Tiles[child.x][child.y] = CreateTile(Tile_Type.Ground, new Vector2Int(child.x, child.y),
                    max_widthCount, max_heightCount);
                
            }
            else if(child.name.Contains("tree"))
            {
                //Debug.Log("tree tile create");
                m_Tiles[child.x][child.y] = CreateTile(Tile_Type.Charge, new Vector2Int(child.x, child.y),
                    max_widthCount, max_heightCount);
            }
                
            else
            {
                //m_Tiles[child.x][child.y].groundType = GroundType.Back;
                m_Tiles[child.x][child.y] = CreateTile(Tile_Type.Back, new Vector2Int(child.x, child.y),
                    max_widthCount, max_heightCount);
            }

            m_Tiles[child.x][child.y].BackSprite = sprite_dic[child.name];
            m_Tiles[child.x][child.y].Set_BackSpriteSize(CELL_SIZE);

            //tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().sprite = sprite_dic[child.name];
            //tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().size = new Vector2(CELL_SIZE, CELL_SIZE);

        }

        //        //위 오른쪽위 왼쪽위를 연결
        //        //만약 내 위치가 포탈위치면 상응하는 포탈위치를 등록해야 한다
        //        //포탈을 구현하지 않으므로 일단은 연결하고 일단 막히는 타일도 없다

        foreach (var child in mapData.m_cellData_List)
        {
            //위
            if (IsCorrectTilePos(m_Tiles, max_widthCount, max_heightCount, child.x, child.y + 1))
                m_Tiles[child.x][child.y].LinkedUpTile(m_Tiles[child.x][child.y + 1]);
            //오른쪽위
            if (IsCorrectTilePos(m_Tiles, max_widthCount, max_heightCount, child.x + 1, child.y + 1))
                m_Tiles[child.x][child.y].LinkedUpTile(m_Tiles[child.x + 1][child.y + 1]);
            //왼쪽위
            if (IsCorrectTilePos(m_Tiles, max_widthCount, max_heightCount, child.x - 1, child.y + 1))
                m_Tiles[child.x][child.y].LinkedUpTile(m_Tiles[child.x - 1][child.y + 1]);

            ////아래
            //if (IsCorrectTilePos(child.x, child.y - 1))
            //    m_Tiles[child.x][child.y].LinkedDownTile(m_Tiles[child.x][child.y - 1]);
            ////왼쪽아래
            //if (IsCorrectTilePos(child.x - 1, child.y - 1))
            //    m_Tiles[child.x][child.y].LinkedDownTile(m_Tiles[child.x - 1][child.y - 1]);
            ////오른쪽 아래
            //if (IsCorrectTilePos(child.x + 1, child.y - 1))
            //    m_Tiles[child.x][child.y].LinkedDownTile(m_Tiles[child.x + 1][child.y - 1]);
        }

        return m_Tiles;
    }

    private Tile CreateTile(Tile_Type type, Vector2Int coordinate, int width, int height)
    {
        GameObject obj = Instantiate(Tile_Prefab[(int)type]);
        
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(
            (width * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * coordinate.x,
            (height * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * coordinate.y);

        Tile tile = obj.GetComponent<Tile>();
        tile.Coordinate = new Vector2Int(coordinate.x, coordinate.y);
        return tile;
        
    }


    private bool IsCorrectTilePos(Tile[][] m_Tiles, int max_widthCount, int max_heightCount, int x, int y)
    {
        if (x < 0 || x >= max_widthCount || y < 0 || y >= max_heightCount)
            return false;
        if (m_Tiles[x][y].tileType == Tile_Type.Back)
            return false;
        return true;
    }

}
