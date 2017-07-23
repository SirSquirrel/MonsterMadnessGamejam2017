using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A class to handle the hardware buttons on the phone from within a level of the game
public class PhoneButtonInGameReceiver : MonoBehaviour {
    
	void Start () {
		
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Pause.pause.paused)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Pause.pause.PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Menu))
        {
            Pause.pause.PauseGame();
        }
    }
}
