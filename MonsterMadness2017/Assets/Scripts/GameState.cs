using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState game_state;


    void Awake ()
    {
        game_state = this;
	}
	

    public void Victory()
    {
        Debug.Log("Victory!", this.gameObject);
    }
    public void Defeat()
    {
        Debug.Log("Defeat!", this.gameObject);
    }


	void Update () {
		
	}
}
