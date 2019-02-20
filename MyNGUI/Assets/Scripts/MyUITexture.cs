using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("MyNGUI/Texture")]
public class MyUITexture : MonoBehaviour
{
    public Texture texture;
    public Vector2 size;

    MeshRenderer renderer;
    GameObject drawCall;
    bool needDestroy;

    // Use this for initialization
    void Start ()
    {
        size = new Vector2(100, 100);
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
        drawCall = new GameObject();
        drawCall.layer = LayerMask.NameToLayer("UI");

        drawCall.AddComponent<MeshFilter>();

        renderer = drawCall.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = new Material(Shader.Find("UI/Texture"));
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
    }

    void RefreshDrawCall()
    {
        drawCall.name = "_DrawCall [" + texture.name + "]";

        float x = size.x / 720;
        float y = size.y / 720;

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
        if (drawCall != null)
            drawCall.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (drawCall != null)
            drawCall.gameObject.SetActive(false);
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
