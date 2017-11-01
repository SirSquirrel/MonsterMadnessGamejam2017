using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates level select buttons based on the LevelOrder.txt file
public class LevelSelectManager : MonoBehaviour
{
    public GameObject button_to_spawn;

    public Transform novice_parent;
    public Transform intermediate_parent;
    public Transform experienced_parent;


    void Start ()
    {
        SpawnButtonsInCategory(LevelManager.novice_levels, novice_parent);
        SpawnButtonsInCategory(LevelManager.intermediate_levels, intermediate_parent);
        SpawnButtonsInCategory(LevelManager.expert_levels, experienced_parent);
    }


    public void SpawnButtonsInCategory(List<string> level_list, Transform parent_to_use)
    {
        foreach (string level_name in level_list)
        {
            CreateButtonAsChildOf(button_to_spawn, level_name, parent_to_use);
        }
    }
    public void CreateButtonAsChildOf(GameObject button_to_use, string level_name, Transform parent)
    {
        GameObject new_button = Instantiate(button_to_use, parent);
        new_button.GetComponent<LevelButton>().level_name = level_name;
    }
}
