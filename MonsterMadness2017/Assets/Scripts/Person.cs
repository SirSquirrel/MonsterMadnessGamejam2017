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
    float progress_towards_destination;

    Tile cur_tile;


    void Start ()
    {
		
	}
	

	void Update ()
    {
        if (cur_tile == null)
            Debug.LogError("Person is not on a tile!", this.gameObject);
        if (destination == null)
            Debug.LogError("Person  has no destination", this.gameObject);


        // Walk towards the destination
        Vector2 new_pos = (Vector2) transform.position + walking_direction * walking_speed;
        Vector2 current_pos = this.transform.position;

        progress_towards_destination += walking_speed * Time.deltaTime;
        this.transform.position = Vector2.Lerp(prev_destination, destination, Mathf.Min(0, progress_towards_destination));
        
        // Did we make it to where we wanted to go?
        if (progress_towards_destination >= 1)
        {
            // Get a new destination
            // Either at the center of a tile
            if (this.transform.position.x % 1f == 0)
            {
                // Determine which direction to go (from which is allowed)
                // Check if this is a dead end, go back the way we came
                this.walking_direction = -walking_direction;
                Vector2 temp = destination;
                destination = prev_destination;
                prev_destination = temp;

                // Not a dead end
            }
            // Or at the edge of a tile
            else
            {
                // Walk to center of new tile
                destination = cur_tile.transform.position;

                // Set prev destination
                prev_destination = destination;
            }
        }
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
