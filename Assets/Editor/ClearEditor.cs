using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerPrefsClear))]
public class ClearEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerPrefsClear t = (PlayerPrefsClear)target;

        if (GUILayout.Button("Clear"))
        {
            t.Clear();
        }
    }
}
