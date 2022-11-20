using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCreator : MonoBehaviour
{

    //원래 재사용을 위해 brick pool에서 가져와야 한다
    //지금은 임시로 그냥 매번 만들어서 사용하는거로!
    public GameObject[] BrickPrefab;

    private Brick[][] m_Bricks;
    public Brick[][] Bricks
    {
        get { return m_Bricks; }
    }


    private void Awake()
    {
        //브릭오브젝트를 미리 생성해서 브릭풀에 넣어두자
        //각 브릭별로 40개씩 생성

        for(int i = 1; i < (int)BrickType.BrickEnd; i++)
        {
            for(int j = 0; j < 40; j++)
            {
                GameObject obj = Instantiate(BrickPrefab[i]);
                Brick brick = obj.GetComponent<Brick>();
                brick.transform.SetParent(transform);

                BrickPool.Instance().Enqueue_Brick(brick);
            }
        }
        
    }

    //tilemap을 토대로 브릭을 생성?ㄴㄴ
    //외부에서 랜덤하게 계산된 데이터를 토대로 생성
    //얘는 그냥 생성하는 놈일뿐 계산은 딴데서 하자
    public Brick[][] CreateBricksInTile(TileTypeInfo[][] tileTypeInfos)
    {
        //생성에 필요한 데이터는...좌표, 브릭타입

        Brick[][] bricks = new Brick[tileTypeInfos.Length][];


        for (int i = 0; i < tileTypeInfos.Length; i++)
        {
            bricks[i] = new Brick[tileTypeInfos[0].Length];
            for (int j = 0; j < tileTypeInfos[0].Length; j++)
            {
                // 원래는 그라운드 타입이어야하고 맵에디터에서 지정한 자리여야 한다
                if (tileTypeInfos[i][j].tile_Type != Tile_Type.Ground)
                    continue;

                Brick brick = BrickPool.Instance().Dequeue_Brick(tileTypeInfos[i][j].brickType);
                brick.transform.SetParent(transform);
                brick.transform.localPosition = tileTypeInfos[i][j].pos;
                brick.coordinate = new Vector2Int(i, j);

                bricks[i][j] = brick;

                //GameObject obj = Instantiate(BrickPrefab[(int)tileTypeInfos[i][j].brickType]);
                //obj.transform.SetParent(transform);
                //obj.transform.localPosition = tileTypeInfos[i][j].pos;

                //bricks[i][j] = obj.GetComponent<Brick>();
                //bricks[i][j].coordinate = new Vector2Int(i, j);
            }
        }

        return bricks;
    }
}
