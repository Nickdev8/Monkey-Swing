using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpriteList", menuName = "Sprite List")]
public class SpriteList : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
}
