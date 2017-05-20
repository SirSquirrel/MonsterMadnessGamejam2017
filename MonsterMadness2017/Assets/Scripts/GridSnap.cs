using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// https://www.youtube.com/watch?v=ukYbRmmlaTM
[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{
    Vector3 offset = new Vector3(-1, 1, 0);

    public float cell_size = 1f;
    private float x, y, z;


    #if (UNITY_EDITOR)
    void Update()
    {
        if (!Application.isPlaying)
        {
            x = Mathf.Round(transform.position.x / cell_size) * cell_size;
            y = Mathf.Round(transform.position.y / cell_size) * cell_size;
            z = transform.position.z;
            transform.position = new Vector3(x, y, z);
        }
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(this.transform.position + offset, "" + (Vector2) this.transform.position);
    }
    #endif
}
