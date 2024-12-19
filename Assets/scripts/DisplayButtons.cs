using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayButtons : MonoBehaviour
{
    public List<GameObject> Buttons;
    public List<GameObject> NonButtons;

    void Update()
    {
        foreach(GameObject button in Buttons)
        {
            if(Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
        }

        foreach(GameObject nonbutton in NonButtons)
        {
            if(Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
            {
                nonbutton.SetActive(false);
            }
            else
            {
                nonbutton.SetActive(true);
            }
        }
    }
}
