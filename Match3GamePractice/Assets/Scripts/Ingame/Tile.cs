using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile : MonoBehaviour
{

    private Vector2Int m_coordinate;
    public Vector2Int Coordinate
    {
        get { return m_coordinate; }
        set { m_coordinate = value; }
    }

    private void Awake()
    {
        FloorObj_Stack = new Stack<GameObject>();
        CoverObj_Stack = new Stack<GameObject>();

        m_LinkedUpTileList = new List<Tile>();
        m_LinkedDownTileList = new List<Tile>();
        //m_brictype = BrickType.Empty;
    }

    private Sprite m_BackSprite;
    public Sprite BackSprite
    {
        get { return m_BackSprite; }
        set
        {
            m_BackSprite = value;
            GetComponent<SpriteRenderer>().sprite = value;
            
        }
    }
    public void Set_BackSpriteSize(int CellSize)
    {
        GetComponent<SpriteRenderer>().size = new Vector2(CellSize, CellSize);
    }

    private Stack<GameObject> FloorObj_Stack;
    private Stack<GameObject> CoverObj_Stack;

    private Brick m_BrickScript;
    public Brick BrickScript
    {
        get { return m_BrickScript; }
        set {

            if (value == null)
            {
                m_BrickScript = null;
                return;
            }
                
            m_BrickScript = value;
            m_BrickScript.coordinate = m_coordinate;
            //m_brictype = value.type;
        }
    }


    //private BrickType m_brictype;
    public BrickType bricktype
    {
        get {

            if (m_BrickScript != null)
                return m_BrickScript.type;
            else 
                return BrickType.Empty;

        }
        //set { m_BrickScript.type = value; }
    }

    [SerializeField]
    private Tile_Type m_tileType;
    public Tile_Type tileType
    {
        get { return m_tileType; }
        set { m_tileType = value; }
    }

    //private GroundType m_groundType = GroundType.Back;
    //public GroundType groundType
    //{
    //    get { return m_groundType; }
    //    set { m_groundType = value; }
    //}


    private Tile m_GravityTile = null;
    public Tile GraviyTile
    {
        get { return m_GravityTile; }
        set
        {
            //빈공간에서 땡겨지는게 우선순위 높음
            //우선순위 아래 왼아 오아
            //현재 방향이 어딘가

            if(value == null)
            {
                m_GravityTile = null;
                return;
            }

            if(m_GravityTile == null)
            {
                m_GravityTile = value;
                return;
            }

            //아래는 dx 0 dy -1 왼아 dx -1 dy -1 우아 dx 1 dy 1
            int current_dx = m_coordinate.x - m_GravityTile.Coordinate.x;
            //int current_dy = m_coordinate.y - m_GravityTile.Coordinate.y;

            int dx = m_coordinate.x - value.Coordinate.x;
            //int dy = m_coordinate.y = value.Coordinate.y;

            bool priority = GravityPriority(current_dx, dx);

            //이미 빈공간에서 당겨지면 방향으로 우선순위를 가려야한다
            if (m_GravityTile.bricktype == BrickType.Empty)
            {
                if(value.bricktype != BrickType.Empty)
                {
                    //기존은 빈공간으로부터 당겨지는데 새로 들어온것은 빈공간이 아니므로 교체는 없다
                    return;
                }

                //방향으로 가린다
                if(priority)
                {
                    m_GravityTile = value;
                }
                
            }
            else
            {
                //빈공간에서 당겨지는게 아닐때 빈공간에서 당기면 무조건 빈공간쪽으로 바뀐다
                if(value.bricktype == BrickType.Empty)
                {
                    m_GravityTile = value;
                    return;
                }
                //기존것과 새로운것 둘다 빈공간이 아니므로 방향으로 우선순위를 가린다
                if (priority)
                {
                    m_GravityTile = value;
                }
            }




        }
    }

    private bool GravityPriority(int current_dx, int dx)
    {
        if (current_dx == 0)
        {
            return false;
        }
        else if (current_dx == -1)
        {
            if (dx == 0)
                return true;
            return false;
        }
        else
        {
            return true;
        }
    }



    private List<Tile> m_LinkedDownTileList;
    public List<Tile> LinkedDownTileList
    {
        get { return m_LinkedDownTileList; }
    }

    private List<Tile> m_LinkedUpTileList;
    public List<Tile> LinkedUpTileList
    {
        get { return m_LinkedUpTileList; }
    }
    public void LinkedUpTile(Tile tile)
    {
        //위 왼쪽위 오른쪽위 우선순위로 연결이 들어온다
        m_LinkedUpTileList.Add(tile);

    }

    public void LinkedDownTile(Tile tile)
    {
        m_LinkedDownTileList.Add(tile);
    }

    public void BrickMoveInTile(Action Func)
    {
        if (BrickScript == null)
        {
            Func();
            return;
        }

        BrickScript.Move(GraviyTile.transform.localPosition, Func);
        GraviyTile.BrickScript = BrickScript;
        BrickScript = null;
    }

    public bool Tile_MoveState()
    {
        //이타일이 그라운드가 아니고 아이스나 특수한 타일이라 이동이 불가능한지 여부
        if (m_tileType == Tile_Type.Back)
            return false;

        return true;
    }

    public void Destroy()
    {
        if(m_BrickScript != null)
            Destroy(m_BrickScript.gameObject);
    }

    public void Reset()
    {
        //data reset

    }
}
