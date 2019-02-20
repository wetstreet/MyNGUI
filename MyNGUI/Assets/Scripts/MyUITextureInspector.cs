using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyUITexture))]
[CanEditMultipleObjects]
public class MyUITextureInspector : Editor
{



    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("texture"), new GUIContent("Texture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"), new GUIContent("Size"));
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("Snap", GUILayout.Width(40)))
        {
            Debug.Log(Screen.width);
            Debug.Log(Screen.height);
        }
    }
}
