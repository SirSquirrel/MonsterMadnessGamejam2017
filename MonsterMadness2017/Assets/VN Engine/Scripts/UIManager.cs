using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager ui_manager;

    // CSV to load so our UI can be put into the proper language
    public TextAsset Localized_UI_CSV;
    // Each language has a dictionary, then that dictionary is searched for a specific key
    [HideInInspector]
    public Dictionary<string, Dictionary<string, string>> Localized_UI_Dictionaries = new Dictionary<string, Dictionary<string, string>>();
    [HideInInspector]
    List<LocalizeTextElement> localized_ui_elements = new List<LocalizeTextElement>();  // Elements that were localized. Get localized again if the language changes

    public Font[] fonts;

    public Text text_panel;
	public Text speaker_panel;
	public GameObject choice_panel;
	public Image background;	// Background image
	public Image foreground;    // Image appears in front of ALL ui elements
    public Text log_text;   // Log containing all dialogue spoken
    public Text scroll_log; // Log accessed by using the mouse scroll wheel when hovering over the dialogue panel
    public ScrollRect scroll_log_rect;
    public GameObject item_grid;   // Item grid where Items are placed
    public Text item_description;
    public Image large_item_image;
    public GameObject entire_UI_panel;
    public GameObject ingame_UI_menu;

    // Used by ChoiceNode
    public Text choice_text_banner;
    public Button[] choice_buttons;

    public GameObject actor_positions;

    // Text options
    public Slider text_scroll_speed_slider;
    public Slider text_size_slider;
    public Toggle text_autosize;

    public FontDropDown font_drop_down;

    public Dropdown language_dropdown;


    void Awake ()
    {
		ui_manager = this;

        if (Localized_UI_CSV != null)
            Localized_UI_Dictionaries = CSVReader.Generate_Localized_Dictionary(Localized_UI_CSV);
        else
            Debug.Log("No Localized_UI_CSV specified", this.gameObject);

        // Get the current language stored in player prefs
        //Set_Language(PlayerPrefs.GetString("Language", LocalizationManager.Supported_Languages[0]));
    }


    // Allows you to set the language in LocalizedManager.Language. Be sure to change the support languages in LocalizationManager.cs
    public void Set_Language(string language)
    {
        LocalizationManager.Set_Language(language);

        // Set the language dropdown correctly in the menu
        int x = 0;
        foreach (string s in LocalizationManager.Supported_Languages)
        {
            if (s == LocalizationManager.Language)
            {
                language_dropdown.value = x;
                break;
            }
            x++;
        }

        Apply_Localization_Change();
    }
    // Takes an int which is used to index the supported languages array in LocalizationManager.cs
    public void Set_Language(int language)
    {
        Set_Language(LocalizationManager.Supported_Languages[language]);
    }


    public void Apply_Localization_Change()
    {
        if (VNSceneManager.scene_manager != null
                && VNSceneManager.current_conversation != null
                && VNSceneManager.current_conversation.Get_Current_Node().GetType() == typeof(DialogueNode))
        {
            DialogueNode n = (DialogueNode)VNSceneManager.current_conversation.Get_Current_Node();
            n.GetLocalizedText(true);
        }

        foreach (LocalizeTextElement e in localized_ui_elements)
        {
            if (e != null)
            {
                e.LocalizeText();
            }
        }
    }


    // Returns the associated value with the given key for the language that has been set
    // Returns "" if the key is null or empty
    public string Get_Localized_UI_Entry(string key)
    {
        // Check for any potential errors
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("Get_Localized_UI_Entry key passed in is null or empty", this.gameObject);
            return "";
        }
        if (!Localized_UI_Dictionaries.ContainsKey(LocalizationManager.Language))
        {
            Debug.LogError("Get_Localized_UI_Entry could not find language " + LocalizationManager.Language, this.gameObject);
            return "";
        }
        if (!Localized_UI_Dictionaries[LocalizationManager.Language].ContainsKey(key))
        {
            Debug.LogError("Get_Localized_UI_Entry could not find the key " + key, this.gameObject);
            return "";
        }

        return Localized_UI_Dictionaries[LocalizationManager.Language][key];
    }


    public void Add_Localized_UI_Element_To_List(LocalizeTextElement element)
    {
        if (!localized_ui_elements.Contains(element))
        {
            localized_ui_elements.Add(element);
        }
    }
}
