using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyUIMenu : MonoBehaviour {
    [MenuItem("MyNGUI/Create Texture")]
    public static void CreateTexture()
    {
        GameObject obj = new GameObject("Texture");
        obj.layer = LayerMask.NameToLayer("UI");
        obj.AddComponent<MyUITexture>();
        Selection.activeGameObject = obj;
    }
}
