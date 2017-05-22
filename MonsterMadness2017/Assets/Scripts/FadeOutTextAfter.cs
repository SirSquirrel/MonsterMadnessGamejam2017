using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FadeOutTextAfter : MonoBehaviour
{
    public float wait_x_seconds;
    public float fade_out_rate = 1.0f;
    Text t;

    void Start ()
    {
        t = this.GetComponent<Text>();
	}


    void Update ()
    {
        wait_x_seconds -= Time.deltaTime;

        if (wait_x_seconds <= 0)
        {
            // Fading
            Color c = t.color;
            c.a -= Time.deltaTime * fade_out_rate;
            t.color = c;

            if (c.a <= 0)
                Destroy(this);
        }
	}
}
