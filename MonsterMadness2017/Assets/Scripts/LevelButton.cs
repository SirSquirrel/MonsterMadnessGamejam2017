using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelButton : MonoBehaviour
{
    public string level_name;

    bool beaten;
    bool optimized;
    bool beaten_on_fast;

    public GameObject completion_star_outline;
    public GameObject completion_star;
    public GameObject optimized_star_outline;
    public GameObject optimized_star;
    public GameObject beaten_on_fast_star_outline;
    public GameObject beaten_on_fast_star;

    public GameObject fully_completed;


    void Start ()
    {
        this.transform.name = level_name;
        this.GetComponentInChildren<Text>().text = level_name;

        // Check player prefs for level completion stats
        beaten = Convert.ToBoolean(PlayerPrefs.GetInt(level_name, 0));
        optimized = Convert.ToBoolean(PlayerPrefs.GetInt(level_name + Optimized, 0));
        beaten_on_fast = Convert.ToBoolean(PlayerPrefs.GetInt(level_name + Beaten_on_fast, 0));

        // Set stars to match
        if (beaten)
        {
            completion_star_outline.SetActive(false);
            completion_star.SetActive(true);
        }
        if (optimized)
        {
            optimized_star_outline.SetActive(false);
            optimized_star.SetActive(true);
        }
        if (beaten_on_fast)
        {
            beaten_on_fast_star_outline.SetActive(false);
            beaten_on_fast_star.SetActive(true);
        }

        if (beaten && optimized && beaten_on_fast)
            fully_completed.SetActive(true);
    }

    public void ButtonClicked()
    {
        StartCoroutine(SwitchLevelCoroutine(level_name));
    }


    IEnumerator SwitchLevelCoroutine(string new_level)
    {
        Instantiate(Resources.Load("FadeInBlack") as GameObject, this.GetComponentInParent<Canvas>().transform);

        yield return new WaitForSeconds(1.1f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(new_level);
    }


    // Used so we save player prefs properly
    public static string Beaten = "";
    public static string Optimized = "optimized";
    public static string Beaten_on_fast = "beaten_on_fast";
}
