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
    public bool defeated = false;

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

    public GameObject game_over_text_options;


    void Awake ()
    {
        game_state = this;
        canvas = GameObject.FindObjectOfType<Canvas>();
        timer = canvas.GetComponentInChildren<Text>(true);
        Time.timeScale = 1f;

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

        StartCoroutine(LoadSceneDelayed(game_over_wait, next_level, true));
    }
    public void Defeat()
    {
        if (game_over)
            return;

        defeated = true;
        game_over = true;
        Debug.Log("Defeat!", this.gameObject);

        GameObject go = Instantiate(Resources.Load("EndLevelText") as GameObject, canvas.transform);
        go.transform.localScale = Vector3.one;
        go.GetComponent<Text>().text = "Mortal Escaped!";
        StartCoroutine(DefeatCoroutine());
    }
    IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(game_over_wait - 0.5f);
        game_over_text_options.SetActive(true);
    }


    public IEnumerator LoadSceneDelayed(float delay, string scene, bool fade_out)
    {
        delay = Mathf.Max(1, delay);

        yield return new WaitForSeconds(delay - 1);

        if (fade_out)
        {
            GameObject go = Instantiate(Resources.Load("FadeInBlack") as GameObject, canvas.transform);
            go.transform.localScale = Vector3.one;
        }

        yield return new WaitForSeconds(1f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }


    void Update ()
    {
        elapsed_time += Time.deltaTime;

        if (victory_counting_down && !game_over)
        {
            victory_countdown_timer -= Time.deltaTime;
            timer.text = "Time till victory: " + (int) victory_countdown_timer;
            if (victory_countdown_timer <= 0)
            {
                timer.gameObject.SetActive(false);
                Victory();
            }
        }
        else if (Settings.Endless_Mode && !game_over)
        {
            timer.text = "" + (int) elapsed_time;
        }

        if (defeated)
        {
            if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Y))
            {
                // Retry this level
                StartCoroutine(LoadSceneDelayed(1.02f, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, true));
                defeated = false;
            }
            else if (Input.GetButtonDown("Cancel") || Input.GetKey(KeyCode.N))
            {
                // Quit to menu
                StartCoroutine(LoadSceneDelayed(1.02f, defeat_level, true));
                defeated = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Restart level
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }


        if (Time.timeScale != 0 && Input.GetKey(KeyCode.BackQuote))
        {
            Time.timeScale = 5.0f;
        }
        else if (Time.timeScale != 0)
        {
            Time.timeScale = 1.0f;
        }
    }
}
