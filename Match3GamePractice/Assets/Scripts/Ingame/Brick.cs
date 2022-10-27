using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    //private GameObject m_brick;
    //public GameObject brick
    //{
    //    get { return m_brick; }
    //    set { m_brick = value; }
    //}

    [SerializeField]
    private BrickType m_type;
    public BrickType type
    {
        get { return m_type; }
        set { m_type = value; }
    }


    public Brick(BrickType type)
    {
        m_type = type;
    }

}
