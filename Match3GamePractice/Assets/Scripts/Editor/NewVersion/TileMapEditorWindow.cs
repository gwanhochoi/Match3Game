using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using UnityEditor.ShortcutManagement;
using System.IO;



public class TileMapEditorWindow : EditorWindow
{

    private int selected_Tool = -1;
    private TilePalette m_TilePalette;

    private Vector2 m_scrollPosition;
    private int m_selectedTile = -1;




    [MenuItem("MyTool/TileMapEditor")]
    public static void OpenTileMapEditor()
    {
        var wnd = GetWindow<TileMapEditorWindow>();
        wnd.titleContent = new GUIContent("TileMapEditor");
        wnd.minSize = new Vector2(400, 400);

    }


    //gui event handling and redering

    private void OnGUI()
    {


        EditorGUI.BeginChangeCheck();


        //map size
        using (new GUILayout.HorizontalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField("WidthCount", GUILayout.Width(position.width / 4));
            EditorData.WidthCount = EditorGUILayout.IntField(EditorData.WidthCount, GUILayout.Width(position.width / 4 - 10));

            EditorGUILayout.LabelField("HeightCount", GUILayout.Width(position.width / 4));
            EditorData.HeightCount = EditorGUILayout.IntField(EditorData.HeightCount, GUILayout.Width(position.width / 4 - 10));

        }

        using (new GUILayout.HorizontalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField("CellWidth", GUILayout.Width(position.width / 4));
            EditorData.CellWidth = EditorGUILayout.IntField(EditorData.CellWidth, GUILayout.Width(position.width / 4 - 10));

            EditorGUILayout.LabelField("CellHeight", GUILayout.Width(position.width / 4));
            EditorData.CellHeight = EditorGUILayout.IntField(EditorData.CellHeight, GUILayout.Width(position.width / 4 - 10));

        }


        //create map button

        if (GUILayout.Button("CreateMap"))
        {
            //create map
            CreateMapObject();

        }

        //temp save button
        if (GUILayout.Button("SaveMap"))
        {
            //create map
            SaveMapData();

        }

        using (new GUILayout.HorizontalScope(GUI.skin.box))
        {
            //toolbar
            //GUIContent(text, tooltip)
            GUIContent[] GUI_Contents = { new GUIContent("default"),
            new GUIContent("Pencil","draw tile mode"), new GUIContent("Erase")};
            selected_Tool = GUILayout.Toolbar(selected_Tool, GUI_Contents);
        }

        EditorGUILayout.Space();

        //Tile Prefab
        //var default_palette = AssetDatabase.LoadAssetAtPath<TilePalette>("Assets/Palette/NewPalette.asset");
        m_TilePalette = EditorGUILayout.ObjectField("TilePalette", m_TilePalette, typeof(TilePalette), false) as TilePalette;

        if (m_TilePalette != null)
        {
            //tile prefab list draw
            if (m_TilePalette.m_prfabs_List.Count > 0)
            {
                //scrollview & grid

                Texture[] prefab_textures = m_TilePalette.m_prfabs_List.Select(pv => AssetPreview.GetAssetPreview(pv)).ToArray();

                using (EditorGUILayout.ScrollViewScope scrollView = new EditorGUILayout.ScrollViewScope(m_scrollPosition))
                {
                    m_scrollPosition = scrollView.scrollPosition;

                    //GUILayout.SelectionGrid(????????? ??????, ????????? ?????????, ????????? ????????? ??????, ???????????? width?????????, ???????????? height ?????????)
                    //width, height??? ????????? ????????? ?????? ????????? ?????????.
                    m_selectedTile = GUILayout.SelectionGrid(m_selectedTile, prefab_textures, 4, GUILayout.Width(position.width - 20),
                        GUILayout.Height((position.width - 20) / 4 * Mathf.CeilToInt((float)prefab_textures.Length / 4)));

                }
            }

        }

        

        // End the code block and update gui change occurred
        if (EditorGUI.EndChangeCheck())
        {
            //????????? ?????? ?????? ????????? ??????
            if (m_selectedTile >= 0)
            {
                EditorData.Selected_Tile = m_TilePalette.m_prfabs_List[m_selectedTile];
            }

            EditorData.SelectedTool = (PaintTool)selected_Tool;



        }
    }

    

    private void SaveMapData()
    {
        //Debug.Log(Application.dataPath + "/MapJsonData/mapdata.json");

        TileMap[] tileMaps = FindObjectsOfType(typeof(TileMap)) as TileMap[];

        int index = 0;
        foreach(var child in tileMaps)
        {

            string data = JsonUtility.ToJson(child.Get_Mapdata(), true);
            File.WriteAllText(Application.dataPath + "/Resources/MapJsonData/" + "mapdata_ " +index + ".json", data);
        }

        

        //var path = EditorUtility.SaveFilePanel("MapData Save", Application.dataPath + "/MapJsonData", "MapData.bin", "bin");
        //if (string.IsNullOrEmpty(path) == false)
        //{
        //    byte[] bytes = mapData.GetMapData();
        //    File.WriteAllBytes(path, bytes);

            
        //}

        

    }

    private void LoadMapData()
    {
        

    }

    private void CreateMapObject()
    {

        if (EditorData.WidthCount <= 0 || EditorData.HeightCount <= 0 ||
            EditorData.WidthCount > 100 || EditorData.HeightCount > 100)
            return;

        if (EditorData.CellWidth < 1 || EditorData.CellHeight < 1)
            return;

        GameObject obj = new GameObject("TileMap");
        obj.AddComponent(typeof(TileMap));

        //var mapData = new MapData(EditorData.WidthCount, EditorData.HeightCount);
    }

    private void OnDestroy()
    {
        EditorData.Selected_Tile = null;

        //tilemap object ????????? ?????????????????? ????????? ?????????
        TileMap[] tileMaps = FindObjectsOfType(typeof(TileMap)) as TileMap[];

        foreach(var chlid in tileMaps)
        {
            chlid.Reset();
        }
    }
}
