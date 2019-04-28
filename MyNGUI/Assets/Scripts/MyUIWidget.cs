using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUIWidget : MonoBehaviour {

    public Vector2 size = new Vector2(100, 100);
    public int depth;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
