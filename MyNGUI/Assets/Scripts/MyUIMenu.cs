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

        GameObject selectedObj = Selection.activeGameObject;
        if (selectedObj == null)
        {
            obj.transform.parent = MyUIRoot.Instance.transform;
        }
        else
        {
            if (selectedObj.transform.IsChildOf(MyUIRoot.Instance.transform))
            {
                obj.transform.parent = selectedObj.transform;
            }
            else
            {
                obj.transform.parent = MyUIRoot.Instance.transform;
            }
        }
        Selection.activeGameObject = obj;
    }
}
