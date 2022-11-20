using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPool
{
    //게임에서 사용할 브릭들을 미리 생성해놓는다
    //재사용하기 위함
    //일단 맵 최대크기는 9x9 이므로 최대 81개의 브릭이 맵에 사용된다
    //각 브릭당 30개정도만 만들까?

    private static BrickPool m_Instance;
    public static BrickPool Instance()
    {
        if(m_Instance == null)
        {
            m_Instance = new BrickPool();
        }

        return m_Instance;
    }

    private Queue<Brick>[] m_BrickQueueArray;

    public BrickPool()
    {
        m_BrickQueueArray = new Queue<Brick>[(int)BrickType.BrickEnd];
        for(int i = 0; i < (int)BrickType.BrickEnd; i++)
        {
            m_BrickQueueArray[i] = new Queue<Brick>();
        }
    }

    public void Enqueue_Brick(Brick brick)
    {
        //넣기전에 위치를 화면에 안보이는 곳으로 옮기자
        brick.transform.localPosition = new Vector3(-5000, -5000);
        int type = (int)brick.type;
        m_BrickQueueArray[type].Enqueue(brick);
    }

    public Brick Dequeue_Brick(BrickType type)
    {
        int index = (int)type;
        if(m_BrickQueueArray[index].Count > 0)
        {
            return m_BrickQueueArray[index].Dequeue();
        }
        return null;
    }


    public Brick Dequeue_RandomBrick()
    {
        //
        int index = Random.Range(1, (int)BrickType.BrickEnd);

        
        if(m_BrickQueueArray[index].Count == 0)
        {
            //해당 brick이 없으면 다른 브릭을 빼자
            List<int> index_list = new List<int>();
            for(int i = 1; i < (int)BrickType.BrickEnd; i++)
            {
                index_list.Add(i);
            }

            while (index_list.Count > 0)
            {
                int count = index_list.Count;
                int new_index = index_list[Random.Range(0, count)];

                if (m_BrickQueueArray[new_index].Count > 0)
                    return m_BrickQueueArray[new_index].Dequeue();
                index_list.Remove(new_index);
            }
            return null;
        }
        return m_BrickQueueArray[index].Dequeue();
    }

}
