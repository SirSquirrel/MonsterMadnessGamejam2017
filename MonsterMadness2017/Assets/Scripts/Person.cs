using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Person : MonoBehaviour
{
    public Vector2 walking_direction;
    public float walking_speed = 1.0f;

    Vector2 destination;
    Vector2 prev_destination;

    Tile cur_tile;


    void Start ()
    {
		
	}
	

	void Update ()
    {
        // Walk towards the destination
        this.transform.position = (Vector2) transform.position + walking_direction * walking_speed;

        // Are we at destination? If, figure out where we can turn

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            cur_tile = collision.gameObject.GetComponent<Tile>();
        }
        else if (collision.tag == "Exit")
        {
            Debug.Log("Person has escaped the mansion!", this.gameObject);
            GameState.game_state.Defeat();
        }
    }
}
