using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledPlayerPref : MonoBehaviour
{
    public string player_pref_to_check;
    public GameObject object_to_activate;

    void Awake ()
    {
        bool activate = Convert.ToBoolean(PlayerPrefs.GetInt(player_pref_to_check, 0));
        object_to_activate.SetActive(activate);
	}
}
