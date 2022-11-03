using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrickType
{
    Empty = 0,
    Blue,
    Green,
    Orange,
    Purple,
    Red,
    Yellow,
    BrickEnd,
    Bomb
}

public enum Bomb
{
    None = 0,
    HorizontalBomb,
    VerticalBomb,
    Dynamite,
    ColorBomb
}

public enum GroundType
{
    Back = 0,
    Ground
}


public class GameField : MonoBehaviour
{

    public int action_count = 0;

    public GameObject Tile_Prefab;
    public GameObject[] BrickPrefabs;

    //private Dictionary<Vector2Int, Brick> m_brick_dic;
    //private BrickType[][] m_bricks;
    private Tile[][] m_Tiles;

    private int max_widthCount;
    private int max_heightCount;

    private delegate bool CheckMatchForCreate(Vector2Int pos, BrickType type);
    private CheckMatchForCreate[] CheckMatchForCreates_Del;

    //private Dictionary<Vector2Int, GameObject> m_BrickObj_Dic;

    //private MatchCheker m_matchCheker;

    private BrickController m_brickController;

    private const int CELL_SIZE = 100;

    private Dictionary<string, Sprite> sprite_dic;

    private void Awake()
    {
        //m_bricks = new BrickType[9][];
        
        CheckMatchForCreates_Del = new CheckMatchForCreate[10];

        CheckMatchForCreates_Del[0] = Check_3Brick_Left;
        CheckMatchForCreates_Del[1] = Check_3Brick_Right;
        CheckMatchForCreates_Del[2] = Check_3Brick_Up;
        CheckMatchForCreates_Del[3] = Check_3Brick_Down;
        CheckMatchForCreates_Del[4] = Check_4Brick_LU;
        CheckMatchForCreates_Del[5] = Check_4Brick_LD;
        CheckMatchForCreates_Del[6] = Check_4Brick_RU;
        CheckMatchForCreates_Del[7] = Check_4Brick_RD;
        CheckMatchForCreates_Del[8] = Check_3Brick_Row_Center;
        CheckMatchForCreates_Del[9] = Check_3Brick_Column_Center;

        //m_BrickObj_Dic = new Dictionary<Vector2Int, GameObject>();

        //m_matchCheker = new MatchCheker();
        m_brickController = new BrickController();
        m_brickController.MoveComplete_Callback = ClearBricks;

        sprite_dic = new Dictionary<string, Sprite>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 9; i++)
        //{
        //    m_bricks[i] = new BrickType[9];
        //    for (int j = 0; j < 9; j++)
        //    {
        //        m_bricks[i][j] = BrickType.Empty;
        //    }

        //}

        LoadMapData();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures");

