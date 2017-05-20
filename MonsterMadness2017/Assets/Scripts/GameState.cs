using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public List<GameObject> Victims;
    public List<Spawner> spawners;

    public bool endless_mode = false;   // If set to true, spawners will never run out of people
    public bool game_over = false;

    public bool victory_counting_down = false;
    [HideInInspector]
    public float victory_countdown_timer = 30f;

    public string next_level = "";
    public string defeat_level = "MainMenu";

    float game_over_wait = 5f;

    Canvas canvas;


    void Awake ()
    {
        game_state = this;
        canvas = GameObject.FindObjectOfType<Canvas>();
	}


    public void CheckVictory()
    {
        if (spawners.Count == 0)
        {
            // Start the countdown to victory
            victory_counting_down = true;
        }
    }
	

    public void Victory()
    {
        if (game_over)
            return;

        game_over = true;
        Debug.Log("Victory!", this.gameObject);

        GameObject go = Instantiate(Resources.Load("EndLevelText") as GameObject, canvas.transform);
        go.GetComponent<Text>().text = "All Mortals Trapped!";

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

        GameObject go = Instantiate(Resources.Load("EndLevelText") as GameObject, canvas.transform);
        go.GetComponent<Text>().text = "Mortal Escaped!";

        StartCoroutine(DefeatCoroutine());
    }
    IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(game_over_wait);
        UnityEngine.SceneManagement.SceneManager.LoadScene(defeat_level);
    }


	void Update ()
    {
		if (victory_counting_down)
        {
            victory_countdown_timer -= Time.deltaTime;
            if (victory_countdown_timer <= 0)
                Victory();
        }
	}
}
