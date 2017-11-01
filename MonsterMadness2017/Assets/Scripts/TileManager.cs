using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

    public static TileManager tileManager;
    public GameObject[] tileList;
    public Tile curTileSelected;
    public bool hasTileSelected = false;
    public GameObject highlight;
    public GameObject tempHighlight;
    public AudioSource[] Sounds;
    public Text swapCounter;

    //swaps done
    public int swapsDone = 0;

    //these variables are only used for challenge levels where the player has a limited number of moves
    public bool limitedMoves = false;
    public int tileMovesAllowed = 10;
    public Text textMovesLeft;

    //used for the raycast
    private float tileSize = 1f;

    // Use this for initialization
    void Start () {
        StartCoroutine(LateStart());
        
    }

    //just make sure the level has loaded before getting the tile list
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        //wait a little for the stage to load
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
        Debug.Log(tileSize);

    }

    // Update is called once per frame
    void Update () {
        if(swapCounter != null)
        {
            swapCounter.text = "moves: " + swapsDone.ToString();
        }

        if (tileList.Length > 0)
        {
            //two different control schemes one for PC and web, one for IOS and android
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            if (Input.GetMouseButtonDown(1) && TileManager.tileManager.hasTileSelected)
            {
                Deselect();
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

            LayerMask maskForTiles = LayerMask.GetMask("Tile");
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
                        //use this if you want something at the start of a touch
                    }

                    if (myTouch.phase == TouchPhase.Ended && !Pause.pause.paused)
                    {
                        //do a raycast to see where the touch is
                        // the setting has been changed so raycasts ignore the colliders they're in so we need to spawn the raycast outside the tile
                        RaycastHit2D tileHit = Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(myTouch.position) - new Vector2(0,tileSize), Vector2.up, tileSize, maskForTiles);

                        if (tileHit)
                        {
                            if (tileHit.transform.gameObject.GetComponent<Tile>().can_be_swapped)
                            {
                                curTileSelected = tileHit.transform.gameObject.GetComponent<Tile>();
                                hasTileSelected = true;
                                //Destroy(tempHighlight);
                                highlight.transform.position = curTileSelected.transform.position;
                                highlight.SetActive(true);
                            }
                            else
                            {
                                Instantiate(Resources.Load("XOutlineDiesAtTime"), tileHit.transform.position, tileHit.transform.rotation);
                            }
                        }
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
                        //do a raycast to see where the touch is
                        // the setting has been changed so raycasts ignore the colliders they're in so we need to spawn the raycast outside the tile
                        RaycastHit2D tileHit = Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(myTouch.position) - new Vector2(0, 1), Vector2.up, 2, maskForTiles);

                        if (tileHit)
                        {
                            if (tileHit.transform.gameObject.GetComponent<Tile>().can_be_swapped)
                            {
                                tileHit.transform.gameObject.GetComponent<Tile>().Swap();
                                hasTileSelected = false;
                                highlight.SetActive(false);
                            }

                            else
                            {
                                Instantiate(Resources.Load("XOutlineDiesAtTime"), tileHit.transform.position, tileHit.transform.rotation);
                            }
                        }
                    }
                }
            }
#endif
        }
    }

    public void PlaySwapSound()
    {
        int rand = Random.Range(0,Sounds.Length);
        Sounds[rand].Play();
    }

    //if the player has currently selected a tile deselect it
    public void Deselect()
    {
        if (curTileSelected != null)
        {
            Instantiate(Resources.Load("XOutlineDiesAtTime"), curTileSelected.transform.position, curTileSelected.transform.rotation);
            TileManager.tileManager.hasTileSelected = false;
            TileManager.tileManager.curTileSelected = null;
            highlight.SetActive(false);
        }
    }
}
