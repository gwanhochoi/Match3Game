using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveDir{

    Left = 0,
    Right,
    Up,
    Down
}


public class MatchCheker
{

    private int max_widthCount;
    private int max_heightCount;

    //private BrickType[] m_Bricks_copy;

    private ShapeData m_ShapeData;
    private MatchData m_MatchData;

    //private List<MatchResult> m_MatchResultList;
    //public List<MatchResult> MatchResultList
    //{
    //    get { return m_MatchResultList; }
    //}

    private Queue<MatchResult> m_MatchResultQueue;
    public Queue<MatchResult> MatchResultQueue
    {
        get { return m_MatchResultQueue; }
    }

    public MatchCheker()
    {
        m_ShapeData = new ShapeData();
        m_MatchData = new MatchData();
        //m_MatchResult = new MatchResult();
        //m_MatchResultList = new List<MatchResult>();
        
    }

    public void Init()
    {
        //m_Bricks_copy = (BrickType[])bricks.Clone();
        max_widthCount = GameDataMGR.Instance.MaxWidthCount;
        max_heightCount = GameDataMGR.Instance.MaxHeightCount;
        m_MatchResultQueue = new Queue<MatchResult>();


    }

    

    public void MatchCheck_SwapTwoPoint(Tile[][] tiles, MoveDir dir, Vector2Int dst, Vector2Int src)
    {

        //List<MatchResult> matchResultList = new List<MatchResult>();
        m_MatchResultQueue.Clear();
        MatchResult result = MatchCheck_Point(tiles, dir, dst);
        if(result != null)
            m_MatchResultQueue.Enqueue(result);
        //result = null;
        if (dir == MoveDir.Left || dir == MoveDir.Right)
            dir = MoveDir.Left == dir ? MoveDir.Right : MoveDir.Left;
        else
            dir = MoveDir.Up == dir ? MoveDir.Down : MoveDir.Up;

        result = MatchCheck_Point(tiles, dir, src);
        if (result != null)
            m_MatchResultQueue.Enqueue(result);

        //if (m_MatchResultQueue.Count == 0)
        //    return null;
        //return m_MatchResultQueue;
    }

    private MatchResult MatchCheck_Point(Tile[][] tiles, MoveDir dir, Vector2Int coordinate)
    {
        //이동된 상태의 bricks를 받아와서 좌표기준 매치검사
        //상하좌우중 어느방향으로 움직여서 들어왔는가에 따라 검사
        //지워지는 좌표를 중복없이 넣는다
        //지워지고 아이템 생성도 판단해야한다. 가장 높은 아이템 하나가 생성되고 생성되는 위치는
        //주어진 coordinate 위치에 생성

        //Debug.Log("dir = " + dir);
        List<Match> matchChecker = m_MatchData.dirCheckDataList[(int)dir];
        //MatchResult matchResult = new MatchResult();
        MatchResult matchResult = new MatchResult();
        foreach (var matchShape in matchChecker)
        {
            List<Tile> clearTile_List = new List<Tile>();
            BrickType std_type = tiles[coordinate.x][coordinate.y].bricktype;
            int type_count = 0;
            int match_count = matchShape.d_list.Count;
            clearTile_List.Add(tiles[coordinate.x][coordinate.y]);
            foreach (var d in matchShape.d_list)
            {
                Vector2Int d_coordinate = coordinate + d;
                if (!CheckCoordinate_InField(d_coordinate))
                    break;
                if (std_type != tiles[d_coordinate.x][d_coordinate.y].bricktype)
                {
                    //Debug.Log("std type = " + std_type);
                    //Debug.Log("compare type = " + tiles[d_coordinate.x][d_coordinate.y].bricktype);
                    break;
                }
                    
                clearTile_List.Add(tiles[d_coordinate.x][d_coordinate.y]);
                type_count++;
            }
            //Debug.Log("match_count = " + match_count);
            //Debug.Log("type_count = " + type_count);

            if (match_count == type_count)
            {
                //지워지는 조건 만족했으므로 좌표를 따로 보관
                //어떤 아이템인지도 matchShape에서 가져오자
                //좌표랑 아이템 담는 클래스 하나 생성
                //Debug.Log("match");
                matchResult.Add_ClearTile(clearTile_List);
                matchResult.bomb = matchShape.bomb;
            }


        }

        if (matchResult.Count() > 0)
        {
            //Debug.Log("match result count = "+matchResult.Count());
            return matchResult;
        }
            

        return null;

        //if (m_MatchResult.Count() == 0)
        //    return null;
        //return m_MatchResult;

    }

