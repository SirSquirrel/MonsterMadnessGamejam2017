using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToPointAndDie : MonoBehaviour {
    public Vector3 pointToGoTo;
    public Vector3 start;
    public float speed = 2f;
    public float dieTime = 1f;
    private float fadeTimer = 1f;
    private float fadeDuration = 0.34f;
    private float startTime = 0f;
    private float journeyLength;
    private Color alphaColor;

	// Use this for initialization
	void Start () {
        //initialize counters and movement variables needed for linear interpolation
        start = transform.position;
        dieTime += Time.time;
        journeyLength = Vector3.Distance(start, pointToGoTo);
        startTime = Time.time;
        fadeTimer = fadeTimer / speed;
        fadeTimer = fadeTimer + Time.time;
        alphaColor = GetComponent<SpriteRenderer>().color;
        alphaColor.a = 0;
    }
	
	// Update is called once per frame
	void Update () {
        float distCovered = (Time.time - startTime) * speed;
        transform.position = Vector3.Lerp(start,pointToGoTo,distCovered);
        if(dieTime<Time.time)
        {
            Destroy(gameObject);
        }
        if(fadeTimer < Time.time)
        {
            GetComponent<SpriteRenderer>().material.color = Color.Lerp(GetComponent<SpriteRenderer>().material.color, alphaColor, fadeDuration * Time.deltaTime);
        }
	}
}
