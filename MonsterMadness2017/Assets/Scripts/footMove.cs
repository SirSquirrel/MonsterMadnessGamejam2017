using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footMove : MonoBehaviour {
    public float speed = 0.1f;
    public float animationSpeed = 3f;
    public bool opposite = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (opposite)
            transform.position = transform.position + transform.right * Mathf.Sin(Time.time * animationSpeed) * speed * animationSpeed;
        else
            transform.position = transform.position - transform.right * Mathf.Sin(Time.time * animationSpeed) * speed * animationSpeed;
    }
}
