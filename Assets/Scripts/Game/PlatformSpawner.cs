using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    normal,
    forest,
    fire,
    winter
}

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
    /// <summary>
    /// The enum parameter to store platform group type.
    /// </summary>
    private PlatformGroupType groupType;
    /// <summary>
    /// Spawn the platform after the spike's spawn.
    /// </summary>
    private int spawnSpikesPlatformCount;
    /// <summary>
    /// The position of current spawned platform after spike spawned.
    /// </summary>
    private Vector3 spikeSpawnedPlatformPos;

    private bool isSpawningSpikePlatform = false;

    private bool isSpikeOnLeftHandSide = false;

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
        InitiateBeginPlatform();
        SpawnClassicPlayerCharacter();
    }

    private void InitiateBeginPlatform()
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
        if(isSpawningSpikePlatform)
        {
            AfterSpawnSpikeSpawnPlatform();
            return;
        }
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
        if(spawnPlatformCount > 0)
        {
            //Spawn the normal single platform.
            InitiateSingleNormalPlatform();
        }else if(spawnPlatformCount == 0)
        {
            //Spawn the combined theme platform. And it will also spawn the stab platform.
            int randNum = Random.Range(0, 3);
            if(randNum == 0)//spawn common theme platform
            {
                InitiateCustomizedPlatformGroup(varsContainer.normalPlatformGroupList);
            }else if(randNum == 1)//spawn different theme's platform.
            {
                switch(groupType)
                {
                    case PlatformGroupType.winter:
                        InitiateCustomizedPlatformGroup(varsContainer.winterPlatformGroupList);
                        break;
                    case PlatformGroupType.forest:
                        InitiateCustomizedPlatformGroup(varsContainer.forestPlatformGroupList);
                        break;
                    case PlatformGroupType.fire:
                        InitiateCustomizedPlatformGroup(varsContainer.normalPlatformGroupList);
                        break;
                }
            }else // spawn stab trap combination
            {
                spawnSpikesPlatformCount = 4;
                isSpawningSpikePlatform = true;
                GameObject spikePlatformGroupGo = InitiateCustomizedPlatformGroup(varsContainer.spikePlatformGroupList);
                Transform spikeTf = spikePlatformGroupGo.transform.Find("Obstacle").transform;
                SpawnPlatformAfterSpikeInitiated(spikeTf);
            }
        }
        if (isLeftSpawan)
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

    private void InitiateSingleNormalPlatform()
    {
        GameObject platformGo = Object.Instantiate(varsContainer.normalPlatformGo, transform);
        platformGo.GetComponent<PlatformController>().SinglePlatformThemeSpriteInitation(chosenPlatformSprite);
        platformGo.transform.position = spawnPlatformPosition;
    }

    private GameObject InitiateCustomizedPlatformGroup(List<GameObject> platformGroupThemeList)
    {
        int randomIndex = Random.Range(0, platformGroupThemeList.Count);
        GameObject platformGroupGo = Object.Instantiate(platformGroupThemeList[randomIndex], transform);
        platformGroupGo.GetComponent<PlatformController>().SinglePlatformThemeSpriteInitation(chosenPlatformSprite);
        platformGroupGo.transform.position = spawnPlatformPosition;
        return platformGroupGo;
    }

    private void RandomlyChoosePlatformTheme()
    {
        int randomIndex = Random.Range(0, varsContainer.platformThemeSpriteList.Count);
        chosenPlatformSprite = varsContainer.platformThemeSpriteList[randomIndex];
        switch(randomIndex)
        {
            case 0:
                groupType = PlatformGroupType.normal;
                break;
            case 1:
                groupType = PlatformGroupType.forest;
                break;
            case 2:
                groupType = PlatformGroupType.winter;
                break;
            case 3:
                groupType = PlatformGroupType.fire;
                break;
        }
    }

    private void SpawnClassicPlayerCharacter()
    {
        GameObject playerPrefabGo = Object.Instantiate(varsContainer.PlayerCharacter);
        playerPrefabGo.transform.position = varsContainer.PlayerInitialSpawnPosition;
    }

    private void SpawnPlatformAfterSpikeInitiated(Transform spikeTf)
    {
        isSpikeOnLeftHandSide = spikeTf.localPosition.x < 0;
        if (spikeTf.localPosition.x < 0)//spawn platform on spike's left
        {
            spikeSpawnedPlatformPos = new Vector3(spikeTf.position.x - varsContainer.nextPosX,
                spikeTf.position.y + varsContainer.nextPosY, 0f);
        }
        else if(spikeTf.localPosition.x > 0)//spawn platform on spike's right
        {
            spikeSpawnedPlatformPos = new Vector3(spikeTf.position.x + varsContainer.nextPosX,
                spikeTf.position.y + varsContainer.nextPosY, 0f);
        }
    }

    private void AfterSpawnSpikeSpawnPlatform()
    {
        if(spawnSpikesPlatformCount > 0)
        {
            spawnSpikesPlatformCount--;
            for(int i = 0; i < 2; i++)
            {
                GameObject platformGo = Object.Instantiate(varsContainer.normalPlatformGo, transform);
                platformGo.GetComponent<PlatformController>().SinglePlatformThemeSpriteInitation(chosenPlatformSprite);
                if (i==0)//spawn the platform on orginal direction. It should be the contray way with spike one.
                {
                    platformGo.transform.position = spawnPlatformPosition;
                    if (isSpikeOnLeftHandSide)
                    {
                        spawnPlatformPosition = new Vector3(spawnPlatformPosition.x + varsContainer.nextPosX,
                            spawnPlatformPosition.y + varsContainer.nextPosY, 0f);
                    }else
                    {
                        spawnPlatformPosition = new Vector3(spawnPlatformPosition.x - varsContainer.nextPosX,
                            spawnPlatformPosition.y + varsContainer.nextPosY, 0f);
                    }
                }else//spawn the platform on spike's platform direction
                {
                    platformGo.transform.position = spikeSpawnedPlatformPos;
                    if (isSpikeOnLeftHandSide)
                    {
                        spikeSpawnedPlatformPos = new Vector3(spikeSpawnedPlatformPos.x - varsContainer.nextPosX,
                            spikeSpawnedPlatformPos.y + varsContainer.nextPosY, 0f);
                    }
                    else
                    {
                        spikeSpawnedPlatformPos = new Vector3(spikeSpawnedPlatformPos.x + varsContainer.nextPosX,
                            spikeSpawnedPlatformPos.y + varsContainer.nextPosY, 0f);
                    }
                }
            }
        }else
        {
            isSpawningSpikePlatform = false;
            DecidePath();
        }
    }
}
