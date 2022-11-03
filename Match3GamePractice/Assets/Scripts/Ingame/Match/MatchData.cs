using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchData 
{
    private List<Match> m_matchData_List;
    public List<Match> matchData
    {
        get { return m_matchData_List; }

    }

    private List<Match> m_rightCheckDataList;
    public List<Match> rightCheckDataList
    {
        get { return m_rightCheckDataList; }
    }

    private List<Match> m_leftCheckDataList;
    public List<Match> leftCheckDataList
    {
        get { return m_leftCheckDataList; }
    }

    private List<Match> m_upCheckDataList;
    public List<Match> upCheckDataList
    {
        get { return m_upCheckDataList; }
    }

    private List<Match> m_downCheckDataList;
    public List<Match> downCheckDataList
    {
        get { return m_downCheckDataList; }
    }

    private List<Match>[] m_dirCheckDataList;
    public List<Match>[] dirCheckDataList
    {
        get { return m_dirCheckDataList; }
    }

    public MatchData()
    {
        m_matchData_List = new List<Match>(); 
        m_dirCheckDataList = new List<Match>[4];
        m_leftCheckDataList = new List<Match>();
        m_rightCheckDataList = new List<Match>();
        m_upCheckDataList = new List<Match>();
        m_downCheckDataList = new List<Match>();

        var match5row = new Match5Row();
        var match5column = new Match5Column();

        var match5T1 = new Match5T1();
        var match5T2 = new Match5T2();
        var match5T3 = new Match5T3();
        var match5T4 = new Match5T4();

        var match5RA1 = new Match5RA1();
        var match5RA2 = new Match5RA2();
        var match5RA3 = new Match5RA3();
        var match5RA4 = new Match5RA4();

        var match4row1 = new Match4Row1();
        var match4row2 = new Match4Row2();

        var match4column1 = new Match4Column1();
        var match4column2 = new Match4Column2();

        var match3row1 = new Match3Row1();
        var match3row2 = new Match3Row2();
        var match3row3 = new Match3Row3();

        var match3column1 = new Match3Column1();
        var match3column2 = new Match3Column2();
        var match3column3 = new Match3Column3();

        m_matchData_List.Add(match5row);
        m_matchData_List.Add(match5column);

        m_matchData_List.Add(match5T1);
        m_matchData_List.Add(match5T2);
        m_matchData_List.Add(match5T3);
        m_matchData_List.Add(match5T4);

        m_matchData_List.Add(match5RA1);
        m_matchData_List.Add(match5RA2);
        m_matchData_List.Add(match5RA3);
        m_matchData_List.Add(match5RA4);

        m_matchData_List.Add(match4row1);
        m_matchData_List.Add(match4row2);

        m_matchData_List.Add(match4column1);
        m_matchData_List.Add(match4column2);

        m_matchData_List.Add(match3row1);
        m_matchData_List.Add(match3row2);
        m_matchData_List.Add(match3row3);

        m_matchData_List.Add(match3column1);
        m_matchData_List.Add(match3column2);
        m_matchData_List.Add(match3column3);

        //right
        m_rightCheckDataList.Add(match5column);
        m_rightCheckDataList.Add(match5T1);
        m_rightCheckDataList.Add(match5RA1);
        m_rightCheckDataList.Add(match5RA2);
        m_rightCheckDataList.Add(match4column1);
        m_rightCheckDataList.Add(match4column2);
        m_rightCheckDataList.Add(match3column1);
        m_rightCheckDataList.Add(match3column2);
        m_rightCheckDataList.Add(match3column3);
        m_rightCheckDataList.Add(match3row1);

        //left
        m_leftCheckDataList.Add(match5column);
        m_leftCheckDataList.Add(match5T2);
        m_leftCheckDataList.Add(match5RA3);
        m_leftCheckDataList.Add(match5RA4);
        m_leftCheckDataList.Add(match4column1);
        m_leftCheckDataList.Add(match4column2);
        m_leftCheckDataList.Add(match3column1);
        m_leftCheckDataList.Add(match3column2);
        m_leftCheckDataList.Add(match3column3);
        m_leftCheckDataList.Add(match3row3);

        //up
        m_upCheckDataList.Add(match5row);
        m_upCheckDataList.Add(match5T4);
        m_upCheckDataList.Add(match5RA2);
        m_upCheckDataList.Add(match5RA3);
        m_upCheckDataList.Add(match4row1);
        m_upCheckDataList.Add(match4row2);
        m_upCheckDataList.Add(match3row1);
        m_upCheckDataList.Add(match3row2);
        m_upCheckDataList.Add(match3row3);
        m_upCheckDataList.Add(match3column3);

        //down
        m_downCheckDataList.Add(match5row);
        m_downCheckDataList.Add(match5T3);
        m_downCheckDataList.Add(match5RA1);
        m_downCheckDataList.Add(match5RA4);
        m_downCheckDataList.Add(match4row1);
        m_downCheckDataList.Add(match4row2);
        m_downCheckDataList.Add(match3row1);
        m_downCheckDataList.Add(match3row2);
        m_downCheckDataList.Add(match3row3);
        m_downCheckDataList.Add(match3column1);

        m_dirCheckDataList[0] = m_leftCheckDataList;
        m_dirCheckDataList[1] = m_rightCheckDataList;
        m_dirCheckDataList[2] = m_upCheckDataList;
        m_dirCheckDataList[3] = m_downCheckDataList;

    }
}
