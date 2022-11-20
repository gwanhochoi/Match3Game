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
}

public enum Bomb
{
    None = 0,
    HorizontalBomb,
    VerticalBomb,
    Dynamite,
    ColorBomb
}

//public enum GroundType
//{
//    Back = 0,
//    Ground
//}

public enum ObjectType
{
    None = 0,
    Brick,
    Bomb
}

public class GameFieldController : MonoBehaviour
{

    private TileMapCreator m_TileMapCreator;
    private BrickFillChecker m_brickFillChecker;
    private BrickCreator m_brickCreator;

    private BrickController m_brickController;
    private MatchCheker m_matchChecker;
    private TileSnakeController m_tileSnakeController;
    private TileController m_tileController;
    private GameLogicDataInfo m_gameLogicDataInfo;


    //private int action_count = 0;
    private GameObject begin_selected_obj;
    private GameObject second_selected_obj;

    private Tile[][] tiles;

    private bool TouchLock;

    void Awake()
    {
        Application.targetFrameRate = 60;
        m_TileMapCreator = GetComponent<TileMapCreator>();
        m_brickFillChecker = new BrickFillChecker();
        m_brickCreator = GetComponent<BrickCreator>();

        m_tileController = new TileController();
        m_brickController = new BrickController();
        m_matchChecker = new MatchCheker();
        m_tileSnakeController = new TileSnakeController();
        m_gameLogicDataInfo = new GameLogicDataInfo();

        TouchLock = false;

        m_brickController.BricksMoveCompleteCallback = BrickMoveEndCallback;

    }

