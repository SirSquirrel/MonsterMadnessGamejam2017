using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;


// This class is static, so you can call it from anywhere.
// Based on this tutorial: http://gamedevelopment.tutsplus.com/tutorials/how-to-save-and-load-your-players-progress-in-unity--cms-20934
public static class SaveManager
{
    // All saved games
    public static List<SaveFile> saved_games = new List<SaveFile>();

    // Name of the save file we save and load to
    private static string save_file_name = "saved_games.gd";
    // Full path/file location of our save file
    private static string full_save_file_path = Application.persistentDataPath + "/" + save_file_name;


    // Adds the saved game to the saved_games list
    public static void AddNewSave(SaveFile current)
    {
        // Add to the saved_games list
        saved_games.Add(current);
        Save();
    }
    // Saves the current saved_games list to a file
    public static void Save()
    {
        Debug.Log("Saving file: " + full_save_file_path);

        // Open a file called saved_games.gd, and save our saved_games list into it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(full_save_file_path);
        bf.Serialize(file, SaveManager.saved_games);
        file.Close();
    }



    // Loads a bunch of saves into the saved_games list from the save file
    public static void LoadFromFile()
    {
        if (File.Exists(full_save_file_path))
        {
            Debug.Log("Loading save file: " + full_save_file_path);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(full_save_file_path, FileMode.Open);
            SaveManager.saved_games = (List<SaveFile>)bf.Deserialize(file);
            file.Close();
        }
        else
            Debug.Log("Could not find save file: " + full_save_file_path);
    }



    // Removes the specified save, then saves the remaining saves to file
    public static void DeleteSave(SaveFile save)
    {
        saved_games.Remove(save);
        Save();
    }



    public static void DeleteAllSaves()
    {
        if (File.Exists(Application.persistentDataPath + "/" + save_file_name))
        {
            File.Delete(full_save_file_path);
            saved_games.Clear();
            Debug.Log("Save file deleted");
        }
    }




    public static string GetGameObjectPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }
}