    public void MatchCheck_Point(Tile[][] tiles, Vector2Int coordinate)
    {

        //중력을 받고 있으면 안됨
        List<Match> matchChecker = m_MatchData.allCheckDataList;
        //MatchResult matchResult = new MatchResult();
        MatchResult matchResult = new MatchResult();
        foreach (var matchShape in matchChecker)
        {
            List<Tile> clearTile_List = new List<Tile>();
            BrickType std_type = tiles[coordinate.x][coordinate.y].bricktype;
            int type_count = 0;
            int match_count = matchShape.d_list.Count;
            clearTile_List.Add(tiles[coordinate.x][coordinate.y]);
            foreach (var d in matchShape.d_list)
            {
                Vector2Int d_coordinate = coordinate + d;
                if (!CheckCoordinate_InField(d_coordinate))
                    break;
                if (std_type != tiles[d_coordinate.x][d_coordinate.y].bricktype)
                {
                    //Debug.Log("std type = " + std_type);
                    //Debug.Log("compare type = " + tiles[d_coordinate.x][d_coordinate.y].bricktype);
                    break;
                }

                //아직 이동이 완료된 타일이 아님
                if(tiles[d_coordinate.x][d_coordinate.y].GraviyTile != null)
                {
                    break;
                }

                clearTile_List.Add(tiles[d_coordinate.x][d_coordinate.y]);
                type_count++;
            }
            //Debug.Log("match_count = " + match_count);
            //Debug.Log("type_count = " + type_count);

            if (match_count == type_count)
            {
                //지워지는 조건 만족했으므로 좌표를 따로 보관
                //어떤 아이템인지도 matchShape에서 가져오자
                //좌표랑 아이템 담는 클래스 하나 생성
                //Debug.Log("match");
                matchResult.Add_ClearTile(clearTile_List);
                matchResult.bomb = matchShape.bomb;
            }


        }

        if (matchResult.Count() > 0)
        {
            //Debug.Log("match result count = "+matchResult.Count());
            m_MatchResultQueue.Enqueue(matchResult);
        }
    }

    public List<Vector2Int> Find_Shape(Tile[][] tiles)
    {
        //현재 브릭들중 지울 수 있는 브릭모양을 찾는다

        //Debug.Log("bricks length = " + bricks.Length);
        //Debug.Log("bricks[0] length = " + bricks[0].Length);

        //shape 안에 좌표를 넣어야 할듯.
        foreach (var shape in m_ShapeData.shapeData)
        {
            //shape모양 좌표가져와서 해당 브릭들이 모두 같은 타입인지 검사


            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    if (tiles[i][j].tileType != Tile_Type.Ground)
                        continue;
                    Vector2Int std_pos = new Vector2Int(i + shape.d_list[0].x, j + shape.d_list[0].y);
                    if (!CheckCoordinate_InField(std_pos))
                        continue;
                    int shape_count = shape.d_list.Count;
                    int type_count = 1;
                    BrickType type = tiles[std_pos.x][std_pos.y].bricktype;
                    List<Vector2Int> coordinate_list = new List<Vector2Int>();
                    coordinate_list.Add(std_pos);
                    for (int k = 1; k < shape_count; k++)
                    {
                        Vector2Int pos = new Vector2Int(i + shape.d_list[k].x, j + shape.d_list[k].y);
                        if (!CheckCoordinate_InField(pos))
                            break;
                        if (type == tiles[pos.x][pos.y].bricktype)
                        {
                            coordinate_list.Add(pos);
                            type_count++;
                            continue;
                        }
                        break;

                    }

                    if (type_count == shape_count)
                    {
                        //해당 모양 좌표의 브릭 타입이 모두 같다
                        //타입과 좌표 따로 갖고 있자

                        //Debug.Log("Bricktype = " + type);

                        //foreach (var child in coordinate_list)
                        //{
                        //    Debug.Log("[" + child.x + "," + child.y + "]");
                        //}


                        return coordinate_list; //일단 하나만 찾자
                    }


                }
            }

        }


        return null;

    }

    private bool CheckCoordinate_InField(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= max_widthCount || pos.y < 0 || pos.y >= max_heightCount)
            return false;
        return true;
    }

    public void Match_Check(Vector2Int coordinate)
    {
        //자리를 바꿨다고 가정하고 체크
    }


    
}
