using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSpriteList : MonoBehaviour {
    public Sprite[] furnitureSprites;
    public static FurnitureSpriteList furnitureSpriteList;
    // Use this for initialization
    void Start()
    {
        furnitureSpriteList = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Sprite GetFurnitureSprite()
    {
        int rand = Random.Range(0, furnitureSprites.Length);
        return furnitureSprites[rand];
    }
}
