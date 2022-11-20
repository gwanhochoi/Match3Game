using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private SpriteRenderer m_SpriteRenderer;



    public Brick(BrickType type)
    {
        m_type = type;
        
    }

    private void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void MoveReserVation(Tile tile)
    {
        //아래 왼쪽아래 오른쪽 아래 우선순위
        
        
    }
   

    public void Move(Vector2 dst, Action Func)
    {
        //dst까지 이동하는데 부드럽게 이동해야함
        //GameField.action_count++;
        StartCoroutine(Move_Cor(dst, Func));

    }

    public void ReturnMove(Vector2 dst, Action Func)
    {
        //GameField.action_count++;
        StartCoroutine(ReturnMove_Cor(dst, Func));
    }


    IEnumerator Move_Cor(Vector2 dst, Action Func)
    {
        float elapseTime = 0;
        float waitTime = 0.15f;
        Vector3 currentPos = transform.localPosition;
        while(elapseTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(currentPos, dst, elapseTime / waitTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = dst;
        //Debug.Log("move complete");
        Func();
        


    }

    IEnumerator ReturnMove_Cor(Vector2 dst, Action Func)
    {
        Vector3 originpos = transform.localPosition;
        float elapseTime = 0;
        float waitTime = 0.15f;
        Vector3 currentPos = transform.localPosition;
        while (elapseTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(currentPos, dst, elapseTime / waitTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = dst;
        //Debug.Log("move complete");
        currentPos = transform.localPosition;
        elapseTime = 0;
        while (elapseTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(currentPos, originpos, elapseTime / waitTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originpos;

        Func();


    }

    public void Twinkle()
    {
        if (Shape_Twinkle_Ie != null)
            StopCoroutine(Shape_Twinkle_Ie);

        m_SpriteRenderer.color = new Color(1, 1, 1);
        Shape_Twinkle_Ie = Shape_Twinkle_Cor();
        StartCoroutine(Shape_Twinkle_Ie);
    }

    public void StopTwinkle()
    {
        if (Shape_Twinkle_Ie != null)
            StopCoroutine(Shape_Twinkle_Ie);

        m_SpriteRenderer.color = new Color(1, 1, 1);
    }

    IEnumerator Shape_Twinkle_Ie;
    IEnumerator Shape_Twinkle_Cor()
    {
        float color = 1.0f;
        float value = -0.1f;
        while (true)
        {
            color += value;
            m_SpriteRenderer.color = new Color(color, color, color);
            if (color >= 1.0f)
            {
                value = -0.1f;
            }
            if (color <= 0.5f)
            {
                value = 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
