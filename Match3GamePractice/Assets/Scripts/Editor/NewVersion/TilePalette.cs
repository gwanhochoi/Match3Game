using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPalette", menuName = "TileMap/TileMap Palette")]
public class TilePalette : ScriptableObject
{
    [SerializeField]
    private List<GameObject> prefabs_List = new List<GameObject>();
    public List<GameObject> m_prfabs_List
    {
        get
        {
            return prefabs_List;
        }

        set
        {
            prefabs_List = value;
        }
    }
}
