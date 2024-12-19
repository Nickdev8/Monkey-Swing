using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileKeyboard : MonoBehaviour
{
    void Update()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
        WebGLInput.mobileKeyboardSupport = true;
    }
}
