using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
public class MyUIRoot : MonoBehaviour
{
    static int mSizeFrame;
    static MethodInfo s_GetSizeOfMainGameView;
    static Vector2 mGameSize;
    static public Vector2 screenSize
    {
        get
        {
            int frame = Time.frameCount;

            if (mSizeFrame != frame || !Application.isPlaying)
            {
                mSizeFrame = frame;

                if (s_GetSizeOfMainGameView == null)
                {
                    System.Type type = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                    s_GetSizeOfMainGameView = type.GetMethod("GetSizeOfMainGameView",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                }
                mGameSize = (Vector2)s_GetSizeOfMainGameView.Invoke(null, null);
            }
            return mGameSize;
        }
    }

    static GameObject instance;
    static public GameObject Instance
    {
        get
        {
            if (instance == null)
            {
                string name = "UI Root";
                instance = GameObject.Find(name);
                if (instance == null)
                {
                    instance = new GameObject(name);
                    instance.AddComponent<MyUIRoot>();

                    GameObject camObj = new GameObject("Camera");
                    camObj.transform.parent = instance.transform;
                    Camera cam = camObj.AddComponent<Camera>();
                    cam.clearFlags = CameraClearFlags.Depth;
                    cam.cullingMask = 1 << LayerMask.NameToLayer("UI");
                    cam.orthographic = true;
                    cam.orthographicSize = 1;
                    cam.nearClipPlane = -10;
                    cam.farClipPlane = 10;
                    cam.depth = 1;
                }
            }
            return instance;
        }
    }

    GameObject _drawCallRoot;
    public GameObject drawCallRoot
    {
        get
        {
            if (_drawCallRoot == null)
            {
#if UNITY_EDITOR
                _drawCallRoot = UnityEditor.EditorUtility.CreateGameObjectWithHideFlags("_UI_" + transform.name, HideFlags.DontSave, new System.Type[0] { });
#else
                _drawCallRoot = new GameObject("_UI_" + transform.name);
#endif
                _drawCallRoot.transform.localScale = transform.localScale;
            }
            return _drawCallRoot;
        }
    }

    //private void OnEnable()
    //{
    //    if (drawCallRoot == null)
    //    {
    //        drawCallRoot = new GameObject("_UI_" + transform.name);
    //        drawCallRoot.transform.localScale = transform.localScale;
    //    }
    //}

    //private void OnDisable()
    //{
    //    DestroyImmediate(drawCallRoot);
    //}

    static public int activeHeight
    {
        get
        {
            Vector2 screen = screenSize;
            float aspect = screen.x / screen.y;
            float initialAspect = (float)manualWidth / manualHeight;
            return (initialAspect > aspect) ? Mathf.RoundToInt(manualWidth / aspect) : manualHeight;
        }
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        UpdateScale();

    }

    static int manualWidth = 1280;
    static int manualHeight = 720;
    void UpdateScale()
    {

        float size = 2f / activeHeight;

        transform.localScale = new Vector3(size, size, size);
    }
}
