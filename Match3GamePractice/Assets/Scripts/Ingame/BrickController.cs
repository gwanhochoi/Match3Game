using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController
{

    //brick 이동 컨트롤이 필요함
    //이동이 완료되는 시점이나 애니메이션 등의 컨트롤을 해야함
    //

    private MatchCheker m_matchChecker;
    private int m_action_Count;
    List<Vector2Int> shape_pos_list;
    public BrickController()
    {
        m_matchChecker = new MatchCheker();
        m_action_Count = 0;
    }

    public delegate void MoveComplete(MatchResult result);
    public MoveComplete MoveComplete_Callback;

    private List<Brick> twinkle_BrickList = new List<Brick>();

    public void BrickSwap(Tile[][] tiles, Brick src, Brick dst)
    {

        //좌우측 우선임
        Vector2Int src_coordinate = src.coordinate;
        Vector2Int dst_coordinate = dst.coordinate;
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

        m_action_Count += 2;

        dst = tiles[src_coordinate.x + dx][src_coordinate.y + dy].BrickObj.GetComponent<Brick>();
        dst_coordinate = dst.coordinate;

        //일단 스왑하자(참조하는 오브젝트만)
        Swap_Two_Brick_Only_ObjData(tiles, src_coordinate, dst_coordinate);

        //스왑후에 매치가 되는지 검사
        MatchResult result = null;
        if (src.type != dst.type)
            result = m_matchChecker.MatchCheck_SwapTwoPoint(tiles, dir, dst_coordinate, src_coordinate);
        else
            m_matchChecker.matchResult.Clear();

        if (result != null)
        {
            //매칭브릭이 있으므로 단방향 이동후 브릭을 지운다
            //이동완료후 지우기 콜백 
            //브릭을 이동하거나 지워지고 채워지는 동안 터치이벤트 막아야한다.
            src.Move(tiles[dst_coordinate.x][dst_coordinate.y].transform.position, SwapMoveEndCallback);
            dst.GetComponent<Brick>().Move(tiles[src_coordinate.x][src_coordinate.y].transform.position, SwapMoveEndCallback);

        }
        else
        {
            //Debug.Log("not match");
            //매칭브릭이 없으므로 스왑했던 오브젝트를 제자리로 돌리고 왕복 이동한다
            //스왑 무빙 둘다 완전 완료 된 후에 클리어 처리를 해야한다
            Swap_Two_Brick_Only_ObjData(tiles, dst_coordinate, src_coordinate);
            src.ReturnMove(tiles[dst_coordinate.x][dst_coordinate.y].transform.position, SwapMoveEndCallback);
            dst.GetComponent<Brick>().ReturnMove(tiles[src_coordinate.x][src_coordinate.y].transform.position, SwapMoveEndCallback);

        }
        
        
    }

    private void SwapMoveEndCallback()
    {
        //스왑무브 두개 끝나면 실행
        m_action_Count--;
        if (m_action_Count != 0)
            return;
        //swap이 완료 됐고 지워지는게 있으면 지운다(안보이게 좌표 변경후 장전한다?)
        MatchResult result = m_matchChecker.matchResult;
        //if (result.Count() == 0)
        //    return;

        MoveComplete_Callback(result);

    }

    private void Swap_Two_Brick_Only_ObjData(Tile[][] tiles, Vector2Int src, Vector2Int dst)
    {
        var temp = tiles[src.x][src.y].BrickObj;
        tiles[src.x][src.y].BrickObj = tiles[dst.x][dst.y].BrickObj;
        tiles[dst.x][dst.y].BrickObj = temp;



    }


    
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
            Brick brick = tiles[child.x][child.y].BrickObj.GetComponent<Brick>();
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
