//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum BrickType
//{
//    Empty = 0,
//    Blue,
//    Green,
//    Orange,
//    Purple,
//    Red,
//    Yellow,
//    BrickEnd,
//}

//public enum Bomb
//{
//    None = 0,
//    HorizontalBomb,
//    VerticalBomb,
//    Dynamite,
//    ColorBomb
//}

//public enum GroundType
//{
//    Back = 0,
//    Ground
//}

//public enum ObjectType
//{
//    None = 0,
//    Brick,
//    Bomb
//}


//public class GameField : MonoBehaviour
//{

//    public int action_count = 0;

//    public GameObject Tile_Prefab;
//    public GameObject[] BrickPrefabs;

//    //private Dictionary<Vector2Int, Brick> m_brick_dic;
//    //private BrickType[][] m_bricks;
//    private Tile[][] m_Tiles;

//    private int max_widthCount;
//    private int max_heightCount;

//    private delegate bool CheckMatchForCreate(Vector2Int pos, BrickType type);
//    private CheckMatchForCreate[] CheckMatchForCreates_Del;

//    //private Dictionary<Vector2Int, GameObject> m_BrickObj_Dic;

//    private MatchCheker m_matchChecker;

//    private BrickController m_brickController;


//    private const int CELL_SIZE = 100;

//    private Dictionary<string, Sprite> sprite_dic;

//    List<MatchResult> m_MatchResultList;

//    TileSnakeController m_TileSnakeController;

//    private void Awake()
//    {
//        //m_bricks = new BrickType[9][];
        
//        CheckMatchForCreates_Del = new CheckMatchForCreate[10];

//        CheckMatchForCreates_Del[0] = Check_3Brick_Left;
//        CheckMatchForCreates_Del[1] = Check_3Brick_Right;
//        CheckMatchForCreates_Del[2] = Check_3Brick_Up;
//        CheckMatchForCreates_Del[3] = Check_3Brick_Down;
//        CheckMatchForCreates_Del[4] = Check_4Brick_LU;
//        CheckMatchForCreates_Del[5] = Check_4Brick_LD;
//        CheckMatchForCreates_Del[6] = Check_4Brick_RU;
//        CheckMatchForCreates_Del[7] = Check_4Brick_RD;
//        CheckMatchForCreates_Del[8] = Check_3Brick_Row_Center;
//        CheckMatchForCreates_Del[9] = Check_3Brick_Column_Center;

//        //m_BrickObj_Dic = new Dictionary<Vector2Int, GameObject>();

//        m_matchChecker = new MatchCheker();
        
//        m_brickController = new BrickController();
        

//        m_MatchResultList = new List<MatchResult>();
//        m_TileSnakeController = new TileSnakeController();

//        sprite_dic = new Dictionary<string, Sprite>();
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        //for (int i = 0; i < 9; i++)
//        //{
//        //    m_bricks[i] = new BrickType[9];
//        //    for (int j = 0; j < 9; j++)
//        //    {
//        //        m_bricks[i][j] = BrickType.Empty;
//        //    }

//        //}

//        LoadMapData();

//        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures");

//        foreach (var child in sprites)
//        {
//            sprite_dic[child.name] = child;
//        }

//        m_matchChecker.Init();

//    }

//    private void LoadMapData()
//    {
//        //TextAsset []mapJson = Resources.LoadAll<TextAsset>("MapJsonData");

//        TextAsset mapJson = null;
//        mapJson = Resources.Load<TextAsset>("MapJsonData/mapdata_0");

//        if (mapJson == null)
//        {
//            Debug.Log("null");
//            return;
//        }
//        MapData mapData = null;
//        mapData = JsonUtility.FromJson<MapData>(mapJson.ToString());


//        if (mapData == null)
//        {
//            Debug.Log("mapData null");
//            return;
//        }

//        GameDataMGR.Instance.mapData = mapData;

//    }

//    private bool IsCorrectTilePos(int x, int y)
//    {
//        if (x < 0 || x >= max_widthCount || y < 0 || y >= max_heightCount)
//            return false;
//        if (m_Tiles[x][y].groundType == GroundType.Back)
//            return false;
//        return true;
//    }

//    public void CreateGameField()
//    {

