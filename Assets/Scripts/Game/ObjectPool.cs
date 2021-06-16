using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton pattern to control this simple gameobject object pool.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    private const int initSpawnSingleObjectCount = 5;
    private List<GameObject> normalPlatformGoList = new List<GameObject>();
    private List<GameObject> commonPlatformGroupGoList = new List<GameObject>();
    private List<GameObject> winterPlatformGroupGoList = new List<GameObject>();
    private List<GameObject> forestPlatformGroupGoList = new List<GameObject>();
    private List<GameObject> SpikePlatformGroupGoList = new List<GameObject>();
    private ManagerVars varsContainer;
    private int spawnIndex;
    private bool isHavingSpawnIndex = false;
    private bool isSpawningSpike = false;
    private bool isSpikeGroupListFilled = false;
    private GameObject spawningSpikeGo;
    private PlatformGroupType curGroupTypeTheme;
    public PlatformGroupType CurGroupTypeTheme { set { curGroupTypeTheme = value; } }

    private bool isLeftSpawnSpike = false;
    public bool IsLeftSpawnSpike { set { isLeftSpawnSpike = value; } }

    public static ObjectPool Instance()
    {
        if(_instance == null)
        {
            _instance = FindObjectOfType<ObjectPool>();
        }
        return _instance;
    }

    private void Awake()
    {
        varsContainer = ManagerVars.GetManagerVarsContainer();
    }

    private void Start()
    {
        ObjectPoolInit();
    }

    private void ObjectPoolInit()
    {
        InitGoListForPool(ref normalPlatformGoList, varsContainer.normalPlatformGo, null);
        InitGoListForPool(ref SpikePlatformGroupGoList, null, varsContainer.spikePlatformGroupList);
        InitGoListForPool(ref commonPlatformGroupGoList, null, varsContainer.commonPlatformGroupList);
        switch (curGroupTypeTheme)
        {
            case PlatformGroupType.common:
                InitGoListForPool(ref commonPlatformGroupGoList, null, varsContainer.commonPlatformGroupList);
                break;
            case PlatformGroupType.winter:
                InitGoListForPool(ref winterPlatformGroupGoList, null, varsContainer.winterPlatformGroupList);
                break;
            case PlatformGroupType.forest:
                InitGoListForPool(ref forestPlatformGroupGoList, null, varsContainer.forestPlatformGroupList);
                break;
            case PlatformGroupType.fire:
                InitGoListForPool(ref commonPlatformGroupGoList, null, varsContainer.commonPlatformGroupList);
                break;
        }
    }

    /// <summary>
    /// Initiate the specific gameobject from the specific varscontainer's gameobject or group for the specific pool list.
    /// </summary>
    /// <param name="poolList"></param>
    /// <param name="varsGo"></param>
    /// <param name="varsGoList"></param>
    /// <returns></returns>
    private GameObject InitGoListForPool(ref List<GameObject> poolList, GameObject varsGo = null, List<GameObject> varsGoList = null, bool isOtherNewSpikePool = false)
    {
        GameObject objectGo = null;
        if (varsGo != null && varsGoList == null)
        {
            for(int i = 0; i < initSpawnSingleObjectCount; i++)
            {
                objectGo = Instantiate(varsGo, transform);
                objectGo.SetActive(false);
                poolList.Add(objectGo);
            }
        }else if(varsGo == null && varsGoList != null)
        {
            for(int i = 0; i < initSpawnSingleObjectCount; i++)
            {
                for(int j = 0; j < varsGoList.Count; j++)
                {
                    objectGo = Instantiate(varsGoList[j], transform);
                    objectGo.SetActive(false);
                    poolList.Add(objectGo);
                }
            }
            if(isOtherNewSpikePool)
            {
                isSpawningSpike = false;
                return spawningSpikeGo;
            }
        }
        return objectGo;
    }

    /// <summary>
    /// Get specific platform object in specific poollist, and also remember to leave the specific varscontainer's gameobject or group list.
    /// </summary>
    /// <param name="poolList"></param>
    /// <param name="varsGo"></param>
    /// <param name="varsGoList"></param>
    /// <returns></returns>
    public GameObject GetSpecifiPlatformInPool(GameObject varsGo = null, List<GameObject> varsGoGroupList = null)
    {
        List<GameObject> poolList = FindAppropriateGoList(varsGo, varsGoGroupList);
        bool isOtherNewSpikePool = false;
        if(isSpawningSpike)
        {
            isSpawningSpike = false;
            if(!isSpikeGroupListFilled)
            {
                //if the current spike group object pool list is not filled, it can return specific spike game object.
                return spawningSpikeGo;
            }
            isSpikeGroupListFilled = false;
            isOtherNewSpikePool = true;
        }
        else
        {
            for (int i = 0; i < poolList.Count; i++)
            {
                if (isHavingSpawnIndex && i != spawnIndex)
                {
                    continue;
                }
                if (poolList[i].activeInHierarchy == false)
                {
                    return poolList[i];
                }
            }
            isOtherNewSpikePool = false;
        }
        return InitGoListForPool(ref poolList, varsGo, varsGoGroupList, isOtherNewSpikePool);
    }

    private List<GameObject> FindAppropriateGoList(GameObject varsGo = null, List<GameObject> varsGoGroupList = null)
    {
        List<GameObject> finalGoList = null;
        if (varsGo != null)
        {
            finalGoList = normalPlatformGoList;
            isHavingSpawnIndex = false;
        }
        else if(varsGoGroupList!=null)
        {
            isHavingSpawnIndex = true;
            if (varsGoGroupList == varsContainer.commonPlatformGroupList)
            {
                finalGoList = commonPlatformGroupGoList;
                spawnIndex = Random.Range(0, commonPlatformGroupGoList.Count);
            }
            else if(varsGoGroupList == varsContainer.winterPlatformGroupList)
            {
                finalGoList = winterPlatformGroupGoList;
                spawnIndex = Random.Range(0, winterPlatformGroupGoList.Count);
            }
            else if(varsGoGroupList == varsContainer.forestPlatformGroupList)
            {
                finalGoList = forestPlatformGroupGoList;
                spawnIndex = Random.Range(0, forestPlatformGroupGoList.Count);
            }
            else if(varsGoGroupList == varsContainer.spikePlatformGroupList)
            {
                finalGoList = SpikePlatformGroupGoList;
                FindAppropriateSpikeGroup();
            }
        }
        return finalGoList;
    }

    private void FindAppropriateSpikeGroup()
    {
        isSpawningSpike = true;
        for (int i = 0; i < SpikePlatformGroupGoList.Count; i++)
        {
            if (SpikePlatformGroupGoList[i].activeInHierarchy == false)
            {
                isSpikeGroupListFilled = false;
                if (isLeftSpawnSpike && (i == 0 || i % 2 == 0))
                {
                    spawningSpikeGo = SpikePlatformGroupGoList[i];
                    break;
                }
                else if (!isLeftSpawnSpike && ((i + 1) % 2 == 0))
                {
                    spawningSpikeGo = SpikePlatformGroupGoList[i];
                    break;
                }
                else
                {
                    //To notice the current spike object pool group list is filled or not.
                    isSpikeGroupListFilled = true;
                }
            }
            isSpikeGroupListFilled = true;
        }
    }
}
