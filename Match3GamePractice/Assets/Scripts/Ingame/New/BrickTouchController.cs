using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTouchController : MonoBehaviour
{
    private int action_count = 0;
    private GameObject begin_selected_obj;
    private GameObject second_selected_obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private GameObject GetTouchedBrick(Vector2 pos)
    {
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.collider.tag == "Brick")
        {
            return hit.collider.gameObject;
        }
        return null;

    }

    // Update is called once per frame
    void Update()
    {
        //Touch event


        if (Input.touchCount > 0 && action_count == 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            Vector2 pos = Camera.main.ScreenToWorldPoint(touchPos);



            switch (touch.phase)
            {
                case TouchPhase.Began:

                    begin_selected_obj = GetTouchedBrick(pos);

                    //if (begin_selected_obj != null)
                    //{
                    //    m_brickController.StopTwinkle();
                    //}

                    break;

                case TouchPhase.Moved:
                    //선택된 브릭이 있어야하고 이미 브릭이 이동상태가 아니어야한다.
                    //선택된 브릭이 있는 상태로 이동해서 브릭이 바뀌면 이동이라 판단.
                    if (begin_selected_obj != null)
                    {

                        //대각선이 선택될 수도 있기때문에 보정이 필요하다.
                        //손가락을 빠르게 이동하면 한칸 건너서 선택되기도 한다.
                        //
                        second_selected_obj = GetTouchedBrick(pos);

                        if (second_selected_obj != null && second_selected_obj != begin_selected_obj)
                        {
                            //스왑 상태로 상태변경 할 것.
                            //브릭 스왑 진행하고 스왑이 완료되거나(아무것도 못지운경우)
                            //지우거나 아이템 효과가 끝나는 등 모든 프로세스 진행이 완료되면 다시 상태 변경
                            //Swap_Two_Brick_Check(begin_selected_obj, second_selected_obj);
                        }
                    }
                    break;


                case TouchPhase.Ended:
                    if (begin_selected_obj != null)
                        begin_selected_obj = null;
                    break;
            }


        }
    }
}
