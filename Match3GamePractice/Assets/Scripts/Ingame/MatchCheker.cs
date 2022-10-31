using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCheker
{

    private int max_widthCount;
    private int max_heightCount;

    //private BrickType[] m_Bricks_copy;

    private ShapeData m_ShapeData;

    public MatchCheker()
    {
        m_ShapeData = new ShapeData();
    }

    public void Init()
    {
        //m_Bricks_copy = (BrickType[])bricks.Clone();
        max_widthCount = GameDataMGR.Instance.MaxWidthCount;
        max_heightCount = GameDataMGR.Instance.MaxHeightCount;

        
    }

    public List<Vector2Int> Find_Shape(BrickType[][] bricks)
    {
        //현재 브릭들중 지울 수 있는 브릭모양을 찾는다

        //Debug.Log("bricks length = " + bricks.Length);
        //Debug.Log("bricks[0] length = " + bricks[0].Length);

        //shape 안에 좌표를 넣어야 할듯.
        foreach (var shape in m_ShapeData.shapeData)
        {
            //shape모양 좌표가져와서 해당 브릭들이 모두 같은 타입인지 검사


            for (int i = 0; i < bricks.Length; i++)
            {
                for (int j = 0; j < bricks[i].Length; j++)
                {
                    Vector2Int std_pos = new Vector2Int(i + shape.d_list[0].x, j + shape.d_list[0].y);
                    if (!CheckCoordinate_InField(std_pos))
                        continue;
                    int shape_count = shape.d_list.Count;
                    int type_count = 1;
                    BrickType type = bricks[std_pos.x][std_pos.y];
                    List<Vector2Int> coordinate_list = new List<Vector2Int>();
                    coordinate_list.Add(std_pos);
                    for (int k = 1; k < shape_count; k++)
                    {
                        Vector2Int pos = new Vector2Int(i + shape.d_list[k].x, j + shape.d_list[k].y);
                        if (!CheckCoordinate_InField(pos))
                            break;
                        if (type == bricks[pos.x][pos.y])
                        {
                            coordinate_list.Add(pos);
                            type_count++;
                        }

                    }

                    if (type_count == shape_count)
                    {
                        //해당 모양 좌표의 브릭 타입이 모두 같다
                        //타입과 좌표 따로 갖고 있자

                        Debug.Log("Bricktype = " + type);

                        foreach (var child in coordinate_list)
                        {
                            Debug.Log("[" + child.x + "," + child.y + "]");
                        }


                        return coordinate_list; //일단 하나만 찾자
                    }


                }
            }

            

        }


        return null;

        
    }

    private bool CheckCoordinate_InField(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= 9 || pos.y < 0 || pos.y >= 9)
            return false;
        return true;
    }

    public void Match_Check(Vector2Int coordinate)
    {
        //자리를 바꿨다고 가정하고 체크
    }


    
}
