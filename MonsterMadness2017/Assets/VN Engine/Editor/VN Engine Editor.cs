using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;

// Creates a new menu when you right click in the Hierarchy pane.
// Allows the user to easily create dialogue elements
public class VNEngineEditor : MonoBehaviour
{
    [MenuItem("VN Engine/Documentation")]
    private static void OpenDocumentation()
    {
        Application.OpenURL(Application.dataPath + "/VN Engine/README.pdf");
    }


    // Imports a .txt file specified by the user.
    [MenuItem("VN Engine/Import script (from .txt file)")]
    private static void ImportTxtScriptFile()
    {
        string path = EditorUtility.OpenFilePanel("Select a script file to import", "", "txt");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        Debug.Log("Reading in .txt script file: " + path);

        // Read the file
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(path);

        // Create a child object to hold all elements created from this file
        Transform import_parent = (new GameObject(Path.GetFileNameWithoutExtension(path))).transform;
        ConversationManager cur_conversation = null;

        // Read it line by line
        while ((line = file.ReadLine()) != null)
        {
            // Continue if it's an empty line
            if (String.IsNullOrEmpty(line))
                continue;
            string[] split_line = line.Split(new char[] { ':' } , 2);
            
            // Create a new conversation
            if (line.StartsWith("Conversation", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                GameObject go = new GameObject(split_line[1] + " Conversation");
                go.transform.parent = import_parent;
                ConversationManager new_conv = go.AddComponent<ConversationManager>();
                if (cur_conversation != null)
                {
                    cur_conversation.start_conversation_when_done = new_conv;
                }
                cur_conversation = new_conv;
            }
            else if (line.StartsWith("EnterActor", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                GameObject go = new GameObject("Enter " + split_line[1]);
                go.transform.parent = cur_conversation.transform;
                EnterActorNode node = go.AddComponent<EnterActorNode>();
                node.actor_name = split_line[1];
            }
            else if (line.StartsWith("ExitActor", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                GameObject go = new GameObject("Exit " + split_line[1]);
                go.transform.parent = cur_conversation.transform;
                ExitActorNode node = go.AddComponent<ExitActorNode>();
                node.actor_name = split_line[1];
            }
            // ADD MORE HERE IF YOU WISH TO EXTEND THE IMPORTING FUNCTIONALITY
            //
            //
            //
            //
            //

            // Must be a line of dialogue
            else if (split_line.Length == 2)
            {
                GameObject go = new GameObject(split_line[0]);
                go.transform.parent = cur_conversation.transform;
                DialogueNode node = go.AddComponent<DialogueNode>();
                node.actor = split_line[0];
                node.textbox_title = split_line[0];
                node.text = split_line[1];
            }
        }
        file.Close();
        Debug.Log("Done importing script: " + path);
    }




    [MenuItem("GameObject/VN Engine/Create DialogueCanvas", false, 0)]
    private static void CreateDialogueCanvas(MenuCommand menuCommand)
    {
        GameObject go = Instantiate(Resources.Load("DialogueCanvas", typeof(GameObject))) as GameObject;     // Create new object
        go.name = "DialogueCanvas";
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/VN Engine/New Conversation", false, 0)]
    private static void CreateConversation(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Conversation");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ConversationManager>();
    }


    [MenuItem("GameObject/VN Engine/Add Dialogue", false, 0)]
    private static void AddDialogue(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Dialogue");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<DialogueNode>();
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().playOnAwake = false;
    }


    [MenuItem("GameObject/VN Engine/Enter Actor", false, 0)]
    private static void EnterActor(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Enter Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<EnterActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Exit Actor", false, 0)]
    private static void ExitActor(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Exit Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ExitActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Exit All Actors", false, 0)]
    private static void ExitAllActors(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Exit All Actors");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ExitAllActorsNode>();
    }


    [MenuItem("GameObject/VN Engine/Change Conversation", false, 0)]
    private static void ChangeConversation(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Change Conversation");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ChangeConversationNode>();
    }


    [MenuItem("GameObject/VN Engine/Change Actor Image", false, 0)]
    private static void ChangeActorImage(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Change Actor Image");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ChangeActorImageNode>();
    }


    [MenuItem("GameObject/VN Engine/Wait", false, 0)]
    private static void Wait(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Wait");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<WaitNode>();
    }


    [MenuItem("GameObject/VN Engine/Show Choices", false, 0)]
    private static void ShowChoicesNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Show Choices");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ChoiceNode>();
    }


    [MenuItem("GameObject/VN Engine/If Then", false, 0)]
    private static void IfNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("If");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<IfNode>();
    }


    [MenuItem("GameObject/VN Engine/Set Background", false, 0)]
    private static void SetBackground(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Set Background");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<SetBackground>();
    }



    [MenuItem("GameObject/VN Engine/Alter Stats", false, 0)]
    private static void AlterStatsNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Alter Stats");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<AlterStatNode>();
    }



    [MenuItem("GameObject/VN Engine/Item", false, 0)]
    private static void ItemNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Item");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ItemNode>();
    }



    [MenuItem("GameObject/VN Engine/Hide or Show UI", false, 0)]
    private static void HideShowUI(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Hide/Show UI");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<HideShowUINode>();
    }



    [MenuItem("GameObject/VN Engine/Fade in from Black", false, 0)]
    private static void FadeInFromBlack(MenuCommand menuCommand)
    {
        GameObject go = Instantiate(Resources.Load("Conversation Pieces/Fade in from Black", typeof(GameObject))) as GameObject;     // Create new object
        go.name = "Fade in from Black";
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/VN Engine/Fade out to Black", false, 0)]
    private static void FadeToBlack(MenuCommand menuCommand)
    {
        GameObject go = Instantiate(Resources.Load("Conversation Pieces/Fade out to Black", typeof(GameObject))) as GameObject;     // Create new object
        go.name = "Fade out to Black";
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/VN Engine/Move Actor Inwards", false, 0)]
    private static void MoveActorInwardsNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Move Actor Inwards");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<MoveActorInwardsNode>();
    }


    [MenuItem("GameObject/VN Engine/Move Actor Outwards", false, 0)]
    private static void MoveActorOutwardsNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Move Actor Outwards");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<MoveActorOutwardsNode>();
    }


    [MenuItem("GameObject/VN Engine/Bring Actor Forward", false, 0)]
    private static void BringActorForwardNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Bring Actor Forward");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<BringActorForwardNode>();
    }


    [MenuItem("GameObject/VN Engine/Bring Actor Backward", false, 0)]
    private static void BringActorBackwardNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Bring Actor Backward");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<BringActorBackwardNode>();
    }


