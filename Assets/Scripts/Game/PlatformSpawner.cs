using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    /// <summary>
    /// The initiation spawn position.(Manualy configure)
    /// </summary>
    public Vector3 startSpawnPos;

    /// <summary>
    /// The count of spawning platform.
    /// </summary>
    private int spawnPlatformCount;
    /// <summary>
    /// Next platform spawning position.
    /// </summary>
    private Vector3 spawnPlatformPosition;
    /// <summary>
    /// The flag (siwtch) to judge spawn left or not.
    /// </summary>
    private bool isLeftSpawan = false;
    /// <summary>
    /// The configuration parameter management container.
    /// </summary>
    private ManagerVars varsContainer;
    /// <summary>
    /// Randomly got the platform theme sprite.
    /// </summary>
    private Sprite chosenPlatformSprite;

    private void Awake()
    {
        EventCenter.AddListner(EventType.DecidePath, DecidePath);
        varsContainer = ManagerVars.GetManagerVarsContainer();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.DecidePath, DecidePath);
    }

    private void Start()
    {
        RandomlyChoosePlatformTheme();
        InitiatePlatformGroup();
        SpawnClassicPlayerCharacter();
    }

    private void InitiatePlatformGroup()
    {
        spawnPlatformPosition = startSpawnPos;
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }
    }

    private void DecidePath()
    {
        if(spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
        }else
        {
            isLeftSpawan = !isLeftSpawan;
            spawnPlatformCount = Random.Range(1, 4);
        }
        SpawnPlatform();
    }

    private void SpawnPlatform()
    {
        InitiateSinglePlatform();
        if(isLeftSpawan)
        {
            //Spawn platform on left hand side.
            spawnPlatformPosition = new Vector3(spawnPlatformPosition.x - varsContainer.nextPosX,
                spawnPlatformPosition.y + varsContainer.nextPosY,0f);
        }else
        {
            //Spawn platform on right hand side.
            spawnPlatformPosition = new Vector3(spawnPlatformPosition.x + varsContainer.nextPosX,
                spawnPlatformPosition.y + varsContainer.nextPosY,0f);
        }
    }

    private void InitiateSinglePlatform()
    {
        GameObject platformGo = Object.Instantiate(varsContainer.normalPlatformGo, transform);
        platformGo.GetComponent<PlatformController>().SinglePlatformThemeSpriteInitation(chosenPlatformSprite);
        platformGo.transform.position = spawnPlatformPosition;
    }

    private void RandomlyChoosePlatformTheme()
    {
        int randomIndex = Random.Range(0, varsContainer.platformThemeSpriteList.Count);
        chosenPlatformSprite = varsContainer.platformThemeSpriteList[randomIndex];
    }

    private void SpawnClassicPlayerCharacter()
    {
        GameObject playerPrefabGo = Object.Instantiate(varsContainer.PlayerCharacter);
        playerPrefabGo.transform.position = varsContainer.PlayerInitialSpawnPosition;
    }
}
