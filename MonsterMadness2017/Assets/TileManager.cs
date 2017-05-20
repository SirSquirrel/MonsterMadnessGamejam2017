using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public static TileManager tileManager;
    public Tile curTileSelected;
    public bool hasTileSelected;
    public GameObject highlight;

	// Use this for initialization
	void Start () {
        tileManager = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && !TileManager.tileManager.hasTileSelected)
        {
            Debug.Log("Deselected");
            TileManager.tileManager.hasTileSelected = false;
            TileManager.tileManager.curTileSelected = null;
            if (highlight!=null)
            {
                Destroy(highlight);
            }
        }
    }
}
