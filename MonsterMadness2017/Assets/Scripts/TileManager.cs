using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

    public static TileManager tileManager;
    public Tile curTileSelected;
    public bool hasTileSelected;
    public GameObject highlight;
    public AudioSource[] Sounds;

    //these variables are only used for challenge levels where the player has a limited number of moves
    public bool limitedMoves = false;
    public int tileMovesAllowed = 10;
    public Text textMovesLeft;

    // Use this for initialization
    void Start () {
        tileManager = this;
        if (limitedMoves)
        {
            textMovesLeft.gameObject.SetActive(true);
        }
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
        if (limitedMoves)
        {
            textMovesLeft.text = tileMovesAllowed.ToString();
        }

        
    }

    public void PlaySwapSound()
    {
        int rand = Random.Range(0,Sounds.Length);
        Sounds[rand].Play();
    }

}