//        MapData mapData = GameDataMGR.Instance.mapData;
//        max_widthCount = mapData.width_Count;
//        max_heightCount = mapData.height_Count;

//        m_Tiles = new Tile[max_widthCount][];


//        for (int i = 0; i < max_widthCount; i++)
//        {
//            m_Tiles[i] = new Tile[max_heightCount];
//            for (int j = 0; j < max_heightCount; j++)
//            {
//                GameObject obj = Instantiate(Tile_Prefab);
//                m_Tiles[i][j] = obj.GetComponent<Tile>();
//                m_Tiles[i][j].transform.SetParent(transform);
//                m_Tiles[i][j].transform.localPosition = new Vector3(
//                    (max_widthCount * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * i,
//                    (max_heightCount * CELL_SIZE / -2.0f + CELL_SIZE / 2) + CELL_SIZE * j);


//                m_Tiles[i][j].Coordinate = new Vector2Int(i, j);
//            }

//        }


//        //일단 맵데이터 기준으로 생성

//        foreach (var child in mapData.m_cellData_List)
//        {
//            m_Tiles[child.x][child.y].BackSprite = sprite_dic[child.name];
//            m_Tiles[child.x][child.y].Set_BackSpriteSize(CELL_SIZE);
//            //원래는 맵에디터에서 그라운드인지 배경인지 속성부여도 해야하는데 일단 임시로 이름가지고 속성 부여하자
//            if (child.name == "ground1" || child.name == "ground2" || child.name.Contains("path"))
//                m_Tiles[child.x][child.y].groundType = GroundType.Ground;
//            else
//                m_Tiles[child.x][child.y].groundType = GroundType.Back;
//            //tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().sprite = sprite_dic[child.name];
//            //tile_Map[child.x][child.y].GetComponent<SpriteRenderer>().size = new Vector2(CELL_SIZE, CELL_SIZE);

            
            
//        }

//        //위 오른쪽위 왼쪽위를 연결
//        //만약 내 위치가 포탈위치면 상응하는 포탈위치를 등록해야 한다
//        //포탈을 구현하지 않으므로 일단은 연결하고 일단 막히는 타일도 없다

//        foreach (var child in mapData.m_cellData_List)
//        {
//            //위
//            if (IsCorrectTilePos(child.x, child.y + 1))
//                m_Tiles[child.x][child.y].LinkedUpTile(m_Tiles[child.x][child.y + 1]);
//            //오른쪽위
//            if (IsCorrectTilePos(child.x + 1, child.y + 1))
//                m_Tiles[child.x][child.y].LinkedUpTile(m_Tiles[child.x + 1][child.y + 1]);
//            //왼쪽위
//            if (IsCorrectTilePos(child.x - 1, child.y + 1))
//                m_Tiles[child.x][child.y].LinkedUpTile(m_Tiles[child.x - 1][child.y + 1]);

//            //아래
//            if (IsCorrectTilePos(child.x, child.y - 1))
//                m_Tiles[child.x][child.y].LinkedDownTile(m_Tiles[child.x][child.y - 1]);
//            //왼쪽아래
//            if (IsCorrectTilePos(child.x - 1, child.y - 1))
//                m_Tiles[child.x][child.y].LinkedDownTile(m_Tiles[child.x - 1][child.y - 1]);
//            //오른쪽 아래
//            if (IsCorrectTilePos(child.x + 1, child.y - 1))
//                m_Tiles[child.x][child.y].LinkedDownTile(m_Tiles[child.x + 1][child.y - 1]);
//        }
//    }




//    private bool Find_PositionableBrick(Vector2Int pos)
//    {
//        //해당위치가 주변 브릭들과 관계에서 가능한 위치인가 체크하자
//        //해당위치가 브릭이 올려질수 있는 타일인가


//        //3칸 가로(현 위치에서 좌우로 2칸 검사하는데 현재 타입과 달라야함)
//        //left 2번 => -dx 두번, right두번  => +dx 두번 

//        //적어도 하나 이상은 굴릴만한게 있어야 하는데 어떻게 넣지?

//        //일단 블럭 랜덤하게 넣고 모양을 만들어야 하나?
//        //모양 찾는 기능도 넣긴 해야 -> 이건 하나하나 다 패턴 검사해야 할거 같고
//        //모양 만드는 기능은 어떻게?