        foreach (var child in sprites)
        {
            sprite_dic[child.name] = child;
        }

    }

    private void LoadMapData()
    {
        //TextAsset []mapJson = Resources.LoadAll<TextAsset>("MapJsonData");

        TextAsset mapJson = null;
        mapJson = Resources.Load<TextAsset>("MapJsonData/mapdata_0");

        if (mapJson == null)
        {
            Debug.Log("null");
            return;
        }
        MapData mapData = null;
        mapData = JsonUtility.FromJson<MapData>(mapJson.ToString());


        if (mapData == null)
        {
            Debug.Log("mapData null");
            return;
        }

        GameDataMGR.Instance.mapData = mapData;

    }

    public void CreateGameField()
    {
        m_Tiles = new Tile[9][];

        MapData mapData = GameDataMGR.Instance.mapData;
        max_widthCount = mapData.width_Count;
        max_heightCount = mapData.height_Count;

        for (int i = 0; i < max_widthCount; i++)
        {
            m_Tiles[i] = new Tile[max_heightCount];
            for (int j = 0; j < max_heightCount; j++)
            {
                GameObject obj = Instantiate(Tile_Prefab);
                m_Tiles[i][j] = obj.GetComponent<Tile>();
                m_Tiles[i][j].transform.SetParent(transform);
                m_Tiles[i][j].transform.localPosition = new Vector3(
                    (max_widthCount * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * i,
                    (max_heightCount * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * j);

            }

        }


        //일단 맵데이터 기준으로 생성

        foreach (var child in mapData.m_cellData_List)
        {
            m_Tiles[child.x][child.y].BackSprite = sprite_dic[child.name];
            m_Tiles[child.x][child.y].Set_BackSpriteSize(CELL_SIZE);
            //원래는 맵에디터에서 그라운드인지 배경인지 속성부여도 해야하는데 일단 임시로 이름가지고 속성 부여하자
            if (child.name == "ground1" || child.name == "ground2" || child.name.Contains("path"))
                m_Tiles[child.x][child.y].groundType = GroundType.Ground;
            else
                m_Tiles[child.x][child.y].groundType = GroundType.Back;
            //tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().sprite = sprite_dic[child.name];
            //tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().size = new Vector2(CELL_SIZE, CELL_SIZE);
        }
    }



    private bool Find_PositionableBrick(Vector2Int pos)
    {
        //해당위치가 주변 브릭들과 관계에서 가능한 위치인가 체크하자
        //해당위치가 브릭이 올려질수 있는 타일인가


        //3칸 가로(현 위치에서 좌우로 2칸 검사하는데 현재 타입과 달라야함)
        //left 2번 => -dx 두번, right두번  => +dx 두번 

        //적어도 하나 이상은 굴릴만한게 있어야 하는데 어떻게 넣지?

        //일단 블럭 랜덤하게 넣고 모양을 만들어야 하나?
        //모양 찾는 기능도 넣긴 해야 -> 이건 하나하나 다 패턴 검사해야 할거 같고
        //모양 만드는 기능은 어떻게?


        //일단 모양을 몇개 만들고 그담에 하나씩 체크해가며 브릭을 넣자
        //3개짜리를 일단 하나 넣는다. 그리고 랜덤하게 브릭을 넣는다?
        //3개가 안되면 4각형 하나를 넣는다.
        //각 브릭종류마다 최대 갯수는 정해 놓는다.(총 칸수 / 브릭수)

        //일단 랜덤하게 브릭 채우자 종류별 개수 상관없이 안지워지는 위치로
        //현위치 기준으로 어느방향이던 같은 타입으로 3개가 되면 안되고 사각형 모양 4개도 안됨.
        //상하 좌우 체크하고 사각형체크. 체크할때 제외할 모양은? 기준위치에서 상하좌우 두칸 더 체크해야

        //Bricktype을 랜덤으로 뽑자(뽑은건 다시 안뽑)

        if (m_Tiles[pos.x][pos.y].groundType == GroundType.Back)
            return false;

        if (m_Tiles[pos.x][pos.y].bricktype != BrickType.Empty)
            return false;

        

        Queue<BrickType> brickType_quque = Get_RandBrick();
        BrickType type = BrickType.Empty;

        int check_count = 0;

        while(brickType_quque.Count > 0)
        {
            type = brickType_quque.Dequeue();
            foreach (var child in CheckMatchForCreates_Del)
            {
                if (!child(pos, type))
                    break;
                check_count++;
            }

            if(check_count == 10)
            {
                //10개 검사 모두 만족한경우
                m_Tiles[pos.x][pos.y].bricktype = type;
                return true;
            }
            check_count = 0;
        }

        return false;
    }

    private Queue<BrickType> Get_RandBrick()
    {
        List<BrickType> list = new List<BrickType>();

        for(BrickType type = BrickType.Blue; type < BrickType.BrickEnd; type++)
        {
            list.Add(type);
        }

        Queue<BrickType> queue = new Queue<BrickType>();
        
        while(list.Count > 0)
        {
            int randNum = Random.Range(0, list.Count);
            BrickType type = (BrickType)list[randNum];
            queue.Enqueue(type);
            list.Remove(type);
        }

        return queue;
    }

    private bool Check_3Brick_Row_Center(Vector2Int pos, BrickType type)
    {
        //양옆에 2개
        int check_pos_x = pos.x;


        if (check_pos_x - 1 < 0 || check_pos_x + 1 >= max_widthCount)
            return true;

        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back)
            return true;


        if (m_Tiles[check_pos_x - 1][pos.y].bricktype == m_Tiles[check_pos_x + 1][pos.y].bricktype)
        {
            if (m_Tiles[check_pos_x - 1][pos.y].bricktype == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Column_Center(Vector2Int pos, BrickType type)
    {
        //위아래 2개
        int check_pos_y = pos.y;


        if (check_pos_y - 1 < 0 || check_pos_y + 1 >= max_widthCount)
            return true;

        if (m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back)
            return true;

        if (m_Tiles[pos.x][check_pos_y - 1].bricktype == m_Tiles[pos.x][check_pos_y + 1].bricktype)
        {
            if (m_Tiles[pos.x][check_pos_y - 1].bricktype == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Left(Vector2Int pos, BrickType type)
    {
        //left 2개
        int check_pos_x = pos.x;


        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0)
        {
            return true;
        }

        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x - 2][pos.y].groundType == GroundType.Back)
            return true;

        if(m_Tiles[check_pos_x - 1][pos.y].bricktype == m_Tiles[check_pos_x - 2][pos.y].bricktype)
        {
            if (m_Tiles[check_pos_x - 1][pos.y].bricktype == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Right(Vector2Int pos, BrickType type)
    {
        //right 2개
        int check_pos_x = pos.x;

        if (check_pos_x + 1 >= max_widthCount || check_pos_x + 2 >= max_widthCount)
            return true;

        if (m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 2][pos.y].groundType == GroundType.Back)
            return true;

        if (m_Tiles[check_pos_x + 1][pos.y].bricktype == m_Tiles[check_pos_x + 2][pos.y].bricktype)
        {
            if (m_Tiles[check_pos_x + 1][pos.y].bricktype == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Up(Vector2Int pos, BrickType type)
    {
        //up 2개
        int check_pos_y = pos.y;

        if (check_pos_y + 1 >= max_heightCount || check_pos_y + 2 >= max_heightCount)
            return true;

        if (m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y + 2].groundType == GroundType.Back)
            return true;

        if (m_Tiles[pos.x][check_pos_y + 1].bricktype == m_Tiles[pos.x][check_pos_y + 2].bricktype)
        {
            if (m_Tiles[pos.x][check_pos_y + 1].bricktype == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Down(Vector2Int pos, BrickType type)
    {
        //down 2개
        int check_pos_y = pos.y;


        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0)
            return true;

        if (m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y - 2].groundType == GroundType.Back)
            return true;

        if (m_Tiles[pos.x][check_pos_y - 1].bricktype == m_Tiles[pos.x][check_pos_y - 2].bricktype)
        {
            if (m_Tiles[pos.x][check_pos_y - 1].bricktype == type)
                return false;
        }

        return true;
    }

    private bool Check_4Brick_LU(Vector2Int pos, BrickType type)
    {
        //left up 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;


        if (check_pos_x - 1 < 0 || check_pos_y + 1 >= max_heightCount)
            return true;

        if (m_Tiles[check_pos_x - 1][check_pos_y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x - 1][check_pos_y + 1].groundType == GroundType.Back ||
            m_Tiles[check_pos_x][check_pos_y + 1].groundType == GroundType.Back)
            return true;

        if (m_Tiles[check_pos_x - 1][check_pos_y].bricktype == m_Tiles[check_pos_x - 1][check_pos_y + 1].bricktype ?
            m_Tiles[check_pos_x - 1][check_pos_y + 1].bricktype == m_Tiles[check_pos_x][check_pos_y + 1].bricktype : false)
        {
            if(m_Tiles[check_pos_x - 1][check_pos_y].bricktype == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_LD(Vector2Int pos, BrickType type)
    {
        //left down 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;


        if (check_pos_x - 1 < 0 || check_pos_y - 1 < 0)
            return true;

        if (m_Tiles[check_pos_x - 1][check_pos_y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x - 1][check_pos_y - 1].groundType == GroundType.Back ||
            m_Tiles[check_pos_x][check_pos_y - 1].groundType == GroundType.Back)
            return true;

        if (m_Tiles[check_pos_x - 1][check_pos_y].bricktype == m_Tiles[check_pos_x - 1][check_pos_y - 1].bricktype ?
           m_Tiles[check_pos_x - 1][check_pos_y - 1].bricktype == m_Tiles[check_pos_x][check_pos_y - 1].bricktype : false)
        {
            if (m_Tiles[check_pos_x - 1][check_pos_y].bricktype == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_RU(Vector2Int pos, BrickType type)
    {
        //right up 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;


        if (check_pos_x + 1 >= max_widthCount || check_pos_y + 1 >= max_heightCount)
            return true;

        if (m_Tiles[check_pos_x + 1][check_pos_y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 1][check_pos_y + 1].groundType == GroundType.Back ||
            m_Tiles[check_pos_x][check_pos_y + 1].groundType == GroundType.Back)
            return true;

        if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == m_Tiles[check_pos_x + 1][check_pos_y + 1].bricktype ?
           m_Tiles[check_pos_x + 1][check_pos_y + 1].bricktype == m_Tiles[check_pos_x][check_pos_y + 1].bricktype : false)
        {
            if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_RD(Vector2Int pos, BrickType type)
    {
        //right down 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;

        if (check_pos_x + 1 >= max_widthCount || check_pos_y - 1 < 0)
            return true;

        if (m_Tiles[check_pos_x + 1][check_pos_y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 1][check_pos_y - 1].groundType == GroundType.Back ||
            m_Tiles[check_pos_x][check_pos_y - 1].groundType == GroundType.Back)
            return true;

        if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == m_Tiles[check_pos_x + 1][check_pos_y - 1].bricktype ?
           m_Tiles[check_pos_x + 1][check_pos_y - 1].bricktype == m_Tiles[check_pos_x][check_pos_y - 1].bricktype : false)
        {
            if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == type)
            {
                return false;
            }
        }

        return true;

    }





    private bool Fill_3Brick(Vector2Int pos)
    {
        //일단 모양 3칸짜리 모양 만들자. 현재 위치 가로 또는 세로
        //oxoo 또는 ooxo
        //o o
        //x o
        //o x
        //o o
        if (m_Tiles[pos.x][pos.y].groundType == GroundType.Back)
            return false;

        if (m_Tiles[pos.x][pos.y].bricktype != BrickType.Empty)
            return false;

        //BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        //oxoo => left 1 right 1 right 2

        if (Check_3Brick_Row1(pos))
            return true;
        else if (Check_3Brick_Row2(pos))
            return true;
        else if (Check_3Brick_Column1(pos))
            return true;
        else if (Check_3Brick_Column2(pos))
            return true;
        return false;
        
    }

    //oxoo
    private bool Check_3Brick_Row1(Vector2Int pos)
    {
        int check_pos_x = pos.x;

        if (check_pos_x - 1 < 0 || check_pos_x + 1 >= max_widthCount
            || check_pos_x + 2 >= max_widthCount)
        {
            return false;
        }

        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 2][pos.y].groundType == GroundType.Back)
            return false;

        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_Tiles[check_pos_x - 1][pos.y].bricktype = type;
        m_Tiles[check_pos_x + 1][pos.y].bricktype = type;
        m_Tiles[check_pos_x + 2][pos.y].bricktype = type;

        return true;
    }

    //ooxo
    private bool Check_3Brick_Row2(Vector2Int pos)
    {
        int check_pos_x = pos.x;

        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0
            || check_pos_x + 1 >= max_widthCount)
        {
            return false;
        }

        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x - 2][pos.y].groundType == GroundType.Back ||
            m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back)
            return false;

        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_Tiles[check_pos_x - 1][pos.y].bricktype = type;
        m_Tiles[check_pos_x - 2][pos.y].bricktype = type;
        m_Tiles[check_pos_x + 1][pos.y].bricktype = type;

        return true;
    }

    //oxoo column
    private bool Check_3Brick_Column1(Vector2Int pos)
    {
        int check_pos_y = pos.y;

        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0
            || check_pos_y + 1 >= max_heightCount)
        {
            return false;
        }

        if (m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y - 2].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back)
            return false;

        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_Tiles[pos.x][check_pos_y - 1].bricktype = type;
        m_Tiles[pos.x][check_pos_y - 2].bricktype = type;
        m_Tiles[pos.x][check_pos_y + 1].bricktype = type;

        return true;
    }

    //ooxo column
    private bool Check_3Brick_Column2(Vector2Int pos)
    {
        int check_pos_y = pos.y;

        if (check_pos_y + 1 >= max_heightCount || check_pos_y + 2 >= max_heightCount
            || check_pos_y - 1 < 0)
        {
            return false;
        }

        if (m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y + 2].groundType == GroundType.Back ||
            m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back)
            return false;
        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_Tiles[pos.x][check_pos_y + 1].bricktype = type;
        m_Tiles[pos.x][check_pos_y + 2].bricktype = type;
        m_Tiles[pos.x][check_pos_y - 1].bricktype = type;

        return true;
    }


    public void Fill_Bricks()
    {

        //일단 맵 전체에 브릭 생성
        //브릭 생성시 지워지는 브릭이 안되도록 배치해야 한다

        //vector2 넣고 랜덤으로 가져오기

        //원래는 가능한 타일 위치를 가져와서 해야함

        Debug.Log("action count = " + action_count);

        m_brickController.StopTwinkle();


        //max_widthCount = GameDataMGR.Instance.MaxWidthCount;
        //max_heightCount = GameDataMGR.Instance.MaxHeightCount - 4;

        for(int i = 0; i < max_widthCount; i++)
        {
            for(int j = 0; j < max_heightCount; j++)
            {
                m_Tiles[i][j].bricktype = BrickType.Empty;
                if(m_Tiles[i][j].groundType == GroundType.Ground)
                    m_Tiles[i][j].Destroy();
            }
        }


        //Vector2[] brick_pos_vec = new Vector2[width_count * height_count];

        List<Vector2Int> brick_pos_list = new List<Vector2Int>();
        

        //list에 넣고 랜덤하게 뽑아오기

        for (int i = 0; i < max_heightCount; i++)
        {
            for(int j = 0; j < max_widthCount; j++)
            {
                //brick_pos_vec[i * height_count + j] = new Vector2(j, i);
                if(m_Tiles[j][i].groundType != GroundType.Back)
                    brick_pos_list.Add(new Vector2Int(j, i));
            }
        }

        

        while(brick_pos_list.Count > 0)
        {
            int range = brick_pos_list.Count;
            //Debug.Log("brick_pos_list count = " + range);
            Vector2Int pick_pos = brick_pos_list[Random.Range(0, range)];
            if (Fill_3Brick(pick_pos))
                break;
            brick_pos_list.Remove(pick_pos);
            //pick_pos에서 3개짜리 모양 만들수 있는지 체크

        }


        


        for (int i = 0; i < max_widthCount; i++)
        {
            for (int j = 0; j < max_heightCount; j++)
            {
                Find_PositionableBrick(new Vector2Int(i, j));
                CreateBrick(new Vector2((max_widthCount * 100 / -2.0f + 50) + 100 * i,
                    (max_heightCount * 100 / -2.0f + 50) + 100 * j), m_Tiles[i][j].bricktype, new Vector2Int(i, j));
            }
        }

        //swap_state = false;

    }


    private void CreateBrick(Vector2 pos, BrickType brickType, Vector2Int coordinate)
    {
        if (brickType == BrickType.Empty)
            return;
        GameObject obj = Instantiate(BrickPrefabs[(int)brickType]);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = pos;
        obj.GetComponent<Brick>().coordinate = coordinate;

        //특정 좌표에 있는 오브젝트를 알 필요가 있기때문에 리스트는 안된다

        //m_BrickObj_Dic[coordinate] = obj;

        m_Tiles[coordinate.x][coordinate.y].BrickObj = obj;
        //m_Tiles[coordinate.x][coordinate.y].bricktype = brickType;

        //m_BrickObj_List.Add(obj);



        //add obj

    }


    private GameObject GetTouchedBrick(Vector2 pos)
    {
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.collider.tag == "Brick")
        {
            return hit.collider.gameObject;
        }
        return null;

    }

    private bool Swap_Two_Brick_Check(GameObject first_obj, GameObject second_obj)
    {
        //두 오브젝트 스왑 무빙 요청 처리
        //바뀌어도 지워지는게 없으면 다시 제자리로
        //바뀌어서 지워지면 그다음 프로세스

        //일단 스왑부터 해보자. 지워지는건지 미리 계산을 해놓고 무빙 해야하는지 아니면 한번 이동을 하고
        //지워지는거 계산을 한후에 안지워지면 다시 역무빙을 해야하는지...
        //일단 스왑부터 해보자

        //두번째 오브젝트가 대각선이나 건너뛴 오브젝트일수 있다.
        //왼쪽 대각선은 왼쪽으로 오른쪽 대각선은 오른쪽
        //건너뛴 오브젝트면 그방향 한칸이다

        //Debug.Log("swap func call");

        if (first_obj.GetComponent<Brick>().coordinate == second_obj.GetComponent<Brick>().coordinate)
            return false;

        Debug.Log("first obj coordinate = " + first_obj.GetComponent<Brick>().coordinate);
        Debug.Log("second obj coordinate = " + second_obj.GetComponent<Brick>().coordinate);

        //같은색을 스왑하는건 의미없음
        action_count++;
        m_brickController.BrickSwap(m_Tiles, first_obj.GetComponent<Brick>(), second_obj.GetComponent<Brick>());
        

        //이동이 완료되면 매치체킹해야함. 




        //지워지는 모양인지 체크하고 무빙
        //모든모양 체크
        //3칸 세로, 3칸 가로, 4칸 세로, 4칸 가로, 5칸 세로, 5칸 가로
        //4칸 사각형 4방향, 5칸 ㄱ자 4방향, T모양 5칸 4방향
        //지워지는 브릭 좌표 다 넣고 (중복안되게)

        return false;

    }

    

    private void ClearBricks(MatchResult result)
    {

        StartCoroutine(temp_cor(result));
        
    }

    IEnumerator temp_cor(MatchResult result)
    {
        yield return new WaitForSeconds(0.1f);

        action_count--;
        foreach (var child in result.ClearCoordinateList)
        {
            //삭제가아니라 숨기자 어차피 브릭 재활용 해야함 
            //m_Tiles[child.x][child.y].Destroy();
            m_Tiles[child.x][child.y].BrickObj.transform.position = new Vector3(-3000, -3000);
        }
    }


    public void Find_Shape()
    {
        m_brickController.FindShape(m_Tiles);

        //모양 없으면 브릭 재배치 해야함

    }

    

    private GameObject begin_selected_obj;
    private GameObject second_selected_obj;
    //private bool swap_state = false;
    // Update is called once per frame
    void Update()
    {
        //Touch event


        if(Input.touchCount > 0 && action_count == 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            Vector2 pos = Camera.main.ScreenToWorldPoint(touchPos);



            switch(touch.phase)
            {
                case TouchPhase.Began:
                    
                    begin_selected_obj = GetTouchedBrick(pos);

                    if(begin_selected_obj != null)
                    {
                        m_brickController.StopTwinkle();
                    }
                    
                    break;

                case TouchPhase.Moved:
                    //선택된 브릭이 있어야하고 이미 브릭이 이동상태가 아니어야한다.
                    //선택된 브릭이 있는 상태로 이동해서 브릭이 바뀌면 이동이라 판단.
                    if (begin_selected_obj != null)
                    {

                        //대각선이 선택될 수도 있기때문에 보정이 필요하다.
                        //손가락을 빠르게 이동하면 한칸 건너서 선택되기도 한다.
                        //
                        second_selected_obj = GetTouchedBrick(pos);

                        if (second_selected_obj != null && second_selected_obj != begin_selected_obj)
                        {
                            //스왑 상태로 상태변경 할 것.
                            //브릭 스왑 진행하고 스왑이 완료되거나(아무것도 못지운경우)
                            //지우거나 아이템 효과가 끝나는 등 모든 프로세스 진행이 완료되면 다시 상태 변경
                            Swap_Two_Brick_Check(begin_selected_obj, second_selected_obj);
                        }
                    }
                    break;
                     

                case TouchPhase.Ended:
                    if(begin_selected_obj != null)
                        begin_selected_obj = null;
                    break;
            }

            
        }

        
        
    }
}
