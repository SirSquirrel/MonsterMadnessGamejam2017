using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Person : MonoBehaviour
{
    Vector2 destination;
    Vector2 prev_destination;

    Tile cur_tile;


    void Start ()
    {
		
	}
	

	void Update ()
    {
	    // Walk towards the destination
        
        // Are we at destination? If, figure out where we can turn

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Debug.Log("Person has escaped the mansion!", this.gameObject);
            GameState.game_state.Defeat();
        }
    }
}
