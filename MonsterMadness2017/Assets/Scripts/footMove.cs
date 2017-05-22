using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footMove : MonoBehaviour
{
    float speed_of_anim = 6f;
    float length_of_step = 0.1f;
    public bool opposite = false;

    Person p;

    void Start ()
    {
        p = this.GetComponentInParent<Person>();
	}


    void FixedUpdate ()
    {
        if (Time.timeScale > 0)
        {
            Vector3 pos = this.transform.localPosition;
            if (opposite)
                pos.x = Mathf.Sin(Time.time * speed_of_anim) * length_of_step;
            else
                pos.x = -Mathf.Sin(Time.time * speed_of_anim) * length_of_step;
            this.transform.localPosition = pos;
        }
    }
}
