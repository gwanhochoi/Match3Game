using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

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
    private GameObject m_Brick;
    public GameObject BrickObj
    {
        get { return m_Brick; }
        set
        {
            m_Brick = value;
            m_brictype = value.GetComponent<Brick>().type;
            m_Brick.GetComponent<Brick>().coordinate = value.GetComponent<Brick>().coordinate;
            
        }
    }

    private BrickType m_brictype = BrickType.Empty;
    public BrickType bricktype
    {
        get { return m_brictype; }
        set { m_brictype = value; }
    }

    private GroundType m_groundType = GroundType.Back;
    public GroundType groundType
    {
        get { return m_groundType; }
        set { m_groundType = value; }
    }

    public Tile()
    {
        FloorObj_Stack = new Stack<GameObject>();
        CoverObj_Stack = new Stack<GameObject>();
    }

    public void Destroy()
    {
        Destroy(m_Brick);
    }
}
