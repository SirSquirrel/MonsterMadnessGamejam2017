using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Difficulty { Easy, Normal, Hard };

public static class Settings
{
    public static Difficulty Current_Difficulty = Difficulty.Normal;

    public static void SetDifficulty(string new_difficulty)
    {
       Current_Difficulty = (Difficulty) Enum.Parse(typeof(Difficulty), new_difficulty);
    }

    public static bool Endless_Mode = false;
}
