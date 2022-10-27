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
    BrickEnd
}



public class GameField : MonoBehaviour
{
    public GameObject[] BrickPrefabs;

    //private Dictionary<Vector2Int, Brick> m_brick_dic;
    private BrickType[][] m_bricks;

    private int max_widthCount;
    private int max_heightCount;

    private delegate bool CheckMatchForCreate(Vector2Int pos, BrickType type);
    private CheckMatchForCreate[] CheckMatchForCreates_Del;

    private List<GameObject> m_BrickObj_List;

    private void Awake()
    {
        m_bricks = new BrickType[9][];
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

        m_BrickObj_List = new List<GameObject>();
        //for(int i = 0; i < 13; i++)
        //{
        //    m_bricks[]
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            m_bricks[i] = new BrickType[13];
            for (int j = 0; j < 13; j++)
            {
                m_bricks[i][j] = BrickType.Empty;
            }

        }

    }

    

    private bool Find_PositionableBrick(Vector2Int pos)
    {
        //해당위치가 주변 브릭들과 관계에서 가능한 위치인가 체크하자
        //해당위치가 브릭이 올려질수 있는 타일인가
        //3칸 세로, 3칸 가로, 4칸 세로, 4칸 가로, 5칸 세로, 5칸 가로
        //4칸 사각형, 5칸 ㄱ자 4방향, T모양 5칸




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

        if (m_bricks[pos.x][pos.y] != BrickType.Empty)
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
                m_bricks[pos.x][pos.y] = type;
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

        if (m_bricks[check_pos_x - 1][pos.y] == m_bricks[check_pos_x + 1][pos.y])
        {
            if (m_bricks[check_pos_x - 1][pos.y] == type)
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

        if (m_bricks[pos.x][check_pos_y - 1] == m_bricks[pos.x][check_pos_y + 1])
        {
            if (m_bricks[pos.x][check_pos_y - 1] == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Left(Vector2Int pos, BrickType type)
    {
        //left 2개
        int check_pos_x = pos.x;

        if (check_pos_x - 1 < 0 || check_pos_x - 2 < 0)
            return true;

        if(m_bricks[check_pos_x - 1][pos.y] == m_bricks[check_pos_x - 2][pos.y])
        {
            if (m_bricks[check_pos_x - 1][pos.y] == type)
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

        if (m_bricks[check_pos_x + 1][pos.y] == m_bricks[check_pos_x + 2][pos.y])
        {
            if (m_bricks[check_pos_x + 1][pos.y] == type)
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

        if (m_bricks[pos.x][check_pos_y + 1] == m_bricks[pos.x][check_pos_y + 2])
        {
            if (m_bricks[pos.x][check_pos_y + 1] == type)
                return false;
        }

        return true;
    }

    private bool Check_3Brick_Down(Vector2Int pos, BrickType type)
    {
        //up 2개
        int check_pos_y = pos.y;

        if (check_pos_y - 1 < 0 || check_pos_y - 2 < 0)
            return true;

        if (m_bricks[pos.x][check_pos_y - 1] == m_bricks[pos.x][check_pos_y - 2])
        {
            if (m_bricks[pos.x][check_pos_y - 1] == type)
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

         if(m_bricks[check_pos_x - 1][check_pos_y] == m_bricks[check_pos_x - 1][check_pos_y + 1] ?
            m_bricks[check_pos_x - 1][check_pos_y + 1] == m_bricks[check_pos_x][check_pos_y + 1] : false)
        {
            if(m_bricks[check_pos_x - 1][check_pos_y] == type)
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

        if (m_bricks[check_pos_x - 1][check_pos_y] == m_bricks[check_pos_x - 1][check_pos_y - 1] ?
           m_bricks[check_pos_x - 1][check_pos_y - 1] == m_bricks[check_pos_x][check_pos_y - 1] : false)
        {
            if (m_bricks[check_pos_x - 1][check_pos_y] == type)
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

        if (m_bricks[check_pos_x + 1][check_pos_y] == m_bricks[check_pos_x + 1][check_pos_y + 1] ?
           m_bricks[check_pos_x + 1][check_pos_y + 1] == m_bricks[check_pos_x][check_pos_y + 1] : false)
        {
            if (m_bricks[check_pos_x + 1][check_pos_y] == type)
            {
                return false;
            }
        }

        return true;

    }

    private bool Check_4Brick_RD(Vector2Int pos, BrickType type)
    {
        //right up 3개
        int check_pos_x = pos.x;
        int check_pos_y = pos.y;

        if (check_pos_x + 1 >= max_widthCount || check_pos_y - 1 < 0)
            return true;

        if (m_bricks[check_pos_x + 1][check_pos_y] == m_bricks[check_pos_x + 1][check_pos_y - 1] ?
           m_bricks[check_pos_x + 1][check_pos_y - 1] == m_bricks[check_pos_x][check_pos_y - 1] : false)
        {
            if (m_bricks[check_pos_x + 1][check_pos_y] == type)
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

        if (m_bricks[pos.x][pos.y] != BrickType.Empty)
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
        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_bricks[check_pos_x - 1][pos.y] = type;
        m_bricks[check_pos_x + 1][pos.y] = type;
        m_bricks[check_pos_x + 2][pos.y] = type;
        //List<Vector2Int> list = new List<Vector2Int>();
        //list.Add(new Vector2Int(check_pos_x - 1, pos.y));
        //list.Add(new Vector2Int(check_pos_x + 1, pos.y));
        //list.Add(new Vector2Int(check_pos_x + 2, pos.y));

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
        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_bricks[check_pos_x - 1][pos.y] = type;
        m_bricks[check_pos_x - 2][pos.y] = type;
        m_bricks[check_pos_x + 1][pos.y] = type;
        //List<Vector2Int> list = new List<Vector2Int>();
        //list.Add(new Vector2Int(check_pos_x - 1, pos.y));
        //list.Add(new Vector2Int(check_pos_x - 2, pos.y));
        //list.Add(new Vector2Int(check_pos_x + 1, pos.y));

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
        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_bricks[pos.x][check_pos_y - 1] = type;
        m_bricks[pos.x][check_pos_y - 2] = type;
        m_bricks[pos.x][check_pos_y + 1] = type;
        //List<Vector2Int> list = new List<Vector2Int>();
        //list.Add(new Vector2Int(pos.x, check_pos_y - 1));
        //list.Add(new Vector2Int(pos.x, check_pos_y - 2));
        //list.Add(new Vector2Int(pos.x, check_pos_y + 1));

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
        BrickType type = (BrickType)Random.Range(1, (int)BrickType.BrickEnd);
        m_bricks[pos.x][check_pos_y + 1] = type;
        m_bricks[pos.x][check_pos_y + 2] = type;
        m_bricks[pos.x][check_pos_y - 1] = type;
        //List<Vector2Int> list = new List<Vector2Int>();
        //list.Add(new Vector2Int(pos.x, check_pos_y + 1));
        //list.Add(new Vector2Int(pos.x, check_pos_y + 2));
        //list.Add(new Vector2Int(pos.x, check_pos_y - 1));

        return true;
    }


    public void Fill_Bricks(int width_count, int height_count)
    {

        //일단 맵 전체에 브릭 생성
        //브릭 생성시 지워지는 브릭이 안되도록 배치해야 한다

        //vector2 넣고 랜덤으로 가져오기

        //원래는 가능한 타일 위치를 가져와서 해야함
        if(m_BrickObj_List.Count > 0)
        {
            for(int i = 0; i < max_widthCount; i++)
            {
                for(int j = 0; j < max_heightCount; j++)
                {
                    m_bricks[i][j] = BrickType.Empty;
                }
            }

            foreach(var child in m_BrickObj_List)
            {
                Destroy(child);
            }
            m_BrickObj_List.Clear();
        }

        max_widthCount = width_count;
        max_heightCount = height_count;

        //Vector2[] brick_pos_vec = new Vector2[width_count * height_count];

        List<Vector2Int> brick_pos_list = new List<Vector2Int>();
        

        //list에 넣고 랜덤하게 뽑아오기

        for (int i = 0; i < height_count; i++)
        {
            for(int j = 0; j < width_count; j++)
            {
                //brick_pos_vec[i * height_count + j] = new Vector2(j, i);
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


        


        for (int i = 0; i < width_count; i++)
        {
            for (int j = 0; j < height_count; j++)
            {
                Find_PositionableBrick(new Vector2Int(i, j));
                CreateBrick(new Vector2((width_count * 100 / -2.0f + 50) + 100 * i,
                    (height_count * 100 / -2.0f + 50) + 100 * j), m_bricks[i][j]);
            }
        }


    }


    private void CreateBrick(Vector2 pos, BrickType brickType)
    {
        GameObject obj = Instantiate(BrickPrefabs[(int)brickType]);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = pos;

        m_BrickObj_List.Add(obj);

        //add obj

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
