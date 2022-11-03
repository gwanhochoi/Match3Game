using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Match 
{
    //우측방향                          
    //xoo x o o o o   xoo  o     o    o
    //    o x o x o   o    o     xoo  o
    //    o o x o x   o    xoo   o    x
    //          o o                   o
    //                                o


    //좌측방향
    //oox x o o o o   oox    o     o  o
    //    o x o x o     o    o   oox  o
    //    o o x o x     o  oox     o  x
    //          o o                   o
    //                                o

    //아래방향
    //xoo oxo oox x oxoo ooxo  xoo  oox  oxo  ooxoo
    //            o            o      o   o
    //            o            o      o   o
    //            
    //            

    //윗방향
    //xoo oxo oox o oxoo ooxo  o      o   o   ooxoo
    //            o            o      o   o
    //            x            xoo  oox  oxo
    //            
    // 


    private List<Vector2Int> m_d_List;
    public List<Vector2Int> d_list
    {
        get { return m_d_List; }
        set { m_d_List = value; }
    }

    private Bomb m_Bomb;
    public Bomb bomb
    {
        get { return m_Bomb; }
        set { m_Bomb = value; }
    }

    public Match()
    {
        m_d_List = new List<Vector2Int>();
        m_Bomb = Bomb.None;
    }

    protected void Add_Point(Vector2Int point)
    {
        m_d_List.Add(point);
    }
}