//        //일단 모양을 몇개 만들고 그담에 하나씩 체크해가며 브릭을 넣자
//        //3개짜리를 일단 하나 넣는다. 그리고 랜덤하게 브릭을 넣는다?
//        //3개가 안되면 4각형 하나를 넣는다.
//        //각 브릭종류마다 최대 갯수는 정해 놓는다.(총 칸수 / 브릭수)

//        //일단 랜덤하게 브릭 채우자 종류별 개수 상관없이 안지워지는 위치로
//        //현위치 기준으로 어느방향이던 같은 타입으로 3개가 되면 안되고 사각형 모양 4개도 안됨.
//        //상하 좌우 체크하고 사각형체크. 체크할때 제외할 모양은? 기준위치에서 상하좌우 두칸 더 체크해야

//        //Bricktype을 랜덤으로 뽑자(뽑은건 다시 안뽑)

//        if (m_Tiles[pos.x][pos.y].groundType == GroundType.Back)
//            return false;

//        if (m_Tiles[pos.x][pos.y].bricktype != BrickType.Empty)
//            return false;


//        Queue<BrickType> brickType_quque = Get_RandBrick();
//        BrickType type = BrickType.Empty;

//        int check_count = 0;

//        while(brickType_quque.Count > 0)
//        {
//            type = brickType_quque.Dequeue();
//            foreach (var child in CheckMatchForCreates_Del)
//            {
//                if (!child(pos, type))
//                    break;
//                check_count++;
//            }

//            if(check_count == 10)
//            {
//                //10개 검사 모두 만족한경우
//                m_Tiles[pos.x][pos.y].bricktype = type;
//                return true;
//            }
//            check_count = 0;
//        }

//        return false;
//    }

//    private Queue<BrickType> Get_RandBrick()
//    {
//        List<BrickType> list = new List<BrickType>();

//        for(BrickType type = BrickType.Blue; type < BrickType.BrickEnd; type++)
//        {
//            list.Add(type);
//        }

//        Queue<BrickType> queue = new Queue<BrickType>();
        
//        while(list.Count > 0)
//        {
//            int randNum = Random.Range(0, list.Count);
//            BrickType type = (BrickType)list[randNum];
//            queue.Enqueue(type);
//            list.Remove(type);
//        }

//        return queue;
//    }

//    private bool Check_3Brick_Row_Center(Vector2Int pos, BrickType type)
//    {
//        //양옆에 2개
//        int check_pos_x = pos.x;


//        if (check_pos_x - 1 < 0 || check_pos_x + 1 >= max_widthCount)
//            return true;

//        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back)
//            return true;


//        if (m_Tiles[check_pos_x - 1][pos.y].bricktype == m_Tiles[check_pos_x + 1][pos.y].bricktype)
//        {
//            if (m_Tiles[check_pos_x - 1][pos.y].bricktype == type)
//                return false;
//        }

//        return true;
//    }

//    private bool Check_3Brick_Column_Center(Vector2Int pos, BrickType type)
//    {
//        //위아래 2개
//        int check_pos_y = pos.y;


//        if (check_pos_y - 1 < 0 || check_pos_y + 1 >= max_heightCount)
//            return true;

//        if (m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[pos.x][check_pos_y - 1].bricktype == m_Tiles[pos.x][check_pos_y + 1].bricktype)
//        {
//            if (m_Tiles[pos.x][check_pos_y - 1].bricktype == type)
//                return false;
//        }

//        return true;
//    }

//    private bool Check_3Brick_Left(Vector2Int pos, BrickType type)
//    {
//        //left 2개
//        int check_pos_x = pos.x;


//        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0)
//        {
//            return true;
//        }

//        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x - 2][pos.y].groundType == GroundType.Back)
//            return true;

//        if(m_Tiles[check_pos_x - 1][pos.y].bricktype == m_Tiles[check_pos_x - 2][pos.y].bricktype)
//        {
//            if (m_Tiles[check_pos_x - 1][pos.y].bricktype == type)
//                return false;
//        }

//        return true;
//    }

//    private bool Check_3Brick_Right(Vector2Int pos, BrickType type)
//    {
//        //right 2개
//        int check_pos_x = pos.x;

