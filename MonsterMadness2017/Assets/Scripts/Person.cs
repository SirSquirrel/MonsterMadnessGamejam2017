using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Exit {  Left, Right, Top, Bottom };

[RequireComponent(typeof(Collider2D))]
public class Person : MonoBehaviour
{
    public Vector2 walking_direction;
    public float walking_speed = 1.0f;

    public Vector2 destination;
    public Vector2 prev_destination;
    public float progress_towards_destination = 0;

    public Tile cur_tile;
    Tile touched_tile;

    float time_alive = 0f;

    public float rotationSpeed = 0.1f;

    //variables for fading
    public bool fading = false;
    public float opacity = 1f;
    public float fadeSpeed = 0.01f;


    void Start ()
    {
        GameState.game_state.Victims.Add(this.gameObject);
        GetComponent<SpriteRenderer>().sprite = PersonSpriteList.personSpriteList.GetPersonSprite();
        if(Settings.Current_Difficulty == Difficulty.Hard)
        {
            walking_speed = walking_speed * 1.5f;
        }
	}
	

	void Update ()
    {
        if(fading)
        {
            Fade();
            return;
        }

        Rotate_To_Walking_Direction();

        time_alive += Time.deltaTime;

        if (cur_tile == null)
            Debug.LogError("Person is not on a tile!", this.gameObject);
        if (destination == null)
            Debug.LogError("Person  has no destination", this.gameObject);

        // Walk towards destination
        progress_towards_destination += walking_speed * Time.deltaTime;
        this.transform.position = Vector2.Lerp(prev_destination, destination, Mathf.Min(1, progress_towards_destination));
        
        // Did we make it to where we wanted to go?
        if (progress_towards_destination >= 1)
        {
            // Get a new destination
            // Either at the center of a tile
            if (Mathf.Abs(this.transform.position.x % 1f) == 0f && Mathf.Abs(this.transform.position.y % 1f) == 0f)
            {
                // Determine which direction to go (from which is allowed)
                if (cur_tile.entrances == 1)
                {
                    // Dead end, go back the way we came
                    this.walking_direction = -walking_direction;
                    Vector2 temp = destination;
                    destination = prev_destination;
                    prev_destination = temp;
                }
                else
                {
                    // There is more than one entrance, go the direction we weren't going before
                    Vector2 new_dir = -walking_direction;
                    // Keep picking until we get a new direction
                    while (new_dir == -walking_direction)
                    {
                        new_dir = Get_Random_Direction();
                    }
                    prev_destination = destination;
                    walking_direction = new_dir;
                    destination = destination + new_dir / 2f;
                }
            }
            // Or at the edge of a tile
            else
            {
                // Can we keep walking?
                if (Can_Walk_Onto_New_Tile())
                {
                    cur_tile = touched_tile;
                    // Set prev destination
                    prev_destination = destination;
                    // Walk to center of new tile
                    destination = cur_tile.transform.position;
                }
                else
                {
                    // Dead end, go back the way we came
                    this.walking_direction = -walking_direction;
                    Vector2 temp = destination;
                    destination = prev_destination;
                    prev_destination = temp;
                }
            }

            progress_towards_destination -= 1;
        }
	}

    public void Rotate_To_Walking_Direction()
    {
        float angle = Mathf.Atan2(walking_direction.y, walking_direction.x) * Mathf.Rad2Deg;
        Quaternion m_desiredDirection = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, m_desiredDirection,
                                              Time.deltaTime * rotationSpeed);
    }


    public Vector2 Get_Random_Direction()
    {
        Vector2 new_dir = Vector2.zero;

        // Keep picking until we get a new direction
        while (new_dir == Vector2.zero)
        {
            int random = Random.Range(0, 4);
            switch (random)
            {
                case 0:
                    // Left
                    if (cur_tile.left_exit)
                        new_dir = Vector2.left;
                    break;
                case 1:
                    // Right
                    if (cur_tile.right_exit)
                        new_dir = Vector2.right;
                    break;
                case 2:
                    // Top
                    if (cur_tile.top_exit)
                        new_dir = Vector2.up;
                    break;
                case 3:
                    // Bottom
                    if (cur_tile.bottom_exit)
                        new_dir = Vector2.down;
                    break;
            }
        }

        return new_dir;
    }


    public bool Can_Walk_Onto_New_Tile()
    {
        if (walking_direction == Vector2.left)
        {
            if (cur_tile.can_go_left)
                return true;
            else
                return false;
        }
        else if (walking_direction == Vector2.right)
        {
            if (cur_tile.can_go_right)
                return true;
            else
                return false;
        }
        else if (walking_direction == Vector2.up)
        {
            if (cur_tile.can_go_top)
                return true;
            else
                return false;
        }
        else if (walking_direction == Vector2.down)
        {
            if (cur_tile.can_go_bottom)
                return true;
            else
                return false;
        }

        return false;
    }

    public void Fade()
    {
        if (opacity > 0)
        {
            opacity = opacity - fadeSpeed;
        }
        GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,opacity) ;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            touched_tile = collision.gameObject.GetComponent<Tile>();
        }

        //the following code block being reached means the game has ended
        else if (collision.tag == "Exit" && time_alive > 1f)
        {
            Debug.Log("Person has escaped the mansion!", this.gameObject);
            GameState.game_state.Defeat();
            Camera.main.GetComponent<CameraManager>().target_pos = this.transform.position;
            Camera.main.GetComponent<CameraManager>().SetZoomLevel(1.0f);
            GameState.game_state.Victims.Remove(gameObject);
            fading = true;
            var children = new List<Transform>();
            for(int i = 0; i < transform.childCount;i++)
            {
                children.Add(transform.GetChild(i));
            }
            for (int i = 0; i < children.Count; i++)
            {
                Destroy(children[i].gameObject);
            }

            Spawner Exit = collision.GetComponent<Spawner>();
            Exit.sounds[1].Play();
            Exit.this_anim.SetTrigger("CycleDoor");



        }
    }
}
