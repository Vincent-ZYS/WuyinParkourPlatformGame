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

    /// <summary>
    /// The background image theme sprite to store the theme resource.
    /// </summary>
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();

    /// <summary>
    /// The platform theme Sprite List to store the theme resource.
    /// </summary>
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();

    /// <summary>
    /// The player's character prefab gameobject.
    /// </summary>
    public GameObject PlayerCharacter;

    /// <summary>
    /// The player's character initial spawn position.
    /// </summary>
    public Vector3 PlayerInitialSpawnPosition;

    /// <summary>
    /// The platform next spawn position.
    /// </summary>
    public float nextPosX = 0.55f, nextPosY = 0.64f;

    /// <summary>
    /// The normal platform gameobject prefab.
    /// </summary>
    public GameObject normalPlatformGo;

    /// <summary>
    /// The dead effect game object of player.
    /// </summary>
    public GameObject playerDeadEffectGo;

    /// <summary>
    /// Several relevant platform game object group list.
    /// </summary>
    public List<GameObject> commonPlatformGroupList = new List<GameObject>();

    public List<GameObject> forestPlatformGroupList = new List<GameObject>();

    public List<GameObject> winterPlatformGroupList = new List<GameObject>();

    public List<GameObject> spikePlatformGroupList = new List<GameObject>();
}
