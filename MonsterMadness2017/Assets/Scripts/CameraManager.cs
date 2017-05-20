using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector2 target_pos;
    public float target_zoom;
    float time_zoom_changed;
    float prev_zoom;

    void Start ()
    {
        CalculateZoomLevel();
    }
    public void SetZoomLevel(float size)
    {
        prev_zoom = Camera.main.orthographicSize;
        time_zoom_changed = Time.time;
        target_zoom = size;
    }
    public void CalculateZoomLevel()
    {
        float max_width = 0;
        float max_height = 0;
        float min_width = 0;
        float min_height = 0;

        // Get our max dimensions
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject go in tiles)
        {
            if (go.transform.position.x >= max_width)
                max_width = go.transform.position.x;
            if (go.transform.position.y >= max_height)
                max_height = go.transform.position.y;
            if (go.transform.position.x <= min_width)
                min_width = go.transform.position.x;
            if (go.transform.position.y <= min_height)
                min_height = go.transform.position.y;
        }

        SetZoomLevel(GetOrphographicZoomToFit(min_width, min_height, max_width, max_height));
    }
    public float GetOrphographicZoomToFit(float min_x, float min_y, float max_x, float max_y)
    {
        // Determine largest X/Y constraint
        float x_constraint = Mathf.Max(Mathf.Abs(min_x), Mathf.Abs(max_x)) + 0.5f;
        float y_constraint = Mathf.Max(Mathf.Abs(min_y), Mathf.Abs(max_y)) + 0.5f;
        Debug.Log(min_x + " " + min_y + ":" + max_x + " " + max_y);
        // Figure out if we should worry about width or height
        float y_zoom = y_constraint / 2;
        float x_zoom = x_constraint / Screen.width * Screen.height;
        //x_zoom /= 2;
        y_zoom *= 2;
        //y_zoom = y_constraint + 0.5f;

        // Reference equations we must solve for
        //float screenHeightInUnits = Camera.main.orthographicSize * 2;
        //float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height; // basically height * screen aspect ratio
        // Add 1 because we have 0.5 extra space on each side
        float needed_zoom = Mathf.Max(x_zoom, y_zoom);
        Debug.Log(x_zoom + ":" + y_zoom + ", " + needed_zoom);

        return needed_zoom;
    }


    void Update ()
    {
        // Zooming
		if (Camera.main.orthographicSize != target_zoom)
        {
            Camera.main.orthographicSize =  Mathf.Lerp(prev_zoom, target_zoom, Time.time - time_zoom_changed);
        }
        // Panning
        if (target_pos != null && (Vector2) Camera.main.transform.position != target_pos)
        {
            Vector3 v = Vector2.Lerp(Camera.main.transform.position, target_pos, 1.0f * Time.deltaTime);
            v.z = -10f;
            Camera.main.transform.position = v;
        }
	}
}
