using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrickController
{

    //brick 이동 컨트롤이 필요함
    //이동이 완료되는 시점이나 애니메이션 등의 컨트롤을 해야함
    //brick을 충전해놓고 하나씩 빼서 사용한다. 재사용

    private MatchCheker m_matchChecker;
    private int m_action_Count;
    List<Vector2Int> shape_pos_list;
    public BrickController()
    {
        m_matchChecker = new MatchCheker();
        m_action_Count = 0;
        //m_TileSnakeController = new TileSnakeController();
    }


    //private delegate void SwapCompleteDel();
    //private SwapCompleteDel SwapCompleteCallback;

    private Action SwapCompleteCallback;
    public Action BricksMoveCompleteCallback;
    
    private List<Brick> twinkle_BrickList = new List<Brick>();
    //private TileSnakeController m_TileSnakeController;


    public void SwapTwoBrick(Tile[][] tiles, Vector2Int src_coordinate, Vector2Int dst_coordinate)
    {
        //두 브릭을 스왑 한다
        //좌우측 우선임
        //Brick src_brick = tiles[src.x][src.y].BrickScript;
        //Brick dst_brick = tiles[dst.x][dst.y].BrickScript;
        //Vector2Int src_coordinate = src_brick.coordinate;
        //Vector2Int dst_coordinate = dst_brick.coordinate;
        //int dx = dst_coordinate.x - src_coordinate.x;
        //int dy = dst_coordinate.y - src_coordinate.y;

        //MoveDir dir;
        //if (dx != 0)
        //{
        //    dx = dx > 0 ? 1 : -1;
        //    dir = dx == 1 ? MoveDir.Right : MoveDir.Left;
        //    dy = 0;
        //}
        //else
        //{
        //    dy = dy > 0 ? 1 : -1;
        //    dir = dy == 1 ? MoveDir.Up : MoveDir.Down;
        //    dx = 0;
        //}


        //dst_brick = tiles[src_coordinate.x + dx][src_coordinate.y + dy].BrickScript;
        //dst_coordinate = dst_brick.coordinate;

        var temp = tiles[src_coordinate.x][src_coordinate.y].BrickScript;
        tiles[src_coordinate.x][src_coordinate.y].BrickScript = tiles[dst_coordinate.x][dst_coordinate.y].BrickScript;
        tiles[dst_coordinate.x][dst_coordinate.y].BrickScript = temp;
        //스왑 정보 메세지 생성한다

    }

    public TileSnake MovingBricksDataIntoEmptySpace(TileSnake tileSnake)
    {
        //지워진 타일 공간으로 브릭들을 이동시킨다
        //머리부터 한칸씩 이동한다(아래, 왼쪽아래 오른쪽 아래 우선순위를 갖는다)
        //한칸이동하고 머리부터 꼬리까지 이동완료가 아닐때까지 체크하고 검사리스트에 넣는다
        //검사리스트로부터 매치검사

        TileSnake newTileSnake = new TileSnake();
        //data상으로 이동
        foreach(var tile in tileSnake.TileList)
        {
            foreach(var child in tile.LinkedDownTileList)
            {
                if(!child.Tile_MoveState() || child.bricktype != BrickType.Empty )
                {
                    continue;
                }
                else
                {
                    //이동가능
                    //tile에 있는 brick을 child로 이동
                    //tile bricktype 빈자리로 변경
                    child.BrickScript = tile.BrickScript;
                    //tile.bricktype = BrickType.Empty;
                    //한칸씩 이동이므로 한번 이동하면 break
                    //새로운 tileSanke에 이동된 타일을 넣는다
                    newTileSnake.Add_Tile(child);
                    break;
                }
            }
        }
        //data상으로 타일안의 Brick들은 이동한 상태다
        //새로운 TileSnake를 바탕으로 브릭오브젝트 무빙후에 매치검사한다
        //무빙은 하나의 코루틴에서 한번에 하자?
        return newTileSnake;
    }


    public void MovingBricksObjIntoEmptySpace(TileSnake tileSnake)
    {
        m_action_Count = tileSnake.TileList.Count;
        foreach(var child in tileSnake.TileList)
        {
            //tile BrickMove 함수 만들어서 호출
            //충전 타일 따로 만들어야함

            if(child.tileType == Tile_Type.Ground)
            {
                Tile_Ground tile_ground = child as Tile_Ground;
                tile_ground.BrickMoveInTile(BrickMoveCallback);
            }
            else if(child.tileType == Tile_Type.Charge)
            {
                Tile_Charge tile_Charge = child as Tile_Charge;
                tile_Charge.BrickMoveInTile(BrickMoveCallback);
            }
            //child.BrickMoveInTile(BrickMoveCallback);
            //if (child.BrickScript == null)
            //    continue;
            
            //child.BrickScript.Move(child.GraviyTile.transform.localPosition, BrickMoveCallback);
            //child.GraviyTile.BrickScript = child.BrickScript;
            //child.BrickScript = null;
        }
    }

    private void BrickMoveCallback()
    {
        m_action_Count--;
        if (m_action_Count != 0)
            return;
        BricksMoveCompleteCallback();
    }


    //public void SearchTileSnake()
    //{
    //    List<MatchResult> list = m_matchChecker.MatchResultList;
    //    //지워진 각 타일의 위치에서 위 오른쪽위 왼쪽위 우선순위로 탐색한다
    //    //각 타일마다 위 오위 왼위 등록한 리스트가 있으므로 가져온다

    //    foreach (var child in list)
    //    {
    //        foreach(var tile in child.ClearTileList)
    //        {
    //            SearchTail(tile);
    //        }
    //    }

    //    m_matchChecker.MatchResultList.Clear();
    //    //foreach(var clearTile in clearTileList)
    //    //{
    //    //    //연결이 안될때까지 찾는다(더이상 탐색할 곳이 없거나 충전타일을 만날때까지)
    //    //    SearchTail(clearTile);
    //    //}

    //}

    //마지막 꼬리까지 검사하고 TileSnake완성시키기
    //재귀 아니면 반복문?
    //private void SearchTail(Tile std_tile)
    //{
    //    var tile = std_tile;
    //    TileSnake tileSnake = new TileSnake();
    //    while (tile.LinkedTileList.Count > 0)
    //    {
    //        int false_Count = 0;
    //        foreach(var child in tile.LinkedTileList)
    //        {
    //            //현재 이동이 불가능한 타일이거나 불가능한 지역이면 다음위치
    //            if (!child.Tile_MoveState())
    //            {
    //                false_Count++;
    //                continue;
    //            }

    //            if (child.bricktype != BrickType.Empty)
    //                tileSnake.Add_Tile(child);
    //            //3군데 검사해서 하나만 만족하면 된다.
    //            //만약에 비어있는 타일이라면 Add는 하지 않고 다음 검사하면 된다
    //            //3군데 모두 Add할 수 없으면 끝이다
    //            //구현은 아직 안되있지만 현재타일이 충전타일 아래 위치면 그때도 끝

    //            tile = child;
    //            break;

    //        }
    //        //세군데 모두 아이스같은 상태때문에 이동이 불가 할수도 있다
    //        if (false_Count == tile.LinkedTileList.Count)
    //            break;
    //    }
    //    m_TileSnakeController.Add_TileSnake(tileSnake);
    //}


    public void TwoBrickReturnMove(Brick src, Brick dst, Vector2 src_pos, Vector2 dst_pos, Action Callback)
    {

        SwapCompleteCallback = Callback;
        m_action_Count += 2;
        src.ReturnMove(dst_pos, SwapMoveCallback);
        dst.ReturnMove(src_pos, SwapMoveCallback);


    }

    public void TwoBrickSwapMove(Brick src, Brick dst, Vector2 src_pos, Vector2 dst_pos, Action Callback)
    {
        SwapCompleteCallback = Callback;
        m_action_Count += 2;
        src.Move(dst_pos, SwapMoveCallback);
        dst.Move(src_pos, SwapMoveCallback);
    }

    private void SwapMoveCallback()
    {
        m_action_Count--;
        if (m_action_Count != 0)
            return;

        SwapCompleteCallback();
    }

    public void EraseBricks(List<Brick> brickList)
    {
        //브릭을 화면 밖으로 보내거나 invisible처리한다
        //지워질때 파티클 애니메이션도 활성
        foreach(var child in brickList)
        {
            BrickPool.Instance().Enqueue_Brick(child);
            //child.transform.localPosition = new Vector3(-5000, -5000);
            //각 타일마다 파티클 애니메이션
        }
    }


    //public void BrickSwap(Tile[][] tiles, Brick src, Brick dst)
    //{

    //    //좌우측 우선임
    //    Vector2Int src_coordinate = src.coordinate;
    //    Vector2Int dst_coordinate = dst.coordinate;
    //    int dx = dst_coordinate.x - src_coordinate.x;
    //    int dy = dst_coordinate.y - src_coordinate.y;

    //    MoveDir dir;

    //    if (dx != 0)
    //    {
    //        dx = dx > 0 ? 1 : -1;
    //        dir = dx == 1 ? MoveDir.Right : MoveDir.Left;
    //        dy = 0;
    //    }
    //    else
    //    {
    //        dy = dy > 0 ? 1 : -1;
    //        dir = dy == 1 ? MoveDir.Up : MoveDir.Down;
    //        dx = 0;
    //    }

    //    m_action_Count += 2;

    //    dst = tiles[src_coordinate.x + dx][src_coordinate.y + dy].BrickObj.GetComponent<Brick>();
    //    dst_coordinate = dst.coordinate;

    //    //일단 스왑하자(참조하는 오브젝트만)
    //    Swap_Two_Brick_Only_ObjData(tiles, src_coordinate, dst_coordinate);

    //    //스왑후에 매치가 되는지 검사
    //    List<MatchResult> result = null;
    //    if (src.type != dst.type)
    //        result = m_matchChecker.MatchCheck_SwapTwoPoint(tiles, dir, dst_coordinate, src_coordinate);
    //    //else
    //    //    result.Clear();

    //    if (result != null)
    //    {
    //        //매칭브릭이 있으므로 단방향 이동후 브릭을 지운다
    //        //이동완료후 지우기 콜백 
    //        //브릭을 이동하거나 지워지고 채워지는 동안 터치이벤트 막아야한다.
    //        src.Move(tiles[dst_coordinate.x][dst_coordinate.y].transform.position, SwapMoveEndCallback);
    //        dst.GetComponent<Brick>().Move(tiles[src_coordinate.x][src_coordinate.y].transform.position, SwapMoveEndCallback);

    //    }
    //    else
    //    {
    //        //Debug.Log("not match");
    //        //매칭브릭이 없으므로 스왑했던 오브젝트를 제자리로 돌리고 왕복 이동한다
    //        //스왑 무빙 둘다 완전 완료 된 후에 클리어 처리를 해야한다
    //        Swap_Two_Brick_Only_ObjData(tiles, dst_coordinate, src_coordinate);
    //        src.ReturnMove(tiles[dst_coordinate.x][dst_coordinate.y].transform.position, SwapMoveEndCallback);
    //        dst.GetComponent<Brick>().ReturnMove(tiles[src_coordinate.x][src_coordinate.y].transform.position, SwapMoveEndCallback);

    //    }
        
        
    //}

    //private void SwapMoveEndCallback()
    //{
    //    //스왑무브 두개 끝나면 실행
    //    m_action_Count--;
    //    if (m_action_Count != 0)
    //        return;
    //    //swap이 완료 됐고 지워지는게 있으면 지운다(안보이게 좌표 변경후 장전한다?)
    //    List<MatchResult> result = m_matchChecker.MatchResultList;
    //    //if (result.Count() == 0)
    //    //    return;

    //    MoveComplete_Callback(result);

    //}

    //private void Swap_Two_Brick_Only_ObjData(Tile[][] tiles, Vector2Int src, Vector2Int dst)
    //{
    //    var temp = tiles[src.x][src.y].BrickObj;
    //    tiles[src.x][src.y].BrickObj = tiles[dst.x][dst.y].BrickObj;
    //    tiles[dst.x][dst.y].BrickObj = temp;
    //}


    
    public void FindShape(Tile[][] tiles)
    {
        m_matchChecker.Init();
        if (shape_pos_list != null)
            shape_pos_list.Clear();
        shape_pos_list = m_matchChecker.Find_Shape(tiles);
        if (shape_pos_list == null)
        {
            //매치가 되는 브릭이 없으므로 브릭 리셋
            //Debug.Log("shape null");
            return;
        }

        //찾은 브릭들 twinkle
        //일단은 찾고 바로 반짝이는데 나중에는 찾고 일정 시간 지난뒤 반짝이도록 하자
        twinkle_BrickList.Clear();
        foreach (var child in shape_pos_list)
        {
            Brick brick = tiles[child.x][child.y].BrickScript;
            brick.Twinkle();
            twinkle_BrickList.Add(brick);
        }
    }

    public void StopTwinkle()
    {
        if (twinkle_BrickList.Count == 0)
            return;

        foreach(var child in twinkle_BrickList)
        {
            child.StopTwinkle();
        }
        twinkle_BrickList.Clear();
    }
}
