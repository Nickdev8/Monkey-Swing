using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    public bool music = true;
    void Awake()
    {
        if(music)
        {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
