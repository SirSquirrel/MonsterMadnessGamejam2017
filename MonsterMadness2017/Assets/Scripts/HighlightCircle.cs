using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCircle : MonoBehaviour
{
    SpriteRenderer s;
    bool entering_in = true;

    float initial_scale = 5f;
    float counter = 0;
    float rate_of_change = 0.5f;

    void Start ()
    {
        s = this.GetComponent<SpriteRenderer>();
        Color c = s.color;
        c.a = 0;
        s.color = c;
    }


    void Update ()
    {
		if (entering_in)
        {
            counter += Time.deltaTime * rate_of_change;
            counter = Mathf.Min(counter, 1);
            float new_scale = Mathf.Lerp(initial_scale, 1f, counter);
            this.transform.localScale = new Vector3(new_scale, new_scale, 1f);

            Color c = s.color;
            c.a = counter;
            s.color = c;

            if (counter >= 1)
            {
                entering_in = false;
                counter = 1;
            }
        }
        else
        {
            // Fade out
            counter -= Time.deltaTime * 2f;
            Color c = s.color;
            c.a = counter;
            s.color = c;

            if (counter <= 0)
                Destroy(this.gameObject);
        }
    }
}
