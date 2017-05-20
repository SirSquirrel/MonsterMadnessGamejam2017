using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script kills the object it is attached to when die_time seconds have elapsed
public class DieAtTime : MonoBehaviour {
    public float die_time = 1;
    public float die_timer;
	// Use this for initialization
	void Start () {
        die_timer = die_time + Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(die_timer <= Time.time)
        {
            Destroy(gameObject);
        }
	}
}
