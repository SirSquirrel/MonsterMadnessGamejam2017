using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a class to generate a random puzzle level
public class RandomStageGenerator : MonoBehaviour {
    //these variables determine the size in tiles for the random stage
    //they are set by the UI by the user and are thus public they should still not be accessed by non menu functions though
    //it will have certain bounds and cannot be less than 9x5 the sizes are small(9x5) medium(10x6) and large(11x7)
    public int stageSizeX = 9;
    public int stageSizeY = 5;

    //number of entrances people can come through this is determined by the user's choice of difficulty and size
    private int entranceNum = 3;

    //difficulty of random stage, this is again chosen through the menu by the user
    public Difficulty stageDifficulty;
    //decided to set this to 0 but left it in incase I want to go back
    private int hardExtraEntrances = 0;
    private int easyFewerEntrances = 1;
    private int easyPeoplePerDoor = 2;
    private int normalPeoplePerDoor = 3;
    private int hardPeoplePerDoor = 4;
    private float InitialTimeTillSpawn = 10;
    private float spawnDelayTime = 15;
    private float spawnDelayMinRandom = 0.9f;
    private float spawnDelayMaxRandom = 1.1f;


    //just needed for ensuring there is a path from the entrance, shows the direction the path is coming from, none means that this is the start of a path
    public enum directionFrom
    {
        left = 0 , top = 1, right = 2, bottom = 3, none = 4
    }

    //this int shows the minimum length for a path from each entrance since we don't want the people coming from an entrance to instantly cause a loss
    private int minPathFromEntrance = 4;

    //the starting "weight" for the random generator when selecting tiles
    private float startWeight = 2;
    //sum of all weights
    float weightSum = 0f;
    float[] weights;

    //Lists of various tilesets, they are set in the editor to avoid using the resources.load() method for this they have to be public
    //A list of all entrances
    public GameObject[] entranceTiles;
    //A list of all non-entrance tiles
    public GameObject[] regularTiles;
    //lists of tiles to use for entrance paths arranged by having a certain entrance
    public GameObject[] twoWayTopTiles;
    public GameObject[] twoWayLeftTiles;
    public GameObject[] twoWayRightTiles;
    public GameObject[] twoWayBottomTiles;

    //list of entrances that have been spawned used for populating them with people
    public Spawner[] entrancesSpawned;

    //this variable is the grid that will have all the tiles on it
    //I know 2D arrays are sometimes frowned upon but this is the best way to represent it because each index represents it's physical location on the grid
    //luckily our tiles are 1 unit large so the tileGrid position is there actual position
    private Tile[,] tileGrid;

	// This is an inbuilt unity function inherrited from MonoBehaviour and is called as soon as the scene is loaded
	void Start () {
        tileGrid = new Tile[stageSizeX,stageSizeY];

        //determine the number of entrances based off size and difficulty, size will be at least 9x5
        //will be rounded by integer division but that's fine 
        entranceNum = (stageSizeX * stageSizeY) / 15;
        if(stageDifficulty == Difficulty.Easy)
        {
            entranceNum = entranceNum - easyFewerEntrances;
        }
        else if(stageDifficulty == Difficulty.Hard)
        {
            entranceNum = entranceNum + hardExtraEntrances;
        }
        entrancesSpawned = new Spawner[entranceNum];

        //set the weights of all the tiles in the list since we don't want to spawn too many of one type
        weights = new float[regularTiles.Length];

        for (int i = 0; i < weights.Length; ++i)
        {
            weights[i] = startWeight;
            weightSum += startWeight;
        }

        GenerateStage();

	}

    private void GenerateStage()
    {
        //random numbers for utility, could get by with two but it's more readable this way
        int randX;
        int randY;
        int randFromList;
        
        //first we generate the entrances and make sure they don't open into a wall
        for(int i = 0 ; i < entranceNum; ++i)
        {
            //spawn a random entrance from the list at randX and randY co-ordinates if the space is occupied run the loop again
            randX = Random.Range(0,stageSizeX-1);
            randY = Random.Range(0, stageSizeY - 1);
            if (tileGrid[randX,randY] == null)
            {
                randFromList = Random.Range(0, entranceTiles.Length - 1);
                GameObject entranceSpawned = Instantiate(entranceTiles[randFromList], new Vector2(randX, randY), transform.rotation);
                tileGrid[randX, randY] = entranceSpawned.GetComponent<Tile>();

                PathFromEntrance(entranceSpawned.GetComponent<Tile>(), randX, randY);
                entrancesSpawned[i] = entranceSpawned.GetComponent<Spawner>();
            }
            else
            {
                --i;
            }
        }

        GameObject tileToSpawn;

        //populate the map with tiles
        for(int x = 0; x < stageSizeX;++x)
        {
            for(int y = 0; y < stageSizeY;++y)
            {
                if (tileGrid[x, y] == null)
                {
                    //get a random weighted tile for the grid numbers
                    tileToSpawn = Instantiate(randomTileFromWeight(), new Vector2(x, y), transform.rotation);
                    tileGrid[x, y] = tileToSpawn.GetComponent<Tile>();
                }
            }
        }

        //find neighbors for all tiles
        //this is neccessary for the pathing of the people
        for (int x = 0; x < stageSizeX; ++x)
        {
            for (int y = 0; y < stageSizeY; ++y)
            {
                if (tileGrid[x, y] == null)
                {
                    tileGrid[x, y].Find_Neighbours();
                }
            }
        }

        //determine people at each entrance
        PopulateEntrances();
    }

