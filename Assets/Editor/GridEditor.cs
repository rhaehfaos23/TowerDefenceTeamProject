using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;

        if (GUILayout.Button("Create"))
        {
            gridManager.CreateGrid();
        }

        if (GUILayout.Button("Save"))
        {
            gridManager.SaveTileMapData();
        }
    }
}
