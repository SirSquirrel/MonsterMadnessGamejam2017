using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public static class LevelManager
{
    public static List<string> novice_levels = new List<string>();
    public static List<string> intermediate_levels = new List<string>();
    public static List<string> expert_levels = new List<string>();


    // Reads in the level order and sets the appropriate level lists
    [RuntimeInitializeOnLoadMethod]
    public static void ReadLevelFile()
    {
        List<string> current_level_list = novice_levels;
        TextAsset file = Resources.Load("LevelOrder") as TextAsset;
        Debug.Log(file);
        string[] linesFromfile = Regex.Split(file.text, "\r\n"); //file.text.Split( "\n"[0] );
        foreach(string line in linesFromfile)
        {
            //string line_temp = Regex.Replace(line, @"[^\u0009^\u000A^\u000D^\u0020-\u007E]", "");
            string line_temp = line.Replace(System.Environment.NewLine, "");
            switch (line_temp)
            {
                case "Novice":
                    current_level_list = novice_levels;
                    break;
                case "Intermediate":
                    current_level_list = intermediate_levels;
                    break;
                case "Expert":
                    current_level_list = expert_levels;
                    break;
                default:
                    // Not a new level category, add the level name to the current list
                    current_level_list.Add(line);
                    break;
            }
            /*
            if (line.Contains("Novice"))
            {
                current_level_list = novice_levels;
            }
            else if (line.Contains("Intermediate"))
            {
                current_level_list = intermediate_levels;
            }
            else if (line.Contains("Expert"))
            {
                current_level_list = expert_levels;
            }
            else
            {
                current_level_list.Add(line);
            }
            */
        }
        //StreamReader file = new StreamReader(path);

        // Go line by line
        /*
        string line;
        List<string> current_level_list = novice_levels;
        while ((line = file.ReadLine()) != null)
        {
            switch (line)
            {
                case "Novice":
                    current_level_list = novice_levels;
                    break;
                case "Intermediate":
                    current_level_list = intermediate_levels;
                    break;
                case "Expert":
                    current_level_list = expert_levels;
                    break;
                default:
                    // Not a new level category, add the level name to the current list
                    current_level_list.Add(line);
                    break;
            }
        }
        file.Close();
        */
        Debug.Log("Finished reading level file");
    }


    // Finds the next level in the current category, and if there isn't one, returns you to the main menu /w scene select
    public static string GetNextLevel(string current_level_name)
    {
        List<string> current_category = novice_levels;

        // Figure out which category we're in
        if (novice_levels.Contains(current_level_name))
            current_category = novice_levels;
        else if (intermediate_levels.Contains(current_level_name))
            current_category = intermediate_levels;
        else if (expert_levels.Contains(current_level_name))
            current_category = expert_levels;
        else
        {
            // Can't find out current category, go to main menu
            Debug.Log("Couldn't find level category /w name " + current_level_name);
            return "MainMenu";
        }

        // We've got a category, find the NEXT level
        int next_level_index = current_category.IndexOf(current_level_name) + 1;
        if (next_level_index < current_category.Count)
            return current_category[next_level_index];
        else
            // Must be at end of category, go to menu
            return "MainMenu";
    }
}
