using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float z_position = -5f;


	void Update ()
    {
		if (Time.timeScale != 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = z_position;
            this.transform.position = pos;
        }
    }
}