//        if (check_pos_x + 1 >= max_widthCount || check_pos_x + 2 >= max_widthCount)
//            return true;

//        if (m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 2][pos.y].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[check_pos_x + 1][pos.y].bricktype == m_Tiles[check_pos_x + 2][pos.y].bricktype)
//        {
//            if (m_Tiles[check_pos_x + 1][pos.y].bricktype == type)
//                return false;
//        }

//        return true;
//    }

//    private bool Check_3Brick_Up(Vector2Int pos, BrickType type)
//    {
//        //up 2개
//        int check_pos_y = pos.y;

//        if (check_pos_y + 1 >= max_heightCount || check_pos_y + 2 >= max_heightCount)
//            return true;

//        if (m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y + 2].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[pos.x][check_pos_y + 1].bricktype == m_Tiles[pos.x][check_pos_y + 2].bricktype)
//        {
//            if (m_Tiles[pos.x][check_pos_y + 1].bricktype == type)
//                return false;
//        }

//        return true;
//    }

//    private bool Check_3Brick_Down(Vector2Int pos, BrickType type)
//    {
//        //down 2개
//        int check_pos_y = pos.y;


//        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0)
//            return true;

//        if (m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y - 2].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[pos.x][check_pos_y - 1].bricktype == m_Tiles[pos.x][check_pos_y - 2].bricktype)
//        {
//            if (m_Tiles[pos.x][check_pos_y - 1].bricktype == type)
//                return false;
//        }

//        return true;
//    }

//    private bool Check_4Brick_LU(Vector2Int pos, BrickType type)
//    {
//        //left up 3개
//        int check_pos_x = pos.x;
//        int check_pos_y = pos.y;


//        if (check_pos_x - 1 < 0 || check_pos_y + 1 >= max_heightCount)
//            return true;

//        if (m_Tiles[check_pos_x - 1][check_pos_y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x - 1][check_pos_y + 1].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x][check_pos_y + 1].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[check_pos_x - 1][check_pos_y].bricktype == m_Tiles[check_pos_x - 1][check_pos_y + 1].bricktype ?
//            m_Tiles[check_pos_x - 1][check_pos_y + 1].bricktype == m_Tiles[check_pos_x][check_pos_y + 1].bricktype : false)
//        {
//            if(m_Tiles[check_pos_x - 1][check_pos_y].bricktype == type)
//            {
//                return false;
//            }
//        }

//        return true;

//    }

//    private bool Check_4Brick_LD(Vector2Int pos, BrickType type)
//    {
//        //left down 3개
//        int check_pos_x = pos.x;
//        int check_pos_y = pos.y;


//        if (check_pos_x - 1 < 0 || check_pos_y - 1 < 0)
//            return true;

//        if (m_Tiles[check_pos_x - 1][check_pos_y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x - 1][check_pos_y - 1].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x][check_pos_y - 1].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[check_pos_x - 1][check_pos_y].bricktype == m_Tiles[check_pos_x - 1][check_pos_y - 1].bricktype ?
//           m_Tiles[check_pos_x - 1][check_pos_y - 1].bricktype == m_Tiles[check_pos_x][check_pos_y - 1].bricktype : false)
//        {
//            if (m_Tiles[check_pos_x - 1][check_pos_y].bricktype == type)
//            {
//                return false;
//            }
//        }

//        return true;

//    }

//    private bool Check_4Brick_RU(Vector2Int pos, BrickType type)
//    {
//        //right up 3개
//        int check_pos_x = pos.x;
//        int check_pos_y = pos.y;


//        if (check_pos_x + 1 >= max_widthCount || check_pos_y + 1 >= max_heightCount)
//            return true;

//        if (m_Tiles[check_pos_x + 1][check_pos_y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 1][check_pos_y + 1].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x][check_pos_y + 1].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == m_Tiles[check_pos_x + 1][check_pos_y + 1].bricktype ?
//           m_Tiles[check_pos_x + 1][check_pos_y + 1].bricktype == m_Tiles[check_pos_x][check_pos_y + 1].bricktype : false)
//        {
//            if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == type)
//            {
//                return false;
//            }
//        }

//        return true;

//    }

