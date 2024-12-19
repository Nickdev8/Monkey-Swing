using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinLoader : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Image imageRenderer;
    public Slider slider;

    public SpriteList spriteList;

    public static int spriteIndex = 0;

    void Update()
    {
        if (spriteList == null || spriteList.sprites.Count == 0)
        {
            Debug.LogWarning("The SpriteList is missing or empty. Create a SpriteList Scriptable Object and add sprites to it.");
        }
        else
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = spriteList.sprites[spriteIndex];
            }
            else
            {
                slider.maxValue = spriteList.sprites.Count - 1;
                int m_SpriteIndex = Convert.ToInt32(slider.value);
                if (m_SpriteIndex != spriteIndex)
                {
                    m_SpriteIndex = spriteIndex;
                    slider.value = spriteIndex;
                }
                imageRenderer.sprite = spriteList.sprites[spriteIndex];
            }
        }
    }

    public void ValueChanged()
    {
        spriteIndex = Convert.ToInt32(slider.value);
    }
}
