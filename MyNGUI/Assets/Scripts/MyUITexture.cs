using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("MyNGUI/Texture")]
public class MyUITexture : MonoBehaviour
{
    public Texture texture;
    public Vector2 size = new Vector2(100,100);

    GameObject drawCall;
    bool needDestroy;

    // Use this for initialization
    void Start ()
    {
        SetTexture();
    }

    void Update()
    {
        if (needDestroy)
        {
            DestroyImmediate(drawCall);
            needDestroy = false;
        }
    }

    void CreateDrawCall(string name)
    {
#if UNITY_EDITOR
        drawCall = UnityEditor.EditorUtility.CreateGameObjectWithHideFlags("", HideFlags.DontSave, typeof(MyUIDrawCall));
#else
        drawCall = new GameObject();
        drawCall.AddComponent<MyUIDrawCall>();
#endif
        MyUIRoot root = transform.GetComponentInParent<MyUIRoot>();
        drawCall.transform.parent = root.drawCallRoot.transform;
        drawCall.transform.localScale = Vector3.one;

        drawCall.layer = LayerMask.NameToLayer("UI");

        drawCall.AddComponent<MeshFilter>();
        //drawCall.transform.parent = MyUIRoot.drawcallRoot.transform;


        MeshRenderer renderer = drawCall.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = new Material(Shader.Find("UI/Texture"));
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
    }

    void RefreshDrawCall()
    {
        drawCall.name = "_DrawCall [" + texture.name + "]";

        float x = size.x / 2;
        float y = size.y / 2;

        Vector3 leftBot = new Vector3(-x, -y, 0);
        Vector3 rightBot = new Vector3(x, -y, 0);
        Vector3 rightTop = new Vector3(x, y, 0);
        Vector3 leftTop = new Vector3(-x, y, 0);
        List<Vector3> vertexList = new List<Vector3>() { leftBot, rightBot, rightTop, leftTop };

        Vector3 leftBotUV = new Vector2(0, 0);
        Vector3 rightBotUV = new Vector2(1, 0);
        Vector3 rightTopUV = new Vector2(1, 1);
        Vector3 leftTopUV = new Vector2(0, 1);
        List<Vector2> uvList = new List<Vector2>() { leftBotUV, rightBotUV, rightTopUV, leftTopUV };

        int[] triangles = { 0, 2, 1, 0, 3, 2 };

        Mesh mesh = new Mesh();
        mesh.hideFlags = HideFlags.DontSave;
        mesh.name = "[MyNGUI] Mesh";
        mesh.vertices = vertexList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.triangles = triangles;

        drawCall.GetComponent<MeshFilter>().mesh = mesh;
        drawCall.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
    }

    void SetTexture()
    {
        if (texture == null)
            needDestroy = true;
        else
        {
            if (drawCall == null)
            {
                CreateDrawCall(texture.name);
            }
            RefreshDrawCall();
        }
    }

    private void OnEnable()
    {
        SetTexture();
    }

    private void OnDisable()
    {
        DestroyImmediate(drawCall);
    }

    private void OnDestroy()
    {
        DestroyImmediate(drawCall);
    }

    private void OnValidate()
    {
        SetTexture();
    }
}
