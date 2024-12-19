using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public Image m_Image;
    public Sprite DeUnMute;
    public Sprite UnMute;
    public AudioSource AS;

    public void OnClick()
    {
        if(PlayerPrefs.GetInt("Mute") == 0)
        PlayerPrefs.SetInt("Mute", 1);
        
        else if(PlayerPrefs.GetInt("Mute") == 1)
        PlayerPrefs.SetInt("Mute", 0);
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("Mute") == 1)
        {
            AS.mute = true;
            m_Image.sprite = UnMute;
        }
        else
        {
            AS.mute = false;
            m_Image.sprite = DeUnMute;
        }
    }
}
