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
    private Vector3 spawnPlatformPosition;
    private bool isLeftSpawan = false;
    private ManagerVars varsContainer;

    private void Start()
    {
        varsContainer = ManagerVars.GetManagerVarsContainer();
        spawnPlatformPosition = startSpawnPos;
        for(int i = 0; i < 5; i++)
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
        GameObject platformGo = Object.Instantiate(varsContainer.normalPlatformGo, transform);
        platformGo.transform.position = spawnPlatformPosition;
        if(isLeftSpawan)
        {
            //Spawn platform on left hand side.
            spawnPlatformPosition = new Vector3(platformGo.transform.position.x - varsContainer.nextPosX, 
                platformGo.transform.position.y + varsContainer.nextPosY,0f);
        }else
        {
            //Spawn platform on right hand side.
            spawnPlatformPosition = new Vector3(platformGo.transform.position.x + varsContainer.nextPosX,
                platformGo.transform.position.y + varsContainer.nextPosY,0f);
        }
    }
}
