using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile_Charge : Tile
{
    //public override void BrickMoveInTile(Action Func)
    //{
    //    //브릭을 새로 생성해서 이동시킨다
    //    //임시로 랜덤 브릭 생성 할까 아니면 브릭 풀만들까


        

    //    //BrickScript.Move(GraviyTile.transform.localPosition, Func);
    //    //GraviyTile.BrickScript = BrickScript;

    //}

    public void Charge_Brick()
    {
        BrickScript = BrickPool.Instance().Dequeue_RandomBrick();
        BrickScript.coordinate = Coordinate;
        BrickScript.transform.localPosition = transform.localPosition;
    }
}
