using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVarsContainer()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }

    public List<Sprite> bgThemeSpriteList = new List<Sprite>();

    /// <summary>
    /// The platform next spawn position.
    /// </summary>
    public float nextPosX = 0.55f, nextPosY = 0.64f;
    
    /// <summary>
    /// The normal platform gameobject prefab.
    /// </summary>
    public GameObject normalPlatformGo;
}
