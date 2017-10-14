using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFade : MonoBehaviour
{
    public static MenuFade menu_fade;

    bool currently_fading = false;
    public GameObject object_to_activate;
    public GameObject second_object_to_activate;
    public GameObject object_to_deactivate;


    public void SetSecondaryObjectToActivate(GameObject obj_to_deactivate)
    {
        second_object_to_activate = obj_to_deactivate;
    }

    public void SetObjectToDeactivate(GameObject obj_to_deactivate)
    {
        object_to_deactivate = obj_to_deactivate;
    }

    public void SwipeLeft(GameObject obj_to_activate)
    {
        if (currently_fading)
            return;

        currently_fading = true;
        object_to_activate = obj_to_activate;
        this.GetComponent<Animator>().Play("SwipeLeft");
    }

    public void ActivateDeactiveObjects()
    {
        if (object_to_activate)
            object_to_activate.gameObject.SetActive(true);
        if (second_object_to_activate)
            second_object_to_activate.gameObject.SetActive(true);
        if (object_to_deactivate)
            object_to_deactivate.gameObject.SetActive(false);

        currently_fading = false;
        object_to_activate = null;
        second_object_to_activate = null;
        object_to_deactivate = null;
    }
}
