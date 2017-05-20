using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour
{
    public void Quit_Application()
    {
        Application.Quit(); 
    }


    public void SwitchLevel(string new_level)
    {
        StartCoroutine(SwitchLevelCoroutine(new_level));
    }
    IEnumerator SwitchLevelCoroutine(string new_level)
    {
        Instantiate(Resources.Load("FadeInBlack") as GameObject, this.GetComponentInParent<Canvas>().transform);

        yield return new WaitForSeconds(1.1f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(new_level);
    }


    public void SetNewDifficulty(int dif)
    {
        switch (dif)
        {
            case 0:
                Settings.SetDifficulty("Normal");
                break;
            case 1:
                Settings.SetDifficulty("Hard");
                break;
        }
        Debug.Log(dif + ":" + Settings.Current_Difficulty);

    }
    public void SetEndlessMode(bool mode)
    {
        Settings.Endless_Mode = mode;
    }
}
