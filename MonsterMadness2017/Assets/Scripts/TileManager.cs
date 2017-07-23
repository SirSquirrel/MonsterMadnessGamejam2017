using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

    public static TileManager tileManager;
    public GameObject[] tileList;
    public Tile curTileSelected;
    public bool hasTileSelected;
    public GameObject highlight;
    public GameObject tempHighlight;
    public AudioSource[] Sounds;

    //these variables are only used for challenge levels where the player has a limited number of moves
    public bool limitedMoves = false;
    public int tileMovesAllowed = 10;
    public Text textMovesLeft;

    //rough size of the tile, should actually be square root of 2 but we want a slight buffer zone
    public float tileSize = 1.5f;

    // Use this for initialization
    void Start () {
        tileManager = this;
        if (limitedMoves)
        {
            textMovesLeft.gameObject.SetActive(true);
        }
        tileList = GameObject.FindGameObjectsWithTag("Tile");
        highlight = (GameObject)Instantiate(Resources.Load("Highlight"));
        highlight.SetActive(false);
        tempHighlight = (GameObject)Instantiate(Resources.Load("TemporaryHighlight"));
        tempHighlight.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //two different control schemes one for PC and web, one for IOS and android
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (Input.GetMouseButtonDown(1) && TileManager.tileManager.hasTileSelected)
        {
            Debug.Log("Deselected");
            TileManager.tileManager.hasTileSelected = false;
            TileManager.tileManager.curTileSelected = null;
            highlight.SetActive(false);
        }

        for (int i = 0; i < GameState.game_state.Victims.Count; i++)
        {
            if (GameState.game_state.Victims[i].GetComponent<Person>().cur_tile == curTileSelected)
            {
                Instantiate(Resources.Load("XOutlineDiesAtTime"), TileManager.tileManager.curTileSelected.transform.position, TileManager.tileManager.curTileSelected.transform.rotation);
                TileManager.tileManager.hasTileSelected = false;
                TileManager.tileManager.curTileSelected = null;
                highlight.SetActive(false);
            }
        }
        if (limitedMoves)
        {
            textMovesLeft.text = tileMovesAllowed.ToString();
        }
#endif

        //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone  
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        GameObject closestTile = tileList[0];
        //just a placeholder to become the magnitude of a vector subtraction
        float curDistance;
        float distanceToClosestTile = float.PositiveInfinity;
        //Check if Input has registered more than zero touches
        for (int i = 0; i < GameState.game_state.Victims.Count; i++)
        {
            if (GameState.game_state.Victims[i].GetComponent<Person>().cur_tile == curTileSelected)
            {
                Instantiate(Resources.Load("XOutlineDiesAtTime"), TileManager.tileManager.curTileSelected.transform.position, TileManager.tileManager.curTileSelected.transform.rotation);
                TileManager.tileManager.hasTileSelected = false;
                TileManager.tileManager.curTileSelected = null;
                highlight.SetActive(false);
            }
        }
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if (!hasTileSelected)
            {
                if (myTouch.phase == TouchPhase.Began || myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary && !hasTileSelected)
                {
                    // decided not to highlight touched tile

                    //for (int i = 0; i < tileList.Length; i++)
                    //{
                    //    curDistance = (Camera.main.ScreenToWorldPoint(myTouch.position) - tileList[i].transform.position).magnitude;
                    //    if (curDistance < distanceToClosestTile)
                    //    {
                    //        distanceToClosestTile = curDistance;
                    //        closestTile = tileList[i];
                    //    }
                    //}

                    ////if (distanceToClosestTile < tileSize)
                    ////{
                    //    closestTile.GetComponent<Tile>().highlightTouch();
                    ////}
                }

                if (myTouch.phase == TouchPhase.Ended && !Pause.pause.paused)
                {
                    for (int i = 0; i < tileList.Length; i++)
                    {
                        curDistance = (Camera.main.ScreenToWorldPoint(myTouch.position) - tileList[i].transform.position).magnitude;
                        if (curDistance < distanceToClosestTile)
                        {
                            distanceToClosestTile = curDistance;
                            closestTile = tileList[i];
                        }
                    }

                    //if (distanceToClosestTile < tileSize)
                    //{
                        curTileSelected = closestTile.GetComponent<Tile>();
                        hasTileSelected = true;
                        //Destroy(tempHighlight);
                        highlight.transform.position = curTileSelected.transform.position;
                    highlight.SetActive(true);
                    //}
                }
            }

            else
            {
                if (myTouch.phase == TouchPhase.Began || myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary && !hasTileSelected)
                {
                    //commented out fo reasons above

                    //for (int i = 0; i < tileList.Length; i++)
                    //{
                    //    curDistance = (Camera.main.ScreenToWorldPoint(myTouch.position) - tileList[i].transform.position).magnitude;
                    //    if (curDistance < distanceToClosestTile)
                    //    {
                    //        distanceToClosestTile = curDistance;
                    //        closestTile = tileList[i];
                    //    }
                    //}

                    ////if (distanceToClosestTile < tileSize)
                    ////{
                    //closestTile.GetComponent<Tile>().highlightTouch();
                    ////}
                }

                if (myTouch.phase == TouchPhase.Ended && !Pause.pause.paused)
                {
                    for (int i = 0; i < tileList.Length; i++)
                    {
                        curDistance = (Camera.main.ScreenToWorldPoint(myTouch.position) - tileList[i].transform.position).magnitude;
                        if (curDistance < distanceToClosestTile)
                        {
                            distanceToClosestTile = curDistance;
                            closestTile = tileList[i];
                        }
                    }

                    //if (distanceToClosestTile < tileSize)
                    //{
                    closestTile.GetComponent<Tile>().Swap();
                    hasTileSelected = false;
                    curTileSelected = null;
                    highlight.SetActive(false);
                    //}
                }
            }
        }
#endif

    }

    public void PlaySwapSound()
    {
        int rand = Random.Range(0,Sounds.Length);
        Sounds[rand].Play();
    }

}
