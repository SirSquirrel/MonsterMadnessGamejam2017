﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //whether the tile is currently being selected to swap
    public bool selected = false;

    public GameObject temporary_highlight;
    public GameObject x_outline;

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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
    void OnMouseOver()
    {
        bool man_on = false;
        for (int i = 0; i < GameState.game_state.Victims.Count; i++)
        {
            if(GameState.game_state.Victims[i].GetComponent<Person>().cur_tile == this)
            {
                man_on = true;
                if (temporary_highlight != null)
                {
                    Destroy(temporary_highlight);
                }
            }
        }

        if((man_on || !can_be_swapped) && x_outline == null)
        {
            x_outline = (GameObject)Instantiate(Resources.Load("XOutline"), transform.position, transform.rotation);
        }

        if (!man_on)
        {
            if (temporary_highlight == null && can_be_swapped)
            {

                temporary_highlight = (GameObject)Instantiate(Resources.Load("TemporaryHighlight"), transform.position, transform.rotation);
            }

            if (can_be_swapped)
            {
                if (x_outline != null)
                {
                    Destroy(x_outline);
                }
                if (Input.GetMouseButtonDown(0) && !TileManager.tileManager.hasTileSelected && !Pause.pause.paused)
                {
                    TileManager.tileManager.hasTileSelected = true;
                    TileManager.tileManager.curTileSelected = this;
                    Destroy(temporary_highlight);
                    TileManager.tileManager.highlight.transform.position = transform.position;
                    TileManager.tileManager.highlight.SetActive(true);
                }
                else if (Input.GetMouseButtonDown(0) && TileManager.tileManager.hasTileSelected && !Pause.pause.paused)
                {
                    Swap();
                    TileManager.tileManager.hasTileSelected = false;
                    TileManager.tileManager.curTileSelected = null;
                    TileManager.tileManager.highlight.SetActive(false);
                }
            }
        }
    }

    void OnMouseExit()
    {
            Destroy(temporary_highlight);
            Destroy(x_outline);
    }
#endif

    // a function to be called for the touch controls to highligh selected tile, Called from TileManager
    public void highlightTouch()
    {
        //this function wasn't actually that helpfull with the touch controls. 
        //
        //bool man_on = false;
        //for (int i = 0; i < GameState.game_state.Victims.Count; i++)
        //{
        //    if (GameState.game_state.Victims[i].GetComponent<Person>().cur_tile == this)
        //    {
        //        man_on = true;
        //        if (temporary_highlight != null)
        //        {
        //            Destroy(temporary_highlight);
        //        }
        //    }
        //}

        //if ((man_on || !can_be_swapped) && x_outline == null)
        //{
        //    x_outline = (GameObject)Instantiate(Resources.Load("XOutline"), transform.position, transform.rotation);
        //}

        //if (!man_on)
        //{
        //    if (temporary_highlight == null && can_be_swapped && TileManager.tileManager.curTileSelected != this)
        //    {
        //        //delete the old highlight when dragging your finger off of the old tile
        //        if(TileManager.tileManager.tempHighlight != temporary_highlight)
        //        {
        //            Destroy(TileManager.tileManager.tempHighlight);
        //            TileManager.tileManager.highlight = null;
        //        }
        //        temporary_highlight = (GameObject)Instantiate(Resources.Load("TemporaryHighlight"), transform.position, transform.rotation);
        //        TileManager.tileManager.tempHighlight = temporary_highlight;
        //    }

        //    if (can_be_swapped)
        //    {
        //        if (x_outline != null)
        //        {
        //            Destroy(x_outline);
        //        }
        //    }
        //}
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

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 1f,LayerMask.GetMask("Tile"));

        if (hit.transform != null && hit.transform.gameObject.GetComponent<Tile>() != null)
            return hit.transform.gameObject.GetComponent<Tile>();
        else
            return null;
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

        for (int i = 0; i < GameState.game_state.Victims.Count; i++)
        {
            if (GameState.game_state.Victims[i].GetComponent<Person>().cur_tile == this)
            {
                Instantiate(Resources.Load("XOutlineDiesAtTime"), transform.position, transform.rotation);
                return;
            }
        }
        TileManager.tileManager.swapsDone++;
        if (TileManager.tileManager.limitedMoves)
        {
            if(TileManager.tileManager.tileMovesAllowed <= 0)
            {
                return;
            }
            TileManager.tileManager.tileMovesAllowed = TileManager.tileManager.tileMovesAllowed - 1;
        }
        Tile tile_to_swap = TileManager.tileManager.curTileSelected;
        TileManager.tileManager.PlaySwapSound();

        Vector3 cur_position = transform.position;
        Vector3 otherTilePos = TileManager.tileManager.curTileSelected.transform.position;
        transform.position = TileManager.tileManager.curTileSelected.transform.position;
        tile_to_swap.transform.position = cur_position;
        GameObject transportTile1 = (GameObject)Instantiate(Resources.Load("TransportSquare"), cur_position, transform.rotation);
        transportTile1.GetComponent<FlyToPointAndDie>().pointToGoTo = otherTilePos;
        GameObject transportTile2 = (GameObject)Instantiate(Resources.Load("TransportSquare"), otherTilePos, transform.rotation);
        transportTile2.GetComponent<FlyToPointAndDie>().pointToGoTo = cur_position;
        Instantiate(Resources.Load("smoke_puff"),transform.position,transform.rotation);
        Instantiate(Resources.Load("smoke_puff"), cur_position, transform.rotation);
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
