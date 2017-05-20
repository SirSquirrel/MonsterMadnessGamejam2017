using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public List<GameObject> Victims;

    public bool game_over = false;

    public string next_level = "";
    public string defeat_level = "MainMenu";

    float game_over_wait = 5f;


    void Awake ()
    {
        game_state = this;
	}
	

    public void Victory()
    {
        if (game_over)
            return;

        game_over = true;
        Debug.Log("Victory!", this.gameObject);
        StartCoroutine(VictoryCoroutine());
    }
    IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(game_over_wait);
        UnityEngine.SceneManagement.SceneManager.LoadScene(next_level);
    }
    public void Defeat()
    {
        if (game_over)
            return;

        game_over = true;
        Debug.Log("Defeat!", this.gameObject);
        StartCoroutine(DefeatCoroutine());
    }
    IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(game_over_wait);
        UnityEngine.SceneManagement.SceneManager.LoadScene(defeat_level);
    }


	void Update () {
		
	}
}
