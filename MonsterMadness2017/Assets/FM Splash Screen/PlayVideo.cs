using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideo : MonoBehaviour 
{

	void Start () 
	{
        #if UNITY_WEBGL
            Debug.Log("WebGL, not playing video");
        #else
            ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        #endif
    }
	
}