    public void CreateGameField()
    {
        tiles = m_TileMapCreator.CreateTileMap(GameDataMGR.Instance.mapData);

        TileTypeInfo[][] tileTypeInfos = new TileTypeInfo[tiles.Length][];
        for(int i = 0; i < tiles.Length; i++)
        {
            tileTypeInfos[i] = new TileTypeInfo[tiles[0].Length];
            for(int j = 0; j < tiles[0].Length; j++)
            {
                tileTypeInfos[i][j] = new TileTypeInfo(tiles[i][j].tileType, ObjectType.None, BrickType.Empty,
                    tiles[i][j].transform.localPosition);
            }
        }

        m_brickFillChecker.Bricks_Reset_In_TileInfos(tileTypeInfos);
        Brick[][] bricks = m_brickCreator.CreateBricksInTile(tileTypeInfos);

        for(int i = 0; i < tiles.Length; i++)
        {
            for(int j = 0; j < tiles[0].Length; j++)
            {
                tiles[i][j].BrickScript = bricks[i][j];
            }
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

    // Start is called before the first frame update
    void Start()
    {
        LoadMapData();
        m_matchChecker.Init();
    }

    private void EraseBricks()
    {
        //브릭을 지운다
        //결과에 따라서 bomb도 생성해야하지만 아직 미구현
        //List<Vector2Int> clearPos_list = new List<Vector2Int>();
        List<Brick> brickList = new List<Brick>();
        List<Tile> tileList = new List<Tile>();
        List<int> clearPosXList = new List<int>();
        while (m_matchChecker.MatchResultQueue.Count > 0)
        {
            
            foreach (var tile in m_matchChecker.MatchResultQueue.Dequeue().ClearTileList)
            {
                if (!brickList.Contains(tile.BrickScript))
                {
                    brickList.Add(tile.BrickScript);
                }

                //if (!clearPos_list.Contains(tile.Coordinate))
                m_gameLogicDataInfo.Add_ClearCoordinate(tile.Coordinate);
                    //clearPos_list.Add(tile.Coordinate);

                if (!tileList.Contains(tile))
                    tileList.Add(tile);

                if (!clearPosXList.Contains(tile.Coordinate.x))
                    clearPosXList.Add(tile.Coordinate.x);

                m_gameLogicDataInfo.Add_TileSnakeCheckPoint(tile.Coordinate);
            }
        }

        m_brickController.EraseBricks(brickList);
        m_tileController.EraseBrick(tileList);

        //m_gameLogicDataInfo.ClearCoordinate_List = clearPos_list;

        //List<Vector2Int> empty_tile_coordinate_List = new List<Vector2Int>();
        

    }

    private void SwapMoveCallback()
    {

        //EraseBricks();
        //TileSnake tileSnake = m_tileSnakeController.SearchTileSanke(tiles, m_gameLogicDataInfo.ClearCoordinate_List);
        ////이동할 브릭을 따로 갖고 있자
        //List<Brick> moveTileList = new List<Brick>();
        //foreach(var tile in tileSnake.TileList)
        //{
        //    moveTileList.Add(tile.BrickScript);
        //}
        //m_gameLogicDataInfo.MovedBricksList = moveTileList;

        //m_brickController.MovingBricksObjIntoEmptySpace(tileSnake);

        //TouchLock = false;

        MoveBricks();
    }

    private void MoveBricks()
    {
        EraseBricks();
        //Debug.Log("clear coordinate list count = " + m_gameLogicDataInfo.ClearCoordinate_List.Count);
        TileSnake tileSnake = m_tileSnakeController.SearchTileSanke(tiles, m_gameLogicDataInfo.ClearCoordinate_List);

        //더이상 이동 시킬 브릭이 없다
        if (tileSnake.TileList.Count == 0)
        {
            TouchLock = false;
            m_gameLogicDataInfo.Clear();
            //Debug.Log("move end");
            return;
        }

        //이동할 브릭을 따로 갖고 있자
        List<Brick> moveTileList = new List<Brick>();

        //Debug.Log("tilesnake list count = " + tileSnake.TileList.Count);
        foreach (var tile in tileSnake.TileList)
        {
            moveTileList.Add(tile.BrickScript);
        }
        //Debug.Log("moveTileList count = " + moveTileList.Count);
        m_gameLogicDataInfo.MovedBricksList = moveTileList;

        m_brickController.MovingBricksObjIntoEmptySpace(tileSnake);
    }

    private void BrickMoveEndCallback()
    {
        //이동이 한번 완료 된 상태
        //앞서 브릭이 지워졌던 좌표로부터 다시 tilesnake를 갱신
        //갱신된 tilesnake와 전에 이동한 브릭들과 비교해서 snake에 없는 브릭들은 최종적으로 이동이
        //완료된 것이므로 그 위치에서 매치검사를 한다(이때 이동이 완료된 브릭들만 즉 중력이 없는 애들만 매치가 된다)이동중이 아니어야한다
        //지워지면 다시 snake갱신하고 이동시작을 반복

        //다시 tileSnake를 찾는다
        //앞서 지웠던 타일 좌표 정보가 필요함

        //새로 빈공간을 찾는다
        //m_gameLogicDataInfo.ClearCoordinate_List.Clear();
        foreach (var child in m_gameLogicDataInfo.TileSnakeCheckPoint_Dic)
        {
            for (int y = child.Value; y < m_TileMapCreator.max_heightCount; y++)
            {
                if (tiles[child.Key][y].tileType == Tile_Type.Ground && tiles[child.Key][y].bricktype == BrickType.Empty)
                {
                    m_gameLogicDataInfo.Add_ClearCoordinate(new Vector2Int(child.Key, y));
                    Debug.Log("pos x = " + child.Key + " " + "pos y = " + y);
                }
            }
        }


        TileSnake tileSnake = m_tileSnakeController.SearchTileSanke(tiles, m_gameLogicDataInfo.ClearCoordinate_List);

        //새로 갱신한 tilesnake와 앞에 이동한 브릭들을 비교해서 이동이 완료된 브릭을 찾아서 매치 검사하자
        //tilesnake에 포함되지 않는 브릭들을 빼내야 한다

        List<Vector2Int> matchCheckPointList = new List<Vector2Int>();
        foreach(var brick in m_gameLogicDataInfo.MovedBricksList)
        {
            if(!tileSnake.ContainsBrick(brick))
            {
                //포함되지 않음
                matchCheckPointList.Add(new Vector2Int(brick.coordinate.x, brick.coordinate.y));
                tiles[brick.coordinate.x][brick.coordinate.y].GraviyTile = null;
                
            }
        }

        //Debug.Log("matchCheckPointList Count = " + matchCheckPointList.Count);
        m_gameLogicDataInfo.MovedBricksList.Clear();

        m_matchChecker.MatchResultQueue.Clear();
        foreach (var child in matchCheckPointList)
        {
            m_matchChecker.MatchCheck_Point(tiles, child);
        }

        //m_gameLogicDataInfo.ClearCoordinate_List.Clear();

        MoveBricks();

    }

    private void SwapReturnMoveCallback()
    {
        TouchLock = false;
    }

    private void Swap_Two_Brick_Check(GameObject first_obj, GameObject second_obj)
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


        if (first_obj.GetComponent<Brick>().coordinate == second_obj.GetComponent<Brick>().coordinate)
            return ;

        //먼저 임의로 타일의 브릭만 스왑을 한다
        //매치 체크를 한다
        //결과에 따라 브릭 이동을 한다

        TouchLock = true;

        //좌우측 우선임
        Brick src_brick = first_obj.GetComponent<Brick>();
        Brick dst_brick = second_obj.GetComponent<Brick>();
        Vector2Int src_coordinate = src_brick.coordinate;
        Vector2Int dst_coordinate = dst_brick.coordinate;
        int dx = dst_coordinate.x - src_coordinate.x;
        int dy = dst_coordinate.y - src_coordinate.y;

        MoveDir dir;
        if (dx != 0)
        {
            dx = dx > 0 ? 1 : -1;
            dir = dx == 1 ? MoveDir.Right : MoveDir.Left;
            dy = 0;
        }
        else
        {
            dy = dy > 0 ? 1 : -1;
            dir = dy == 1 ? MoveDir.Up : MoveDir.Down;
            dx = 0;
        }


        dst_brick = tiles[src_coordinate.x + dx][src_coordinate.y + dy].BrickScript;
        dst_coordinate = dst_brick.coordinate;

        //스왑을 먼저 하고 매치가 되는지 체크한다
        //매치가 안되면 다시 복구 한다
        m_brickController.SwapTwoBrick(tiles, src_coordinate, dst_coordinate);

        m_matchChecker.MatchCheck_SwapTwoPoint(tiles, dir, dst_coordinate, src_coordinate);

        if(m_matchChecker.MatchResultQueue.Count > 0)
        {
            //먼저 브릭 스왑 이동을 한다
            //이동이 끝나면 매치결과에 따른 브릭을 지운다
            m_brickController.TwoBrickSwapMove(src_brick, dst_brick,
                tiles[src_coordinate.x][src_coordinate.y].transform.localPosition,
                tiles[dst_coordinate.x][dst_coordinate.y].transform.localPosition, SwapMoveCallback);
        }
        else
        {
            m_brickController.SwapTwoBrick(tiles, dst_coordinate, src_coordinate);
            m_brickController.TwoBrickReturnMove(src_brick, dst_brick,
                tiles[src_coordinate.x][src_coordinate.y].transform.localPosition,
                tiles[dst_coordinate.x][dst_coordinate.y].transform.localPosition, SwapReturnMoveCallback);
        }


        //같은색을 스왑하는건 의미없음


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

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && !TouchLock)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            Vector2 pos = Camera.main.ScreenToWorldPoint(touchPos);



            switch (touch.phase)
            {
                case TouchPhase.Began:

                    begin_selected_obj = GetTouchedBrick(pos);

                    //if (begin_selected_obj != null)
                    //{
                    //    m_brickController.StopTwinkle();
                    //}

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
                    if (begin_selected_obj != null)
                        begin_selected_obj = null;
                    break;
            }


        }
    }
}
