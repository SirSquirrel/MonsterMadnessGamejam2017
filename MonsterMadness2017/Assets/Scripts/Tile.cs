﻿using System.Collections;
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

    public Tile left_neighbour;
    public Tile right_neighbour;
    public Tile top_neighbour;
    public Tile bottom_neighbour;

    // Based on this tile's exits, and the neighbours exits, here where a person can go
    public bool can_go_left = false;
    public bool can_go_right = false;
    public bool can_go_top = false;
    public bool can_go_bottom = false;


    void Start ()
    {
        Find_Neighbours();
	}


    void Update ()
    {
		
	}


    public void Find_Neighbours()
    {
        // Raycast nearby tiles to find neighbours
        // Stick with units of 1

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


    public void CalculateExits()
    {
        can_go_left = left_neighbour != null && left_exit && left_neighbour.right_exit ? true : false;
        can_go_right = right_neighbour != null && right_exit && right_neighbour.left_exit ? true : false;
        can_go_top = top_neighbour != null && top_exit && top_neighbour.bottom_exit ? true : false;
        can_go_bottom = bottom_neighbour != null && bottom_exit && bottom_neighbour.top_exit ? true : false;
    }
}