    //generates a path from a given entrance requires the entrance and it's tile grid location
    //this is to give players time to react when a person spawns
    private void PathFromEntrance(Tile Entrance, int tileGridX, int tileGridY)
    {
        int randX;
        int randY;
        //while the entrance is blocked keep moving it this should not run much
        while (!PathCheck(Entrance, tileGridX, tileGridY))
        {
            
            randX = Random.Range(0, stageSizeX - 1);
            randY = Random.Range(0, stageSizeY - 1);
            if (tileGrid[randX, randY] == null)
            {
                tileGrid[tileGridX, tileGridY] = null;
                Entrance.transform.position = new Vector2(randX, randY);
                tileGrid[randX, randY] = Entrance;
                tileGridX = randX;
                tileGridY = randY;
            }
        }

        //add the buffer tile in front of te entrance
        int randFromList;
        if (Entrance.bottom_exit)
        {
            randFromList = Random.Range(0, twoWayTopTiles.Length - 1);
            tileGrid[tileGridX,tileGridY - 1] = Instantiate(twoWayTopTiles[randFromList], new Vector2(tileGridX, tileGridY - 1), transform.rotation).GetComponent<Tile>();
        }
        else if (Entrance.top_exit)
        {
            randFromList = Random.Range(0, twoWayBottomTiles.Length - 1);
            tileGrid[tileGridX, tileGridY + 1] = Instantiate(twoWayBottomTiles[randFromList], new Vector2(tileGridX, tileGridY + 1), transform.rotation).GetComponent<Tile>();
        }
        else if (Entrance.left_exit)
        {
            randFromList = Random.Range(0, twoWayRightTiles.Length - 1);
            tileGrid[tileGridX - 1, tileGridY] = Instantiate(twoWayRightTiles[randFromList], new Vector2(tileGridX - 1, tileGridY ), transform.rotation).GetComponent<Tile>();
        }
        else if (Entrance.right_exit)
        {
            randFromList = Random.Range(0, twoWayLeftTiles.Length - 1);
            tileGrid[tileGridX + 1, tileGridY] = Instantiate(twoWayLeftTiles[randFromList], new Vector2(tileGridX + 1, tileGridY ), transform.rotation).GetComponent<Tile>();
        }
    }

    //this should only be called to guarentee a path from an entrance
    //returns true if the path will be able to continue otherwise returns false it needs to be provided with the tile and it's position on the grid
    private bool PathCheck(Tile tileToCheck, int tileGridX, int tileGridY)
    {
        //we check any of the openings to see that they don't open onto the edge of the map or another tile

        if ((tileToCheck.left_exit && tileGridX == 0)
            || (tileToCheck.left_exit && tileGrid[tileGridX - 1, tileGridY] != null))
        { return false; }
        else if ((tileToCheck.top_exit && tileGridY == stageSizeY - 1)
            || (tileToCheck.top_exit && tileGrid[tileGridX, tileGridY + 1] != null))
        { return false; }
        else if ((tileToCheck.right_exit && tileGridX == stageSizeX - 1)
            || (tileToCheck.right_exit && tileGrid[tileGridX + 1, tileGridY] != null))
        { return false; }
        else if ((tileToCheck.bottom_exit && tileGridY == 0)
            || (tileToCheck.bottom_exit && tileGrid[tileGridX, tileGridY - 1] != null))
        { return false; }
        else
        { return true; }
    }

    //randomely select a tile giving preference to tiles that have had fewer spawned
    private GameObject randomTileFromWeight()
    {
        //assign a value so the function knows it is returning something
        GameObject tileToPick = regularTiles[0];
        //select the randomTile
        float randPicker = Random.Range(0f,weightSum);
        float runningTotal = 0;
        bool tileSelected = false;
        int indexOfTile = 0;
        while(!tileSelected)
        {
            runningTotal += weights[indexOfTile];
            if (randPicker < runningTotal)
            {
                tileToPick = regularTiles[indexOfTile];
                tileSelected = true;
            }
            else
            {
                ++indexOfTile;
            }
        }

        //take weight from the tile picked and recalculate the sum
        weightSum = weightSum - weights[indexOfTile] / 2;
        weights[indexOfTile] = weights[indexOfTile] / 2;

        return tileToPick;
    }

    

    //populates the entrances on the map with people
    private void PopulateEntrances()
    {
        int peopleToSpawn;
        //set up each door with the correct number of people to spawn
        if(stageDifficulty == Difficulty.Easy)
        {
            peopleToSpawn = entranceNum * easyPeoplePerDoor;
            for(int i = 0; i < entrancesSpawned.Length;i++)
            {
                entrancesSpawned[i].people_to_spawn = easyPeoplePerDoor;
            }
        }
        if (stageDifficulty == Difficulty.Normal)
        {
            peopleToSpawn = entranceNum * normalPeoplePerDoor;
            for (int i = 0; i < entrancesSpawned.Length; i++)
            {
                entrancesSpawned[i].people_to_spawn = normalPeoplePerDoor;
            }
        }
        if (stageDifficulty == Difficulty.Hard)
        {
            peopleToSpawn = entranceNum * hardPeoplePerDoor;
            for (int i = 0; i < entrancesSpawned.Length; i++)
            {
                entrancesSpawned[i].people_to_spawn = hardPeoplePerDoor;
            }
        }

        //set up the initial and repeating spawn delays.
        //because easy has 2 people per door only 1 door will be active at a time, on normal and above multiple doors can be active
        float timeForNextDoor = InitialTimeTillSpawn;
        for (int i = 0; i < entrancesSpawned.Length; i++)
        {
            entrancesSpawned[i].initial_spawn_delay = timeForNextDoor;
            entrancesSpawned[i].spawn_delay = spawnDelayTime * Random.Range(spawnDelayMinRandom,spawnDelayMaxRandom);
            timeForNextDoor += spawnDelayTime * Random.Range(spawnDelayMinRandom, spawnDelayMaxRandom) * 2;
        }
    }

    
}
