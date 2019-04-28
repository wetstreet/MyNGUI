using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyUILabelInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty spTexture = serializedObject.FindProperty("text");
        EditorGUILayout.PropertyField(spTexture, new GUIContent("Text"));
        SerializedProperty spDepth = serializedObject.FindProperty("depth");
        EditorGUILayout.PropertyField(spDepth, new GUIContent("Depth"));
        serializedObject.ApplyModifiedProperties();
    }
}
