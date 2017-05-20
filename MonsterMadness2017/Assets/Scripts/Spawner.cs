using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int people_to_spawn = 1;
    public Exit exit_side;

    float cur_spawn_delay = 0;
    public float initial_spawn_delay = 60f;
    public float spawn_delay = 20f;
    Tile this_tile;


    void Start ()
    {
        cur_spawn_delay = initial_spawn_delay;
        this_tile = this.GetComponent<Tile>();
	}


    void Update ()
    {
        cur_spawn_delay -= Time.deltaTime;

        if (cur_spawn_delay <= 0)
        {
            cur_spawn_delay = spawn_delay;
            Spawn();
            people_to_spawn--;
            if (people_to_spawn <= 0)
                Destroy(this);
        }
    }

    public void Spawn()
    {
        GameObject go = Instantiate(Resources.Load("Person") as GameObject);
        go.transform.position = this.transform.position;
        Person p = go.GetComponent<Person>();
        p.prev_destination = this.transform.position;

        // Set their current tile
        p.cur_tile = this_tile;

        switch (exit_side)
        {
            case Exit.Left:
                p.destination = this.transform.position + Vector3.left / 2f;
                p.walking_direction = Vector2.left;
                break;
            case Exit.Right:
                p.destination = this.transform.position + Vector3.right / 2f;
                p.walking_direction = Vector2.right;
                break;
            case Exit.Top:
                p.destination = this.transform.position + Vector3.up / 2f;
                p.walking_direction = Vector2.up;
                break;
            case Exit.Bottom:
                p.destination = this.transform.position + Vector3.down / 2f;
                p.walking_direction = Vector2.down;
                break;
        }
    }
}
