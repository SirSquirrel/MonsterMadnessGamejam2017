using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector2 target_pos;
    public float target_zoom;

    void Start ()
    {
        CalculateZoomLevel();
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
                max_height = go.transform.position.x;
            if (go.transform.position.x >= max_width)
                min_width = go.transform.position.x;
            if (go.transform.position.y >= max_height)
                max_height = go.transform.position.x;
        }

        float screenHeightInUnits = Camera.main.orthographicSize * 2;
        float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height; // basically height * screen aspect ratio


        Camera.main.orthographicSize = Mathf.Max(screenHeightInUnits, screenWidthInUnits);
    }
    public float OrthographicallyFitThese(float min_x, float min_y, float max_x, float max_y)
    {
        return 0f;
    }

    void Update ()
    {
		
	}
}
