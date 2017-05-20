using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public List<GameObject> Victims;
    public List<Spawner> spawners;

    public bool game_over = false;

    public bool victory_counting_down = false;
    [HideInInspector]
    public float victory_countdown_timer = 30f;
    Canvas canvas;
    [HideInInspector]
    public Text timer;

    public string next_level = "";
    public string defeat_level = "MainMenu";

    float elapsed_time = 0;
    float game_over_wait = 4.5f;



    void Awake ()
    {
        game_state = this;
        canvas = GameObject.FindObjectOfType<Canvas>();
        timer = canvas.GetComponentInChildren<Text>(true);

        if (Settings.Endless_Mode)
            timer.gameObject.SetActive(true);

        GameObject go = Instantiate(Resources.Load("FadeOutBlack") as GameObject, canvas.transform);
        go.transform.localScale = Vector3.one;
    }


    public void CheckVictory()
    {
        if (spawners.Count == 0)
        {
            // Start the countdown to victory
            victory_counting_down = true;
            timer.gameObject.SetActive(true);
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
        yield return new WaitForSeconds(game_over_wait - 1.1f);
        GameObject go = Instantiate(Resources.Load("FadeInBlack") as GameObject, canvas.transform);
        go.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(next_level);
    }
    public void Defeat()
    {
        if (game_over)
            return;

        game_over = true;
        Debug.Log("Defeat!", this.gameObject);

        GameObject go = Instantiate(Resources.Load("EndLevelText") as GameObject, canvas.transform);
        go.transform.localScale = Vector3.one;
        go.GetComponent<Text>().text = "Mortal Escaped!";

        StartCoroutine(DefeatCoroutine());
    }
    IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(game_over_wait - 1.1f);
        GameObject go = Instantiate(Resources.Load("FadeInBlack") as GameObject, canvas.transform);
        go.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(defeat_level);
    }


	void Update ()
    {
        elapsed_time += Time.deltaTime;

        if (victory_counting_down && !game_over)
        {
            victory_countdown_timer -= Time.deltaTime;
            timer.text = "Time till victory: " + victory_countdown_timer;
            if (victory_countdown_timer <= 0)
            {
                timer.gameObject.SetActive(false);
                Victory();
            }
        }
        else if (Settings.Endless_Mode)
        {
            timer.text = "" + elapsed_time;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
