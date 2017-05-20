using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public static TileManager tileManager;
    public Tile curTileSelected;
    public bool hasTileSelected; 

	// Use this for initialization
	void Start () {
        tileManager = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
