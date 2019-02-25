using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("MyNGUI/Texture")]
public class MyUITexture : MonoBehaviour
{
    public Texture texture;
    public Vector2 size = new Vector2(100,100);
    public int depth;

    GameObject drawCall;
    bool needDestroy;
    Mesh mesh;
    Material material;

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

        // update transform in editor mode
        if (!Application.isPlaying)
        {
            RefreshDrawCall();
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
        
        MeshRenderer renderer = drawCall.AddComponent<MeshRenderer>();
        material = new Material(Shader.Find("UI/Texture"));
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
    }

    void RefreshDrawCall()
    {
        if (texture == null)
            return;

        if (drawCall == null)
            CreateDrawCall(texture.name);

        drawCall.name = "_DrawCall [" + texture.name + "]";

        float x = size.x / 2;
        float y = size.y / 2;

        Vector3 leftBot = new Vector3(-x, -y, 0);
        Vector3 rightBot = new Vector3(x, -y, 0);
        Vector3 rightTop = new Vector3(x, y, 0);
        Vector3 leftTop = new Vector3(-x, y, 0);
        List<Vector3> vertexList = new List<Vector3>() { leftBot, rightBot, rightTop, leftTop };

        // relative to parent vertices
        List<Vector3> rtpVerts = new List<Vector3>();
        
        Matrix4x4 mat = transform.parent.worldToLocalMatrix * transform.localToWorldMatrix;
        foreach (Vector3 v in vertexList)
        {
            rtpVerts.Add(mat.MultiplyPoint3x4(v));
        }

        Vector3 leftBotUV = new Vector2(0, 0);
        Vector3 rightBotUV = new Vector2(1, 0);
        Vector3 rightTopUV = new Vector2(1, 1);
        Vector3 leftTopUV = new Vector2(0, 1);
        List<Vector2> uvList = new List<Vector2>() { leftBotUV, rightBotUV, rightTopUV, leftTopUV };

        int[] triangles = { 0, 2, 1, 0, 3, 2 };

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.hideFlags = HideFlags.DontSave;
            mesh.name = "[MyNGUI] Mesh";
        }
        mesh.vertices = rtpVerts.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.triangles = triangles;
        
        drawCall.GetComponent<MeshFilter>().mesh = mesh;
        drawCall.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;

        material.renderQueue = 3000 + depth;
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 center = Vector3.zero;
        Vector3 _size = new Vector3(size.x, size.y, 1f);

        // Draw the gizmo
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = (UnityEditor.Selection.activeGameObject == gameObject) ? Color.white : Color.gray;
        Gizmos.DrawWireCube(center, _size);

        // Make the widget selectable
        _size.z = 0.01f;
        Gizmos.color = Color.clear;
        Gizmos.DrawCube(center, size);
    }
    
#endif

}
