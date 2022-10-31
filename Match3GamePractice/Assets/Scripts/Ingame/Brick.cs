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

    private Vector2Int m_coordinate;
    public Vector2Int coordinate
    {
        get { return m_coordinate; }
        set { m_coordinate = value; }
    }


    public Brick(BrickType type)
    {
        m_type = type;
    }



    public void Move(Vector2 dst)
    {
        //dst까지 이동하는데 부드럽게 이동해야함

        //StartCoroutine(Move_Cor(dst));
        StartCoroutine(ReturnMove_Cor(dst));

    }

    IEnumerator Move_Cor(Vector2 dst)
    {
        float elapseTime = 0;
        float waitTime = 0.15f;
        Vector3 currentPos = transform.position;
        while(elapseTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, dst, elapseTime / waitTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.position = dst;
        //Debug.Log("move complete");
        yield return null;


    }

    IEnumerator ReturnMove_Cor(Vector2 dst)
    {
        Vector3 originpos = transform.position;
        float elapseTime = 0;
        float waitTime = 0.15f;
        Vector3 currentPos = transform.position;
        while (elapseTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, dst, elapseTime / waitTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.position = dst;
        //Debug.Log("move complete");
        currentPos = transform.position;
        elapseTime = 0;
        while (elapseTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, originpos, elapseTime / waitTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originpos;

        yield return null;


    }

}
