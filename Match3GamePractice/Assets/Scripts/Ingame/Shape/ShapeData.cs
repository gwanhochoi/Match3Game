using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeData
{
    private List<Shape> m_shapeData_List;
    public List<Shape> shapeData
    {
        get { return m_shapeData_List; }
        
    }
    public ShapeData()
    {
        //일단 모양 수작업으로 만들어서 넣고 나중에 툴 만들어서 데이터 따로 갖고 있어도 될듯?
        //그냥 json파일에 넣어둘까?
        m_shapeData_List = new List<Shape>();

        //아이템끼리 조합하는게 첫번째고(그 안에서도 갈림)
        //5개긴거, 5개 ㄱ자T자, 4개 순으로 찾고
        //마지막으로 3개짜리 찾고

        //5개긴거 가로 세로
        //ooxoo   o
        //  o   ooxoo

        var shape5row1 = new Shape5_Row1();
        var shape5row2 = new Shape5_Row2();


        //o   o
        //o   o
        //xo ox
        //o   o
        //o   o
        var shape5column1 = new Shape5_Column1();
        var shape5column2 = new Shape5_Column2();



        //5개 T모양
        // o    o
        //oxo   o     o    o
        // o   oxo  ooxo  oxoo
        // o    o     o    o
        var shape5T1 = new Shape5_T1();
        var shape5T2 = new Shape5_T2();
        var shape5T3 = new Shape5_T3();
        var shape5T4 = new Shape5_T4();


        //5개 ㄱ자모양 총 8개
        //      o            o   o   o      o     o
        // oxoo xoo  ooxo  oox   o   o      o     o
        //  o   o      o     o  oxoo xoo  ooxo  oox
        //  o   o      o     o       o            o

        var shape5RA1 = new Shape5_RA1();
        var shape5RA2 = new Shape5_RA2();
        var shape5RA3 = new Shape5_RA3();
        var shape5RA4 = new Shape5_RA4();
        var shape5RA5 = new Shape5_RA5();
        var shape5RA6 = new Shape5_RA6();
        var shape5RA7 = new Shape5_RA7();
        var shape5RA8 = new Shape5_RA8();


        //4개모양 8개
        //  o          o        o o   o o
        // oxoo oxoo ooxo ooxo ox xo  o o
        //       o          o   o o  ox xo
        //                      o o   o o

        var shape4row1 = new Shape4_Row1();
        var shape4row2 = new Shape4_Row2();
        var shape4row3 = new Shape4_Row3();
        var shape4row4 = new Shape4_Row4();

        var shape4column1 = new Shape4_Column1();
        var shape4column2 = new Shape4_Column2();
        var shape4column3 = new Shape4_Column3();
        var shape4column4 = new Shape4_Column4();


        ///3개짜리
        //oxoo ooxo o o
        //          x o
        //          o x
        //          o o

        //xoo o   oxo  o  oox   o
        //o   xoo  o  oxo   o oox

        //xo ox o   o  o   o
        //o   o xo ox  o   o
        //o   o o   o  xo ox

        var shape3row1 = new Shape3_Row1();
        var shape3row2 = new Shape3_Row2();
        var shape3row3 = new Shape3_Row3();
        var shape3row4 = new Shape3_Row4();
        var shape3row5 = new Shape3_Row5();
        var shape3row6 = new Shape3_Row6();
        var shape3row7 = new Shape3_Row7();
        var shape3row8 = new Shape3_Row8();

        var shape3column1 = new Shape3_Column1();
        var shape3column2 = new Shape3_Column2();
        var shape3column3 = new Shape3_Column3();
        var shape3column4 = new Shape3_Column4();
        var shape3column5 = new Shape3_Column5();
        var shape3column6 = new Shape3_Column6();
        var shape3column7 = new Shape3_Column7();
        var shape3column8 = new Shape3_Column8();

        m_shapeData_List.Add(shape5row1);
        m_shapeData_List.Add(shape5row2);
        m_shapeData_List.Add(shape5column1);
        m_shapeData_List.Add(shape5column2);

        m_shapeData_List.Add(shape5T1);
        m_shapeData_List.Add(shape5T2);
        m_shapeData_List.Add(shape5T3);
        m_shapeData_List.Add(shape5T4);

        m_shapeData_List.Add(shape5RA1);
        m_shapeData_List.Add(shape5RA2);
        m_shapeData_List.Add(shape5RA3);
        m_shapeData_List.Add(shape5RA4);
        m_shapeData_List.Add(shape5RA5);
        m_shapeData_List.Add(shape5RA6);
        m_shapeData_List.Add(shape5RA7);
        m_shapeData_List.Add(shape5RA8);

        m_shapeData_List.Add(shape4row1);
        m_shapeData_List.Add(shape4row2);
        m_shapeData_List.Add(shape4row3);
        m_shapeData_List.Add(shape4row4);

        m_shapeData_List.Add(shape4column1);
        m_shapeData_List.Add(shape4column2);
        m_shapeData_List.Add(shape4column3);
        m_shapeData_List.Add(shape4column4);

        m_shapeData_List.Add(shape3row1);
        m_shapeData_List.Add(shape3row2);
        m_shapeData_List.Add(shape3row3);
        m_shapeData_List.Add(shape3row4);
        m_shapeData_List.Add(shape3row5);
        m_shapeData_List.Add(shape3row6);
        m_shapeData_List.Add(shape3row7);
        m_shapeData_List.Add(shape3row8);

        m_shapeData_List.Add(shape3column1);
        m_shapeData_List.Add(shape3column2);
        m_shapeData_List.Add(shape3column3);
        m_shapeData_List.Add(shape3column4);
        m_shapeData_List.Add(shape3column5);
        m_shapeData_List.Add(shape3column6);
        m_shapeData_List.Add(shape3column7);
        m_shapeData_List.Add(shape3column8);

    }


}
