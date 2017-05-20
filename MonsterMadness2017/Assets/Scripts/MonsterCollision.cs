using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollision : MonoBehaviour {

    public AudioSource scream;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag.Equals("Person"))
        {
            GameState.game_state.Victims.Remove(collider.gameObject);
            Destroy(collider.gameObject);
            scream.Play();
            Instantiate(Resources.Load("blood"), transform.position, transform.rotation);
        }
    }
}