//    private bool Check_4Brick_RD(Vector2Int pos, BrickType type)
//    {
//        //right down 3개
//        int check_pos_x = pos.x;
//        int check_pos_y = pos.y;

//        if (check_pos_x + 1 >= max_widthCount || check_pos_y - 1 < 0)
//            return true;

//        if (m_Tiles[check_pos_x + 1][check_pos_y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 1][check_pos_y - 1].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x][check_pos_y - 1].groundType == GroundType.Back)
//            return true;

//        if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == m_Tiles[check_pos_x + 1][check_pos_y - 1].bricktype ?
//           m_Tiles[check_pos_x + 1][check_pos_y - 1].bricktype == m_Tiles[check_pos_x][check_pos_y - 1].bricktype : false)
//        {
//            if (m_Tiles[check_pos_x + 1][check_pos_y].bricktype == type)
//            {
//                return false;
//            }
//        }

//        return true;

//    }





//    private bool Fill_3Brick(Vector2Int pos)
//    {
//        //일단 모양 3칸짜리 모양 만들자. 현재 위치 가로 또는 세로
//        //oxoo 또는 ooxo
//        //o o
//        //x o
//        //o x
//        //o o
//        if (m_Tiles[pos.x][pos.y].groundType == GroundType.Back)
//            return false;

//        if (m_Tiles[pos.x][pos.y].bricktype != BrickType.Empty)
//            return false;

//        //BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
//        //oxoo => left 1 right 1 right 2

//        if (Check_3Brick_Row1(pos))
//            return true;
//        else if (Check_3Brick_Row2(pos))
//            return true;
//        else if (Check_3Brick_Column1(pos))
//            return true;
//        else if (Check_3Brick_Column2(pos))
//            return true;
//        return false;
        
//    }

//    //oxoo
//    private bool Check_3Brick_Row1(Vector2Int pos)
//    {
//        int check_pos_x = pos.x;

//        if (check_pos_x - 1 < 0 || check_pos_x + 1 >= max_widthCount
//            || check_pos_x + 2 >= max_widthCount)
//        {
//            return false;
//        }

//        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 2][pos.y].groundType == GroundType.Back)
//            return false;

//        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
//        m_Tiles[check_pos_x - 1][pos.y].bricktype = type;
//        m_Tiles[check_pos_x + 1][pos.y].bricktype = type;
//        m_Tiles[check_pos_x + 2][pos.y].bricktype = type;

//        return true;
//    }

//    //ooxo
//    private bool Check_3Brick_Row2(Vector2Int pos)
//    {
//        int check_pos_x = pos.x;

//        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0
//            || check_pos_x + 1 >= max_widthCount)
//        {
//            return false;
//        }

//        if (m_Tiles[check_pos_x - 1][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x - 2][pos.y].groundType == GroundType.Back ||
//            m_Tiles[check_pos_x + 1][pos.y].groundType == GroundType.Back)
//            return false;

//        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
//        m_Tiles[check_pos_x - 1][pos.y].bricktype = type;
//        m_Tiles[check_pos_x - 2][pos.y].bricktype = type;
//        m_Tiles[check_pos_x + 1][pos.y].bricktype = type;

//        return true;
//    }

//    //oxoo column
//    private bool Check_3Brick_Column1(Vector2Int pos)
//    {
//        int check_pos_y = pos.y;

//        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0
//            || check_pos_y + 1 >= max_heightCount)
//        {
//            return false;
//        }

//        if (m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y - 2].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back)
//            return false;

//        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
//        m_Tiles[pos.x][check_pos_y - 1].bricktype = type;
//        m_Tiles[pos.x][check_pos_y - 2].bricktype = type;
//        m_Tiles[pos.x][check_pos_y + 1].bricktype = type;

//        return true;
//    }

//    //ooxo column
//    private bool Check_3Brick_Column2(Vector2Int pos)
//    {
//        int check_pos_y = pos.y;

//        if (check_pos_y + 1 >= max_heightCount || check_pos_y + 2 >= max_heightCount
//            || check_pos_y - 1 < 0)
//        {
//            return false;
//        }

//        if (m_Tiles[pos.x][check_pos_y + 1].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y + 2].groundType == GroundType.Back ||
//            m_Tiles[pos.x][check_pos_y - 1].groundType == GroundType.Back)
//            return false;
//        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
//        m_Tiles[pos.x][check_pos_y + 1].bricktype = type;
//        m_Tiles[pos.x][check_pos_y + 2].bricktype = type;
//        m_Tiles[pos.x][check_pos_y - 1].bricktype = type;

