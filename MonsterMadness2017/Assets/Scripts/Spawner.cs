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
    public float lightStep = 10;
    public bool soundPlayed = false;
    public bool animPlayed = false;
    public bool lightTriggered = false;


    public AudioSource[] sounds;
    Tile this_tile;
    Animator this_anim;
    Light this_light;

    void Start ()
    {
        cur_spawn_delay = initial_spawn_delay;
        this_tile = this.GetComponent<Tile>();
        this_anim = this.GetComponentInChildren<Animator>();
        this_light = this.GetComponentInChildren<Light>();
        GameState.game_state.spawners.Add(this);

	}


    void Update ()
    {
        cur_spawn_delay -= Time.deltaTime;
        if(cur_spawn_delay <= 5 && !soundPlayed)
        {
            sounds[0].Play();
            soundPlayed = true;
            lightTriggered = true;

            GameObject go = Instantiate(Resources.Load("Circle") as GameObject);
            go.transform.position = this.transform.position;
        }

        if (lightTriggered && this_light.intensity <= 2)
        {
            this_light.intensity += lightStep * Time.deltaTime;
        }

        if (cur_spawn_delay <= 1 && !animPlayed)
        {
            this_anim.SetTrigger("CycleDoor");
            animPlayed = true;
        }

        if (cur_spawn_delay <= 0)
        {
            soundPlayed = false;
            animPlayed = false;
            lightTriggered = false;
            this_light.intensity = 0.5f;
            cur_spawn_delay = spawn_delay;
            Spawn();

            // Spawners only decrease numbers if not on endless mode
            if (!Settings.Endless_Mode)
            {
                people_to_spawn--;

                if (people_to_spawn <= 0)
                {
                    GameState.game_state.spawners.Remove(this);
                    GameState.game_state.CheckVictory();
                    Destroy(this);
                }
            }
        }
    }

    public void Spawn()
    {
        GameObject go = Instantiate(Resources.Load("Person") as GameObject);
        go.transform.position = this.transform.position;
        Person p = go.GetComponent<Person>();
        p.prev_destination = this.transform.position;

        sounds[1].Play();

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
