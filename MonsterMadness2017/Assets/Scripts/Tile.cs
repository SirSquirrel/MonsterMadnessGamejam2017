using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Tile> neighbours = new List<Tile>();

    // Set to true if this tile has a
    public bool left_exit = false;
    public bool right_exit = false;
    public bool top_exit = false;
    public bool bottom_exit = false;

    public int entrances = 0;

    public Tile left_neighbour;
    public Tile right_neighbour;
    public Tile top_neighbour;
    public Tile bottom_neighbour;

    // Based on this tile's exits, and the neighbours exits, here where a person can go
    public bool can_go_left = false;
    public bool can_go_right = false;
    public bool can_go_top = false;
    public bool can_go_bottom = false;

    public int exits = 0;

    public bool man_on_tile = false;
    public int man_on_tile_count = 0;

    //whether the tile is currently being selected to swap
    public bool selected = false;

    public GameObject temporary_highlight;

    //whether the tile can be swapped, set to false for entrances and exits
    public bool can_be_swapped = true;

    void Start()
    {
        Find_Neighbours();

        if (top_exit)
            entrances++;
        if (bottom_exit)
            entrances++;
        if (left_exit)
            entrances++;
        if (right_exit)
            entrances++;
    }

    void OnMouseOver()
    {
        if (!TileManager.tileManager.hasTileSelected && temporary_highlight == null && !man_on_tile && can_be_swapped)
        {

            temporary_highlight = (GameObject)Instantiate(Resources.Load("TemporaryHighlight"), transform.position, transform.rotation);
        }

        if (can_be_swapped)
        {
            if (Input.GetMouseButtonDown(0) && !TileManager.tileManager.hasTileSelected && !man_on_tile && can_be_swapped)
            {
                Debug.Log("Selected");
                TileManager.tileManager.hasTileSelected = true;
                TileManager.tileManager.curTileSelected = this;
                Destroy(temporary_highlight);
                TileManager.tileManager.highlight = (GameObject)Instantiate(Resources.Load("Highlight"), transform.position, transform.rotation);
            }
            else if (Input.GetMouseButtonDown(0) && TileManager.tileManager.hasTileSelected && can_be_swapped)
            {
                Swap();
                TileManager.tileManager.hasTileSelected = false;
                TileManager.tileManager.curTileSelected = null;
                Destroy(TileManager.tileManager.highlight);
            }
        }
    }

    void OnMouseExit()
    {
        Destroy(temporary_highlight);
    }

    void Update()
    {

    }


    public void Find_Neighbours()
    {
        // Raycast left
        left_neighbour = RaycastNeighbour(Vector2.left);
        // Raycast right
        right_neighbour = RaycastNeighbour(Vector2.right);
        // Raycast up
        top_neighbour = RaycastNeighbour(Vector2.up);
        // Raycast down
        bottom_neighbour = RaycastNeighbour(Vector2.down);

        CalculateExits();
    }
    public Tile RaycastNeighbour(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 1f);

        if (hit.transform != null && hit.transform.gameObject.GetComponent<Tile>() != null)
            return hit.transform.gameObject.GetComponent<Tile>();
        else
            return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Person")
        {
            man_on_tile = true;
            man_on_tile_count = man_on_tile_count + 1;
            if(TileManager.tileManager.curTileSelected == this)
            {
                TileManager.tileManager.hasTileSelected = false;
                TileManager.tileManager.curTileSelected = null;
                Destroy(TileManager.tileManager.highlight);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Person")
        {
            man_on_tile_count = man_on_tile_count - 1;
            if(man_on_tile_count == 0)
            {
                man_on_tile = false;
            }
        }
    }


    public void CalculateExits()
    {
        exits = 0;
        can_go_left = left_neighbour != null && left_exit && left_neighbour.right_exit ? true : false;
        if (can_go_left)
        {
            exits = exits + 1;
        }
        can_go_right = right_neighbour != null && right_exit && right_neighbour.left_exit ? true : false;
        if (can_go_right)
        {
            exits = exits + 1;
        }
        can_go_top = top_neighbour != null && top_exit && top_neighbour.bottom_exit ? true : false;
        if (can_go_top)
        {
            exits = exits + 1;
        }
        can_go_bottom = bottom_neighbour != null && bottom_exit && bottom_neighbour.top_exit ? true : false;
        if (can_go_bottom)
        {
            exits = exits + 1;
        }
    }

    public void Swap()
    {
        Tile tile_to_swap = TileManager.tileManager.curTileSelected;

        Vector3 cur_position = transform.position;
        transform.position = TileManager.tileManager.curTileSelected.transform.position;
        tile_to_swap.transform.position = cur_position;
        Find_Neighbours();

        //have all neighbours re-evaluate as well
        if (left_neighbour != null)
            left_neighbour.GetComponent<Tile>().Find_Neighbours();

        if (right_neighbour != null)
            right_neighbour.GetComponent<Tile>().Find_Neighbours();

        if (top_neighbour != null)
            top_neighbour.GetComponent<Tile>().Find_Neighbours();

        if (bottom_neighbour != null)
            bottom_neighbour.GetComponent<Tile>().Find_Neighbours();

        
        tile_to_swap.Find_Neighbours();

        if (tile_to_swap.left_neighbour != null)
            tile_to_swap.left_neighbour.GetComponent<Tile>().Find_Neighbours();

        if (tile_to_swap.right_neighbour != null)
            tile_to_swap.right_neighbour.GetComponent<Tile>().Find_Neighbours();

        if (tile_to_swap.top_neighbour != null)
            tile_to_swap.top_neighbour.GetComponent<Tile>().Find_Neighbours();

        if (tile_to_swap.bottom_neighbour != null)
            tile_to_swap.bottom_neighbour.GetComponent<Tile>().Find_Neighbours();

        Debug.Log("swap");
    }
}