//        return true;
//    }


//    public void Fill_Bricks()
//    {

//        //일단 맵 전체에 브릭 생성
//        //브릭 생성시 지워지는 브릭이 안되도록 배치해야 한다

//        //vector2 넣고 랜덤으로 가져오기

//        //원래는 가능한 타일 위치를 가져와서 해야함

//        //Debug.Log("action count = " + action_count);

//        m_brickController.StopTwinkle();


//        //max_widthCount = GameDataMGR.Instance.MaxWidthCount;
//        //max_heightCount = GameDataMGR.Instance.MaxHeightCount - 4;

//        for(int i = 0; i < max_widthCount; i++)
//        {
//            for(int j = 0; j < max_heightCount; j++)
//            {
//                m_Tiles[i][j].bricktype = BrickType.Empty;
//                if(m_Tiles[i][j].groundType == GroundType.Ground)
//                    m_Tiles[i][j].Destroy();
//            }
//        }


//        //Vector2[] brick_pos_vec = new Vector2[width_count * height_count];

//        List<Vector2Int> brick_pos_list = new List<Vector2Int>();
        

//        //list에 넣고 랜덤하게 뽑아오기

//        for (int i = 0; i < max_heightCount; i++)
//        {
//            for(int j = 0; j < max_widthCount; j++)
//            {
//                //brick_pos_vec[i * height_count + j] = new Vector2(j, i);
//                if(m_Tiles[j][i].groundType != GroundType.Back)
//                    brick_pos_list.Add(new Vector2Int(j, i));
//            }
//        }

        

//        while(brick_pos_list.Count > 0)
//        {
//            int range = brick_pos_list.Count;
//            //Debug.Log("brick_pos_list count = " + range);
//            Vector2Int pick_pos = brick_pos_list[Random.Range(0, range)];
//            if (Fill_3Brick(pick_pos))
//                break;
//            brick_pos_list.Remove(pick_pos);
//            //pick_pos에서 3개짜리 모양 만들수 있는지 체크

//        }


//        for (int i = 0; i < max_widthCount; i++)
//        {
//            for (int j = 0; j < max_heightCount; j++)
//            {
//                Find_PositionableBrick(new Vector2Int(i, j));
//                CreateBrick(new Vector2((max_widthCount * 100 / -2.0f + 50) + 100 * i,
//                    (max_heightCount * 100 / -2.0f + 50) + 100 * j), m_Tiles[i][j].bricktype, new Vector2Int(i, j));
//            }
//        }

//        //swap_state = false;

//    }


//    private void CreateBrick(Vector2 pos, BrickType brickType, Vector2Int coordinate)
//    {
//        if (brickType == BrickType.Empty)
//            return;
//        GameObject obj = Instantiate(BrickPrefabs[(int)brickType]);
//        obj.transform.SetParent(transform);
//        obj.transform.localPosition = pos;
//        //obj.GetComponent<Brick>().coordinate = coordinate;

//        //특정 좌표에 있는 오브젝트를 알 필요가 있기때문에 리스트는 안된다

//        //m_BrickObj_Dic[coordinate] = obj;

//        m_Tiles[coordinate.x][coordinate.y].BrickScript = obj.GetComponent<Brick>();
//        //m_Tiles[coordinate.x][coordinate.y].bricktype = brickType;

//        //m_BrickObj_List.Add(obj);



//        //add obj

//    }


//    private GameObject GetTouchedBrick(Vector2 pos)
//    {
//        Ray2D ray = new Ray2D(pos, Vector2.zero);
//        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
//        if (hit.collider != null && hit.collider.tag == "Brick")
//        {
//            return hit.collider.gameObject;
//        }
//        return null;

//    }

//    private bool Swap_Two_Brick_Check(GameObject first_obj, GameObject second_obj)
//    {
//        //두 오브젝트 스왑 무빙 요청 처리
//        //바뀌어도 지워지는게 없으면 다시 제자리로
//        //바뀌어서 지워지면 그다음 프로세스

