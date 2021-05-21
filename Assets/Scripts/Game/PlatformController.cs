using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public SpriteRenderer[] PlatformSpriteRenderer;

    public void SinglePlatformThemeSpriteInitation(Sprite sprite)
    {
        for(int i=0; i < PlatformSpriteRenderer.Length; i++)
        {
            PlatformSpriteRenderer[i].sprite = sprite;
        }
    }

}
