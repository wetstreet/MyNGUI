using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyUIMenu : MonoBehaviour {
    public static void SetParentAndSelect(GameObject obj)
    {
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
        obj.transform.localScale = Vector3.one;
        Selection.activeGameObject = obj;
    }

    [MenuItem("MyNGUI/Create Texture")]
    public static void CreateTexture()
    {
        GameObject obj = new GameObject("Texture");
        obj.layer = LayerMask.NameToLayer("UI");
        obj.AddComponent<MyUITexture>();

        SetParentAndSelect(obj);
    }


    [MenuItem("MyNGUI/Create Label")]
    public static void CreateLabel()
    {
        GameObject obj = new GameObject("Label");
        obj.layer = LayerMask.NameToLayer("UI");
        obj.AddComponent<MyUILabel>();

        SetParentAndSelect(obj);
    }
}
