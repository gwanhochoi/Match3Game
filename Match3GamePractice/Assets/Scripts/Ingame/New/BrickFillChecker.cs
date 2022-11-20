using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickFillChecker
{
    //어느 위치에 어떤 브릭 또는 아이템을 채울지 정한다
    //맵데이터를 기반으로 채우고 나머지는 랜덤하게 채운다
    //예를들면 사용할 브릭의 종류나 특정 위치에 특정 브릭과 아이템이 있을수 있다
    //게임 진행중에 더이상 매치가 불가능할때 브릭을 재배치 할 때도 사용한다

    private delegate bool CheckMatchForCreate(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type);
    private CheckMatchForCreate[] CheckMatchForCreates_Del;

    public BrickFillChecker()
    {
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
    }


    public void Bricks_Reset_In_TileInfos(TileTypeInfo[][] tileInfos)
    {
        List<Vector2Int> brick_pos_list = new List<Vector2Int>();


        //list에 넣고 랜덤하게 뽑아오기

        for (int i = 0; i < tileInfos.Length; i++)
        {
            for (int j = 0; j < tileInfos[0].Length; j++)
            {
                if(IsSetBrickInTile(tileInfos[i][j]))
                    brick_pos_list.Add(new Vector2Int(i, j));
            }
        }

        //pick_pos에서 3개짜리 모양 만들수 있는지 체크
        while (brick_pos_list.Count > 0)
        {
            int range = brick_pos_list.Count;
            //Debug.Log("brick_pos_list count = " + range);
            Vector2Int pick_pos = brick_pos_list[Random.Range(0, range)];
            if (Fill_3Brick(tileInfos, pick_pos))
                break;
            brick_pos_list.Remove(pick_pos);
            

        }

        for (int i = 0; i < tileInfos.Length; i++)
        {
            for (int j = 0; j < tileInfos[0].Length; j++)
            {
                Find_PositionableBrick(tileInfos, new Vector2Int(i, j));
            }
        }
    }

    private bool Find_PositionableBrick(TileTypeInfo[][] tileInfos, Vector2Int pos)
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

        if (tileInfos[pos.x][pos.y].tile_Type != Tile_Type.Ground)
            return false;

        if (tileInfos[pos.x][pos.y].objectType != ObjectType.None)
            return false;


        Queue<BrickType> brickType_quque = Get_RandBrick();
        BrickType type = BrickType.Empty;

        int check_count = 0;

        while (brickType_quque.Count > 0)
        {
            type = brickType_quque.Dequeue();
            foreach (var child in CheckMatchForCreates_Del)
            {
                if (!child(tileInfos, pos, type))
                    break;
                check_count++;
            }

            if (check_count == 10)
            {
                //10개 검사 모두 만족한경우
                tileInfos[pos.x][pos.y].brickType = type;
                return true;
            }
            check_count = 0;
        }

        return false;
    }

    private bool Check_3Brick_Row_Center(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //양옆에 2개
        int check_pos_x = pos.x;


        if (check_pos_x - 1 < 0 || check_pos_x + 1 >= tileInfos.Length)
            return true;

        if (tileInfos[check_pos_x - 1][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 1][pos.y].tile_Type != Tile_Type.Ground)
            return true;


        if (tileInfos[check_pos_x - 1][pos.y].brickType == tileInfos[check_pos_x + 1][pos.y].brickType)
        {
            if (tileInfos[check_pos_x - 1][pos.y].brickType == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Column_Center(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //위아래 2개
        int check_pos_y = pos.y;


        if (check_pos_y - 1 < 0 || check_pos_y + 1 >= tileInfos[0].Length)
            return true;

        if (tileInfos[pos.x][check_pos_y - 1].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y + 1].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[pos.x][check_pos_y - 1].brickType == tileInfos[pos.x][check_pos_y + 1].brickType)
        {
            if (tileInfos[pos.x][check_pos_y - 1].brickType == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Left(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //left 2개
        int check_pos_x = pos.x;


        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0)
        {
            return true;
        }

        if (tileInfos[check_pos_x - 1][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x - 2][pos.y].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[check_pos_x - 1][pos.y].brickType == tileInfos[check_pos_x - 2][pos.y].brickType)
        {
            if (tileInfos[check_pos_x - 1][pos.y].brickType == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Right(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //right 2개
        int check_pos_x = pos.x;

        if (check_pos_x + 1 >= tileInfos.Length || check_pos_x + 2 >= tileInfos.Length)
            return true;

        if (tileInfos[check_pos_x + 1][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 2][pos.y].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[check_pos_x + 1][pos.y].brickType == tileInfos[check_pos_x + 2][pos.y].brickType)
        {
            if (tileInfos[check_pos_x + 1][pos.y].brickType == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Up(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //up 2개
        int check_pos_y = pos.y;

        if (check_pos_y + 1 >= tileInfos[0].Length || check_pos_y + 2 >= tileInfos[0].Length)
            return true;

        if (tileInfos[pos.x][check_pos_y + 1].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y + 2].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[pos.x][check_pos_y + 1].brickType == tileInfos[pos.x][check_pos_y + 2].brickType)
        {
            if (tileInfos[pos.x][check_pos_y + 1].brickType == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Down(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //down 2개
        int check_pos_y = pos.y;


        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0)
            return true;

        if (tileInfos[pos.x][check_pos_y - 1].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y - 2].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[pos.x][check_pos_y - 1].brickType == tileInfos[pos.x][check_pos_y - 2].brickType)
        {
            if (tileInfos[pos.x][check_pos_y - 1].brickType == type)
                return false;
        }

        return true;
    }

    private bool Check_4Brick_LU(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //left up 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;


        if (check_pos_x - 1 < 0 || check_pos_y + 1 >= tileInfos[0].Length)
            return true;

        if (tileInfos[check_pos_x - 1][check_pos_y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x - 1][check_pos_y + 1].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x][check_pos_y + 1].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[check_pos_x - 1][check_pos_y].brickType == tileInfos[check_pos_x - 1][check_pos_y + 1].brickType ?
            tileInfos[check_pos_x - 1][check_pos_y + 1].brickType == tileInfos[check_pos_x][check_pos_y + 1].brickType : false)
        {
            if (tileInfos[check_pos_x - 1][check_pos_y].brickType == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_LD(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //left down 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;


        if (check_pos_x - 1 < 0 || check_pos_y - 1 < 0)
            return true;

        if (tileInfos[check_pos_x - 1][check_pos_y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x - 1][check_pos_y - 1].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x][check_pos_y - 1].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[check_pos_x - 1][check_pos_y].brickType == tileInfos[check_pos_x - 1][check_pos_y - 1].brickType ?
           tileInfos[check_pos_x - 1][check_pos_y - 1].brickType == tileInfos[check_pos_x][check_pos_y - 1].brickType : false)
        {
            if (tileInfos[check_pos_x - 1][check_pos_y].brickType == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_RU(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //right up 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;


        if (check_pos_x + 1 >= tileInfos.Length || check_pos_y + 1 >= tileInfos[0].Length)
            return true;

        if (tileInfos[check_pos_x + 1][check_pos_y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 1][check_pos_y + 1].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x][check_pos_y + 1].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[check_pos_x + 1][check_pos_y].brickType == tileInfos[check_pos_x + 1][check_pos_y + 1].brickType ?
           tileInfos[check_pos_x + 1][check_pos_y + 1].brickType == tileInfos[check_pos_x][check_pos_y + 1].brickType : false)
        {
            if (tileInfos[check_pos_x + 1][check_pos_y].brickType == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_RD(TileTypeInfo[][] tileInfos, Vector2Int pos, BrickType type)
    {
        //right down 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;

        if (check_pos_x + 1 >= tileInfos.Length || check_pos_y - 1 < 0)
            return true;

        if (tileInfos[check_pos_x + 1][check_pos_y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 1][check_pos_y - 1].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x][check_pos_y - 1].tile_Type != Tile_Type.Ground)
            return true;

        if (tileInfos[check_pos_x + 1][check_pos_y].brickType == tileInfos[check_pos_x + 1][check_pos_y - 1].brickType ?
           tileInfos[check_pos_x + 1][check_pos_y - 1].brickType == tileInfos[check_pos_x][check_pos_y - 1].brickType : false)
        {
            if (tileInfos[check_pos_x + 1][check_pos_y].brickType == type)
            {
                return false;
            }
        }

        return true;

    }

    private Queue<BrickType> Get_RandBrick()
    {
        List<BrickType> list = new List<BrickType>();

        for (BrickType type = BrickType.Blue; type < BrickType.BrickEnd; type++)
        {
            list.Add(type);
        }

        Queue<BrickType> queue = new Queue<BrickType>();

        while (list.Count > 0)
        {
            int randNum = Random.Range(0, list.Count);
            BrickType type = (BrickType)list[randNum];
            queue.Enqueue(type);
            list.Remove(type);
        }

        return queue;
    }

    private bool IsSetBrickInTile(TileTypeInfo info)
    {
        if(info.tile_Type == Tile_Type.Ground && info.objectType != ObjectType.Bomb)
        {
            return true;
        }
        return false;
    }


    private bool Fill_3Brick(TileTypeInfo[][] tileInfos ,Vector2Int pos)
    {
        //일단 모양 3칸짜리 모양 만들자. 현재 위치 가로 또는 세로
        //oxoo 또는 ooxo
        //o o
        //x o
        //o x
        //o o
        if (tileInfos[pos.x][pos.y].tile_Type != Tile_Type.Ground)
            return false;

        if (tileInfos[pos.x][pos.y].objectType == ObjectType.Bomb)
            return false;

        //BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        //oxoo => left 1 right 1 right 2

        if (Check_3Brick_Row1(tileInfos, pos))
            return true;
        else if (Check_3Brick_Row2(tileInfos, pos))
            return true;
        else if (Check_3Brick_Column1(tileInfos, pos))
            return true;
        else if (Check_3Brick_Column2(tileInfos, pos))
            return true;
        return false;

    }

    //oxoo
    private bool Check_3Brick_Row1(TileTypeInfo[][] tileInfos, Vector2Int pos)
    {
        int check_pos_x = pos.x;

        if (check_pos_x - 1 < 0 || check_pos_x + 1 >= tileInfos.Length
            || check_pos_x + 2 >= tileInfos.Length)
        {
            return false;
        }

        if (tileInfos[check_pos_x - 1][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 1][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 2][pos.y].tile_Type != Tile_Type.Ground)
            return false;

        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        tileInfos[check_pos_x - 1][pos.y].brickType = type;
        tileInfos[check_pos_x + 1][pos.y].brickType = type;
        tileInfos[check_pos_x + 2][pos.y].brickType = type;

        return true;
    }

    //ooxo
    private bool Check_3Brick_Row2(TileTypeInfo[][] tileInfos, Vector2Int pos)
    {
        int check_pos_x = pos.x;

        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0
            || check_pos_x + 1 >= tileInfos.Length)
        {
            return false;
        }

        if (tileInfos[check_pos_x - 1][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x - 2][pos.y].tile_Type != Tile_Type.Ground ||
            tileInfos[check_pos_x + 1][pos.y].tile_Type != Tile_Type.Ground)
            return false;

        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        tileInfos[check_pos_x - 1][pos.y].brickType = type;
        tileInfos[check_pos_x - 2][pos.y].brickType = type;
        tileInfos[check_pos_x + 1][pos.y].brickType = type;

        return true;
    }

    //oxoo column
    private bool Check_3Brick_Column1(TileTypeInfo[][] tileInfos, Vector2Int pos)
    {
        int check_pos_y = pos.y;

        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0
            || check_pos_y + 1 >= tileInfos[0].Length)
        {
            return false;
        }

        if (tileInfos[pos.x][check_pos_y - 1].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y - 2].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y + 1].tile_Type != Tile_Type.Ground)
            return false;

        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        tileInfos[pos.x][check_pos_y - 1].brickType = type;
        tileInfos[pos.x][check_pos_y - 2].brickType = type;
        tileInfos[pos.x][check_pos_y + 1].brickType = type;

        return true;
    }

    //ooxo column
    private bool Check_3Brick_Column2(TileTypeInfo[][] tileInfos, Vector2Int pos)
    {
        int check_pos_y = pos.y;

        if (check_pos_y + 1 >= tileInfos[0].Length || check_pos_y + 2 >= tileInfos[0].Length
            || check_pos_y - 1 < 0)
        {
            return false;
        }

        if (tileInfos[pos.x][check_pos_y + 1].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y + 2].tile_Type != Tile_Type.Ground ||
            tileInfos[pos.x][check_pos_y - 1].tile_Type != Tile_Type.Ground)
            return false;
        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        tileInfos[pos.x][check_pos_y + 1].brickType = type;
        tileInfos[pos.x][check_pos_y + 2].brickType = type;
        tileInfos[pos.x][check_pos_y - 1].brickType = type;

        return true;
    }
}
