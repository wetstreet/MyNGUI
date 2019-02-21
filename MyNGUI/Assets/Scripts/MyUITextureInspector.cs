using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MyUITexture))]
[CanEditMultipleObjects]
public class MyUITextureInspector : Editor
{



    public override void OnInspectorGUI()
    {
        SerializedProperty spTexture = serializedObject.FindProperty("texture");
        EditorGUILayout.PropertyField(spTexture, new GUIContent("Texture"));
        SerializedProperty spSize = serializedObject.FindProperty("size");
        EditorGUILayout.PropertyField(spSize, new GUIContent("Size"));
        if (GUILayout.Button("Snap", GUILayout.Width(40)))
        {
            Texture tex = spTexture.objectReferenceValue as Texture;
            spSize.vector2Value = new Vector2(tex.width, tex.height);
        }
        if (GUILayout.Button("Full Screen", GUILayout.Width(80)))
        {
            Vector2 size = Handles.GetMainGameViewSize();
            float aspect = size.x / size.y;
            int height = MyUIRoot.activeHeight;
            Debug.Log("activeHeight=" + height);
            Debug.Log("aspect=" + aspect);
            int width = Mathf.RoundToInt(height * aspect);
            Debug.Log("width=" + width);
            spSize.vector2Value = new Vector2(width, height);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
