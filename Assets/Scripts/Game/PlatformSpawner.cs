using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlatformGroupType
{
    common,
    forest,
    fire,
    winter,
    spike
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
    private Vector3 nextSpawnPlatformPosition;
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
    private PlatformGroupType curGroupType;
    /// <summary>
    /// Spawn the platform after the spike's spawn.
    /// </summary>
    private int spawnSpikesPlatformCount;
    /// <summary>
    /// The position of current spawned platform after spike spawned.
    /// </summary>
    private Vector3 spikeSpawnedPlatformPos;
    /// <summary>
    /// The flag to judge is the platform currently spawning on spike-group or not.
    /// </summary>
    private bool isSpawningSpikePlatform = false;
    /// <summary>
    /// The flag to judge the spike-group is on current platform left-hand side or not.
    /// </summary>
    private bool isSpikeOnLeftHandSide = false;

    public int MileStoneScore;
    public float fallTime;
    public float minFallTime;
    public float fallTimeMultiply;

    private void Awake()
    {
        EventCenter.AddListner(EventType.DecidePath, DecidePath);
        varsContainer = ManagerVars.GetManagerVarsContainer();
        RandomlyChoosePlatformTheme();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.DecidePath, DecidePath);
    }

    private void Start()
    {
        InitiateBeginPlatform();
        SpawnClassicPlayerCharacter();
    }

    private void Update()
    {
        if(GameManager.Instance().isGameStart && !GameManager.Instance().isGameOver && !GameManager.Instance().isGamePause)
        {
            UpdateFallPlatform();
        }
    }

    private void UpdateFallPlatform()
    {
        if(GameManager.Instance().playerScore >= MileStoneScore)
        {
            MileStoneScore *= 2;
            fallTime *= fallTimeMultiply;
            if(fallTime <= minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }

    private void InitiateBeginPlatform()
    {
        nextSpawnPlatformPosition = startSpawnPos;
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
            //Spawn the combined theme platform. And it will also spawn the spike platform.
            int randNum = Random.Range(0, 3);
            if(randNum == 0)//spawn common theme platform
            {
                InitiateCustomizedPlatformGroup(varsContainer.commonPlatformGroupList);
            }else if(randNum == 1)//spawn different theme's platform.
            {
                switch(curGroupType)
                {
                    case PlatformGroupType.common:
                        InitiateCustomizedPlatformGroup(varsContainer.commonPlatformGroupList);
                        break;
                    case PlatformGroupType.winter:
                        InitiateCustomizedPlatformGroup(varsContainer.winterPlatformGroupList);
                        break;
                    case PlatformGroupType.forest:
                        InitiateCustomizedPlatformGroup(varsContainer.forestPlatformGroupList);
                        break;
                    case PlatformGroupType.fire:
                        InitiateCustomizedPlatformGroup(varsContainer.commonPlatformGroupList);
                        break;
                }
            }else //spawn spike trap combination
            {
                spawnSpikesPlatformCount = 4;
                isSpawningSpikePlatform = true;
                GameObject spikePlatformGroupGo = InitiateCustomizedPlatformGroup(varsContainer.spikePlatformGroupList, true);
                Transform spikeTf = spikePlatformGroupGo.transform.Find("Obstacle").transform;
                SpawnPlatformAfterSpikeInitiated(spikeTf);
            }
        }
        //Time to spawan diamond randomly
        int rdSpwDiamondNum = Random.Range(0, 15);
        if(rdSpwDiamondNum == 5 && GameManager.Instance().isGameStart)
        {
            GameObject diamondGo = ObjectPool.Instance().GetDiamondGoInPool(varsContainer.diamondItemGo);
            diamondGo.transform.position = new Vector3(nextSpawnPlatformPosition.x, nextSpawnPlatformPosition.y + 0.5f, 0f);
            diamondGo.SetActive(true);
        }
        //To create a new position for next platform.
        if (isLeftSpawan)
        {
            //Spawn platform on left hand side.
            nextSpawnPlatformPosition = new Vector3(nextSpawnPlatformPosition.x - varsContainer.nextPosX,
                nextSpawnPlatformPosition.y + varsContainer.nextPosY,0f);
        }else
        {
            //Spawn platform on right hand side.
            nextSpawnPlatformPosition = new Vector3(nextSpawnPlatformPosition.x + varsContainer.nextPosX,
                nextSpawnPlatformPosition.y + varsContainer.nextPosY,0f);
        }
    }

    private void InitiateSingleNormalPlatform()
    {
        GameObject platformGo = ObjectPool.Instance().GetSpecifiPlatformInPool(varsContainer.normalPlatformGo, null);
        platformGo.SetActive(true);
        platformGo.GetComponent<PlatformController>().SinglePlatformInitation(chosenPlatformSprite, fallTime);
        platformGo.transform.position = nextSpawnPlatformPosition;
    }

    private GameObject InitiateCustomizedPlatformGroup(List<GameObject> platformGroupThemeList, bool isSpawnSpike = false)
    {
        if(isSpawnSpike)
        {
            ObjectPool.Instance().IsLeftSpawnSpike = !isLeftSpawan;
        }
        GameObject platformGroupGo = ObjectPool.Instance().GetSpecifiPlatformInPool(null, platformGroupThemeList);
        platformGroupGo.SetActive(true);
        platformGroupGo.GetComponent<PlatformController>().SinglePlatformInitation(chosenPlatformSprite, fallTime);
        platformGroupGo.transform.position = nextSpawnPlatformPosition;
        if(!isSpawnSpike && isLeftSpawan && platformGroupGo.transform.Find("Obstacle") != null)
        {
            platformGroupGo.transform.Find("Obstacle").transform.localPosition = new Vector3(1.1f, 0f, 0f);
        }else if(!isSpawnSpike && !isLeftSpawan && platformGroupGo.transform.Find("Obstacle") != null)
        {
            platformGroupGo.transform.Find("Obstacle").transform.localPosition = new Vector3(-1.1f, 0f, 0f);
        }
        return platformGroupGo;
    }

    private void RandomlyChoosePlatformTheme()
    {
        int randomIndex = Random.Range(0, varsContainer.platformThemeSpriteList.Count);
        chosenPlatformSprite = varsContainer.platformThemeSpriteList[randomIndex];
        switch(randomIndex)
        {
            case 0:
                curGroupType = PlatformGroupType.common;
                break;
            case 1:
                curGroupType = PlatformGroupType.forest;
                break;
            case 2:
                curGroupType = PlatformGroupType.winter;
                break;
            case 3:
                curGroupType = PlatformGroupType.fire;
                break;
        }
        ObjectPool.Instance().CurGroupTypeTheme = curGroupType; //Pass the platform theme type to Object Pool.
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
                GameObject platformGo = ObjectPool.Instance().GetSpecifiPlatformInPool(varsContainer.normalPlatformGo, null);
                //Object.Instantiate(varsContainer.normalPlatformGo, transform);
                platformGo.GetComponent<PlatformController>().SinglePlatformInitation(chosenPlatformSprite, fallTime);
                platformGo.SetActive(true);
                if (i==0)//spawn the platform on orginal direction. It should be the contray way with spike one.
                {
                    platformGo.transform.position = nextSpawnPlatformPosition;
                    if (isSpikeOnLeftHandSide)
                    {
                        nextSpawnPlatformPosition = new Vector3(nextSpawnPlatformPosition.x + varsContainer.nextPosX,
                            nextSpawnPlatformPosition.y + varsContainer.nextPosY, 0f);
                    }else
                    {
                        nextSpawnPlatformPosition = new Vector3(nextSpawnPlatformPosition.x - varsContainer.nextPosX,
                            nextSpawnPlatformPosition.y + varsContainer.nextPosY, 0f);
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