//        //일단 스왑부터 해보자. 지워지는건지 미리 계산을 해놓고 무빙 해야하는지 아니면 한번 이동을 하고
//        //지워지는거 계산을 한후에 안지워지면 다시 역무빙을 해야하는지...
//        //일단 스왑부터 해보자

//        //두번째 오브젝트가 대각선이나 건너뛴 오브젝트일수 있다.
//        //왼쪽 대각선은 왼쪽으로 오른쪽 대각선은 오른쪽
//        //건너뛴 오브젝트면 그방향 한칸이다

        
//        if (first_obj.GetComponent<Brick>().coordinate == second_obj.GetComponent<Brick>().coordinate)
//            return false;



//        //먼저 임의로 타일의 브릭만 스왑을 한다
//        //매치 체크를 한다
//        //결과에 따라 브릭 이동을 한다

//        //좌우측 우선임
//        Brick src_brick = first_obj.GetComponent<Brick>();
//        Brick dst_brick = second_obj.GetComponent<Brick>();
//        Vector2Int src_coordinate = src_brick.coordinate;
//        Vector2Int dst_coordinate = dst_brick.coordinate;
//        int dx = dst_coordinate.x - src_coordinate.x;
//        int dy = dst_coordinate.y - src_coordinate.y;

//        MoveDir dir;
//        if (dx != 0)
//        {
//            dx = dx > 0 ? 1 : -1;
//            dir = dx == 1 ? MoveDir.Right : MoveDir.Left;
//            dy = 0;
//        }
//        else
//        {
//            dy = dy > 0 ? 1 : -1;
//            dir = dy == 1 ? MoveDir.Up : MoveDir.Down;
//            dx = 0;
//        }


//        dst_brick = m_Tiles[src_coordinate.x + dx][src_coordinate.y + dy].BrickScript;
//        dst_coordinate = dst_brick.coordinate;

        
//        //m_brickController.SwapTwoBrick(m_Tiles, src_coordinate, dst_coordinate, dir);


//        //action_count++;
//        //Swap_Two_Brick_Only_ObjData(src_coordinate, dst_coordinate);

//        ////스왑되는 방향을 넣어서 검사 효율을 높인다
//        ////매치 결과를 받아온다
//        //m_MatchResultList = m_matchChecker.MatchCheck_SwapTwoPoint(m_Tiles, dir, dst_coordinate, src_coordinate);

//        ////매치 결과에 따라 편도, 왕복 브릭 이동을 한다
//        //if(m_MatchResultList != null)
//        //{
//        //    //먼저 브릭 스왑 이동을 한다
//        //    //이동이 끝나면 매치결과에 따른 브릭을 지운다
//        //    m_brickController.TwoBrickSwapMove(src_brick, dst_brick,
//        //        m_Tiles[src_coordinate.x][src_coordinate.y].transform.localPosition,
//        //        m_Tiles[dst_coordinate.x][dst_coordinate.y].transform.localPosition, SwapMoveCallback);
//        //}
//        //else
//        //{
//        //    //매치결과 null
//        //    //임의로 스왑했던 브릭을 제자리로 돌려 놓는다
//        //    //브릭 왕복 이동
//        //    Swap_Two_Brick_Only_ObjData(dst_coordinate, src_coordinate);
//        //    m_brickController.TwoBrickReturnMove(src_brick, dst_brick,
//        //        m_Tiles[src_coordinate.x][src_coordinate.y].transform.localPosition,
//        //        m_Tiles[dst_coordinate.x][dst_coordinate.y].transform.localPosition, SwapReturnMoveCallback);

//        //}

//        //같은색을 스왑하는건 의미없음



//        return false;

//    }

//    private void SwapReturnMoveCallback()
//    {
//        action_count --;
//    }

//    private void EraseBrick()
//    {

//    }

