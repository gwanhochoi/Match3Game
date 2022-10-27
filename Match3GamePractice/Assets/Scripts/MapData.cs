using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class MapData //: ISerializationCallbackReceiver
{
    public int width_Count;
    public int height_Count;

    //public Dictionary<Vector2Int, string> map_dic;

    //public List<Vector2Int> _keys;
    //public List<string> _values;

    public List<CellData> m_cellData_List;


    public MapData(int width, int height)
    {
        width_Count = width;
        height_Count = height;

        m_cellData_List = new List<CellData>();

        //_keys = new List<Vector2Int>();
        //_values = new List<string>();

        //map_dic = new Dictionary<Vector2Int, string>();

        //for (int i = 0; i < height; i++)
        //{
        //    for (int j = 0; j < width; j++)
        //    {
        //        //map_dic[new Vector2Int(j, i)] = "none";
        //        m_cellData.Add(new CellData(j, i, "none"));
        //    }
        //}

    }

    public void Add_CellData(int x, int y, string name)
    {
        m_cellData_List.Add(new CellData(x, y, name));
    }

    public void ClearMapData()
    {
        m_cellData_List.Clear();
    }

    //public void OnBeforeSerialize()
    //{
    //    //throw new NotImplementedException();
    //    _keys.Clear();
    //    _values.Clear();

    //    foreach(var child in map_dic)
    //    {
    //        _keys.Add(child.Key);
    //        _values.Add(child.Value);
    //    }



    //}

    //public void OnAfterDeserialize()
    //{
    //    //throw new NotImplementedException();

    //    map_dic = new Dictionary<Vector2Int, string>();

    //    for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
    //        map_dic.Add(_keys[i], _values[i]);
    //}

    

    //public byte[] GetMapData()
    //{
    //    byte[] bytes = null;
    //    using (var ms = new MemoryStream())
    //    {
    //        using (var writer = new BinaryWriter(ms))
    //        {
    //            writer.Write(map_dic.Count);

    //            foreach (var data in map_dic)
    //            {
    //                writer.Write(data.Key.x);
    //                writer.Write(data.Key.y);
    //                writer.Write(data.Value);
    //            }

    //            bytes = ms.ToArray();
    //        }
    //    }

    //    return bytes;
    //}




}