    [MenuItem("GameObject/VN Engine/Darken Actor", false, 0)]
    private static void DarkenActorNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Darken Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<DarkenActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Brighten Actor", false, 0)]
    private static void BrightenActorNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Brighten Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<BrightenActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Flip Actor", false, 0)]
    private static void FlipActorFacingNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Flip Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<FlipActorFacingNode>();
    }


    [MenuItem("GameObject/VN Engine/Play Sound Effect", false, 0)]
    private static void PlaySoundEffectNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Play Sound Effect");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<PlaySoundEffectNode>();
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().playOnAwake = false;
    }


    [MenuItem("GameObject/VN Engine/Play Music", false, 0)]
    private static void PlayMusicNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Play Music");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<SetMusicNode>();
    }


    [MenuItem("GameObject/VN Engine/Fade out Music", false, 0)]
    private static void FadeOutMusicNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Fade out Music");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<SetMusicNode>();
        go.GetComponent<SetMusicNode>().fadeOutPreviousMusic = true;
        go.GetComponent<SetMusicNode>().fadeOutTime = 5.0f;
    }



    [MenuItem("GameObject/VN Engine/Perform Actions", false, 0)]
    private static void PerformActionsNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Perform Actions");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<PerformActionsNode>();
    }



    [MenuItem("GameObject/VN Engine/Load Scene", false, 0)]
    private static void LoadSceneNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Load Scene");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<LoadSceneNode>();
    }


    [MenuItem("GameObject/VN Engine/Clear Text", false, 0)]
    private static void ClearTextNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Clear Text");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ClearTextNode>();
    }
}