//    private void SwapMoveCallback()
//    {
//        //스왑이 완료 됐고 매치 결과가 있는 상태
//        //결과에 따라 브릭을 지운다
//        //타일 타입은 empty로 변경
//        List<Vector2Int> list = new List<Vector2Int>();
//        foreach (var matchResult in m_MatchResultList)
//        {
//            List<Brick> brickList = new List<Brick>();
//            foreach(var tile in matchResult.ClearTileList)
//            {
//                tile.bricktype = BrickType.Empty;
//                brickList.Add(tile.BrickScript);
//                if(!list.Contains(tile.Coordinate))
//                    list.Add(tile.Coordinate);
//                //Debug.Log("BrickType = " + m_Tiles[tile.BrickScript.coordinate.x]
//                //    [tile.BrickScript.coordinate.y].bricktype);
//            }
//            m_brickController.EraseBricks(brickList);
//        }
//        //브릭을 지우면 빈공간이 생기는데 그곳에서 tileSnake를 찾는다

//        //tilesnake를 찾는다
//        TileSnake tileSnake = m_TileSnakeController.SearchTileSanke(m_Tiles, list);

//        //tileSnake = m_brickController.MovingBricksDataIntoEmptySpace(tileSnake);
//        StartCoroutine(temp_cor(tileSnake));

//        //moving test


//        //tilesnake 머리부터 한칸씩 이동
//        //한칸씩 이동후에 머리부터 이동완료 체크하면서 snake에서 자르고 검사리스트(좌표 중복 안되게 처리할 것)를 리턴한다
//        //받은 검사리스트로 다시 매치체크한다
//        //매치가되면 지우고 다시 snake를 추가한다
//        //매치체크하면 matchresult가 또 생기겠지. 그러면 다시 위 작업 반복이면서 snake는 새로 추가다.
//        //지우는거 함수로 따로 빼고 이 과정 반복문으로 돌릴수 있게 하자
//        //m_brickController.


//        action_count--;
        
//    }

//    IEnumerator temp_cor(TileSnake tileSnake)
//    {
//        yield return new WaitForSeconds(0.3f);
//        m_brickController.MovingBricksObjIntoEmptySpace(tileSnake);
//        //Debug.Log()
//    }



//    private void Swap_Two_Brick_Only_ObjData(Vector2Int src, Vector2Int dst)
//    {
//        var temp = m_Tiles[src.x][src.y].BrickScript;
//        m_Tiles[src.x][src.y].BrickScript = m_Tiles[dst.x][dst.y].BrickScript;
//        m_Tiles[dst.x][dst.y].BrickScript = temp;
//    }


//    public void Find_Shape()
//    {
//        m_brickController.FindShape(m_Tiles);

//        //모양 없으면 브릭 재배치 해야함

//    }

    

//    private GameObject begin_selected_obj;
//    private GameObject second_selected_obj;
//    //private bool swap_state = false;
//    // Update is called once per frame
//    void Update()
//    {
//        //Touch event


//        if(Input.touchCount > 0 && action_count == 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            Vector2 touchPos = touch.position;
//            Vector2 pos = Camera.main.ScreenToWorldPoint(touchPos);



//            switch(touch.phase)
//            {
//                case TouchPhase.Began:
                    
//                    begin_selected_obj = GetTouchedBrick(pos);

//                    if(begin_selected_obj != null)
//                    {
//                        m_brickController.StopTwinkle();
//                    }
                    
//                    break;

//                case TouchPhase.Moved:
//                    //선택된 브릭이 있어야하고 이미 브릭이 이동상태가 아니어야한다.
//                    //선택된 브릭이 있는 상태로 이동해서 브릭이 바뀌면 이동이라 판단.
//                    if (begin_selected_obj != null)
//                    {

//                        //대각선이 선택될 수도 있기때문에 보정이 필요하다.
//                        //손가락을 빠르게 이동하면 한칸 건너서 선택되기도 한다.
//                        //
//                        second_selected_obj = GetTouchedBrick(pos);

//                        if (second_selected_obj != null && second_selected_obj != begin_selected_obj)
//                        {
//                            //스왑 상태로 상태변경 할 것.
//                            //브릭 스왑 진행하고 스왑이 완료되거나(아무것도 못지운경우)
//                            //지우거나 아이템 효과가 끝나는 등 모든 프로세스 진행이 완료되면 다시 상태 변경
//                            Swap_Two_Brick_Check(begin_selected_obj, second_selected_obj);
//                        }
//                    }
//                    break;
                     

//                case TouchPhase.Ended:
//                    if(begin_selected_obj != null)
//                        begin_selected_obj = null;
//                    break;
//            }

            
//        }

        
        
//    }
//}
