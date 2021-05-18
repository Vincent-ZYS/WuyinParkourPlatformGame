using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBgTheme : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ManagerVars vars;

    private void Awake()
    {
        RandomInitiateGameBgTheme();
    }

    /// <summary>
    /// Randomly iniate Game Background image sprte function.
    /// </summary>
    private void RandomInitiateGameBgTheme()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        vars = ManagerVars.GetManagerVarsContainer();
        int randSpriteIndex = Random.Range(0, vars.bgThemeSpriteList.Count);
        spriteRenderer.sprite = vars.bgThemeSpriteList[randSpriteIndex];
    }
}
