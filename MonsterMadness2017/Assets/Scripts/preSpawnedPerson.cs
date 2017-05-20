using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preSpawnedPerson : Person{

	// Use this for initialization
	void Start () {
        GameState.game_state.Victims.Add(this.gameObject);
        if (Settings.Current_Difficulty == Difficulty.Hard)
        {
            walking_speed = walking_speed * 1.5f;
        }

        //find which tile we are currently on
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up,1f, LayerMask.GetMask("Tile"));
        cur_tile = hit.transform.gameObject.GetComponent<Tile>();
        transform.position = cur_tile.transform.position;
        prev_destination = transform.position;
        Vector2 new_dir = Get_Random_Direction();
        walking_direction = new_dir;
        destination = prev_destination + new_dir / 2f;
    }
	
	
}
