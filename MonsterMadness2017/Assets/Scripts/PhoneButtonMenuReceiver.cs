using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneButtonMenuReceiver : MonoBehaviour {
    public Button menuButton;
	
	void Start () {
		
	}
	
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Menu))
        {
            menuButton.onClick.Invoke();
        }
    }
}
