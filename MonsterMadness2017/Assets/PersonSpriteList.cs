using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpriteList : MonoBehaviour {
    public Sprite[] personSprites;
    public static PersonSpriteList personSpriteList;
	// Use this for initialization
	void Start () {
        personSpriteList = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Sprite GetPersonSprite()
    {
        int rand = Random.Range(0, personSprites.Length);
        return personSprites[rand];
    }
}
