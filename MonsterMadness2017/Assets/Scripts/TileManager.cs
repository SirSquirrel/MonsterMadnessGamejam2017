using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public static TileManager tileManager;
    public Tile curTileSelected;
    public bool hasTileSelected;
    public GameObject highlight;
    public AudioSource[] Sounds;

	// Use this for initialization
	void Start () {
        tileManager = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && TileManager.tileManager.hasTileSelected)
        {
            Debug.Log("Deselected");
            TileManager.tileManager.hasTileSelected = false;
            TileManager.tileManager.curTileSelected = null;
            if (highlight!=null)
            {
                Destroy(highlight);
            }
        }

        for (int i = 0; i < GameState.game_state.Victims.Count; i++)
        {
            if (GameState.game_state.Victims[i].GetComponent<Person>().cur_tile == curTileSelected)
            {
                Instantiate(Resources.Load("XOutlineDiesAtTime"), TileManager.tileManager.curTileSelected.transform.position, TileManager.tileManager.curTileSelected.transform.rotation);
                TileManager.tileManager.hasTileSelected = false;
                TileManager.tileManager.curTileSelected = null;
                if (highlight != null)
                {
                    Destroy(highlight);
                   
                }
            }
        }
    }

    public void PlaySwapSound()
    {
        int rand = Random.Range(0,Sounds.Length);
        Sounds[rand].Play();
    }

}
