using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSpriteScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite
            = FurnitureSpriteList.furnitureSpriteList.GetFurnitureSprite();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
