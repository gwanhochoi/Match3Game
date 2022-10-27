using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData
{
    public int x;
    public int y;
    public string name;


    public CellData(int x, int y, string name)
    {
        this.x = x;
        this.y = y;
        this.name = name;
    }
}
