using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{


    private void Awake()
    {
        
        Application.targetFrameRate = 60;

    }


    public void CreateBlock_InField()
    {
        //원래는 블럭 놓을수 있는 위치에만 생성해야하지만 임시로 일단 맵 최대크기 다 사용한다고 가정하고 생성하자.
        //일단 타일에 따른 처리는 아직 안하므로 높이를 4개 줄인상태로 9x9로 한다
        GetComponent<GameField>().Fill_Bricks();
        
    }

    

    public void Find_Shape()
    {
        GetComponent<GameField>().Find_Shape();
    }

    

}
