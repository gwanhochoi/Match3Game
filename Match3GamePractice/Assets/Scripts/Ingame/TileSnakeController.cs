using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSnakeController
{

    //private List<TileSnake> m_tileSnakeList;
    //public List<TileSnake> TileSnakeList
    //{
    //    get { return m_tileSnakeList; }
    //}
    public TileSnakeController()
    {
        //m_tileSnakeList = new List<TileSnake>();
        //m_TileSnake = new TileSnake();

        //m_TileSnake = new TileSnake();
    }

    //private TileSnake m_TileSnake;
    //public TileSnake tileSnake
    //{
    //    get { return m_TileSnake; }
    //}

    //한번 이동하고 머리부터 이동완료 체크하고 이동완료인 브릭은 리스트에 담고
    //리스트에서 매치검사하고 지우고 다시 타일스네이크 검사하고
    //일단 여기서는 타일스테이크를 찾는것만 하고
    //이동은 밖에서 하는것

    public TileSnake SearchTileSanke(Tile[][] tiles, List<Vector2Int> list)
    {
        //List<TileSnake> tileSnakeList = new List<TileSnake>();
        TileSnake tileSnake = new TileSnake();
        foreach (var pos in list)
        {
            var tile = tiles[pos.x][pos.y];
            if (tile.BrickScript != null)
            {
                //Debug.Log("pos_x = " + pos.x);
                //Debug.Log("pos_y = " + pos.y);
                continue;
            }
                
            //TileSnake tileSnake = new TileSnake();
            while (tile.LinkedUpTileList.Count > 0 && tile.tileType == Tile_Type.Ground)
            {
                int false_Count = 0;
                //int child_count = 0;
                foreach (var child in tile.LinkedUpTileList)
                {
                    //현재 이동이 불가능한 타일이거나 불가능한 지역이면 다음위치
                    if (!child.Tile_MoveState())
                    {
                        false_Count++;
                        continue;
                    }

                    if (child.bricktype != BrickType.Empty)
                    {
                        
                        //Debug.Log("tileSnake AddTile");
                        tileSnake.Add_Tile(child);
                        child.GraviyTile = tile;
                        
                    }
                    else
                    {
                        if(child.tileType == Tile_Type.Charge)
                        {
                            Tile_Charge tile_Charge = child as Tile_Charge;
                            tileSnake.Add_Tile(child);
                            tile_Charge.GraviyTile = tile;
                            tile_Charge.Charge_Brick();

                        }
                    }
                        
                    //3군데 검사해서 하나만 만족하면 된다.
                    //만약에 비어있는 타일이라면 Add는 하지 않고 다음 검사하면 된다
                    //3군데 모두 Add할 수 없으면 끝이다
                    //구현은 아직 안되있지만 현재타일이 충전타일 아래 위치면 그때도 끝

                    tile = child;
                    break;

                }
                //세군데 모두 아이스같은 상태때문에 이동이 불가 할수도 있다
                if (false_Count == tile.LinkedUpTileList.Count)
                    break;
            }
            
        }
        //x값과 y값이 낮은 순으로 sort한다
        tileSnake.TileList.Sort(delegate(Tile a, Tile b) {

            if(a.Coordinate.y == b.Coordinate.y)
            {
                return a.Coordinate.x.CompareTo(b.Coordinate.x);
            }
            else
            {
                return a.Coordinate.y.CompareTo(b.Coordinate.y);
            }

	    });


        return tileSnake;
    }




    //private TileSnake Search(Tile std_tile)
    //{
    //    var tile = std_tile;
    //    TileSnake tileSnake = new TileSnake();
    //    while (tile.LinkedUpTileList.Count > 0)
    //    {
    //        int false_Count = 0;
    //        foreach (var child in tile.LinkedUpTileList)
    //        {
    //            //현재 이동이 불가능한 타일이거나 불가능한 지역이면 다음위치
    //            if (!child.Tile_MoveState())
    //            {
    //                false_Count++;
    //                continue;
    //            }

    //            if (child.bricktype != BrickType.Empty)
    //                tileSnake.Add_Tile(child);
    //            //3군데 검사해서 하나만 만족하면 된다.
    //            //만약에 비어있는 타일이라면 Add는 하지 않고 다음 검사하면 된다
    //            //3군데 모두 Add할 수 없으면 끝이다
    //            //구현은 아직 안되있지만 현재타일이 충전타일 아래 위치면 그때도 끝

    //            tile = child;
    //            break;

    //        }
    //        //세군데 모두 아이스같은 상태때문에 이동이 불가 할수도 있다
    //        if (false_Count == tile.LinkedUpTileList.Count)
    //            break;
    //    }
    //    return tileSnake;
    //    //m_TileSnakeController.Add_TileSnake(tileSnake);
    //}

    private void CreateTileSnake()
    {
        //tilesnke 생성
    }

    

}
