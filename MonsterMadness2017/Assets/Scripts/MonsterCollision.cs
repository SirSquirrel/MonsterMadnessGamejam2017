using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollision : MonoBehaviour {

    public AudioSource scream;

    void Start ()
    {
		
	}


    void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag.Equals("Person"))
        {
            GameState.game_state.Victims.Remove(collider.gameObject);
            Destroy(collider.gameObject);
            scream.Play();
            Instantiate(Resources.Load("blood"), transform.position, transform.rotation);

            if(GameState.game_state.monsterEatLevel)
            {
                GameState.game_state.monsterEaten++;
                if (GameState.game_state.monsterEatLevel)
                {
                    if (GameState.game_state.monsterEaten >= GameState.game_state.monsterEatVictory)
                    {
                        GameState.game_state.victory_counting_down = true;
                        GameState.game_state.timer.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
