using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton pattern to control this simple gameobject object pool.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public const int initSpawnSingleObjectCount = 5;
    public List<GameObject> normalPlatformGoList = new List<GameObject>();
    public List<GameObject> commonPlatformGroupGoList = new List<GameObject>();
    public List<GameObject> winterPlatformGroupGoList = new List<GameObject>();
    public List<GameObject> forestPlatformGroupGoList = new List<GameObject>();
    public List<GameObject> SpikePlatformGroupGoList = new List<GameObject>();
    public ManagerVars varsContainer;
    private int spawnIndex;
    private bool isHavingSpawnIndex = false;
    private bool isSpawningSpike = false;
    private GameObject spawningSpikeGo;

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
        ObjectPoolInit();
    }

    private void ObjectPoolInit()
    {
        InitGoListForPool(ref normalPlatformGoList, varsContainer.normalPlatformGo, null);
        InitGoListForPool(ref commonPlatformGroupGoList, null, varsContainer.commonPlatformGroupList);
        InitGoListForPool(ref winterPlatformGroupGoList, null, varsContainer.winterPlatformGroupList);
        InitGoListForPool(ref forestPlatformGroupGoList, null, varsContainer.forestPlatformGroupList);
        InitGoListForPool(ref SpikePlatformGroupGoList, null, varsContainer.spikePlatformGroupList);
    }

    /// <summary>
    /// Initiate the specific gameobject from the specific varscontainer's gameobject or group for the specific pool list.
    /// </summary>
    /// <param name="poolList"></param>
    /// <param name="varsGo"></param>
    /// <param name="varsGoList"></param>
    /// <returns></returns>
    private GameObject InitGoListForPool(ref List<GameObject> poolList, GameObject varsGo = null, List<GameObject> varsGoList = null)
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
        if(isSpawningSpike)
        {
            isSpawningSpike = false;
            return spawningSpikeGo;
        }
        for (int i = 0; i < poolList.Count; i++)
        {
            if(isHavingSpawnIndex && i != spawnIndex)
            {
                continue;
            }
            if(poolList[i].activeInHierarchy == false)
            {
                return poolList[i];
            }
        }
        return InitGoListForPool(ref poolList, varsGo, varsGoGroupList);
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
                spawnIndex = Random.Range(1, commonPlatformGroupGoList.Count);
            }
            else if(varsGoGroupList == varsContainer.winterPlatformGroupList)
            {
                finalGoList = winterPlatformGroupGoList;
                spawnIndex = Random.Range(1, winterPlatformGroupGoList.Count);
            }
            else if(varsGoGroupList == varsContainer.forestPlatformGroupList)
            {
                finalGoList = forestPlatformGroupGoList;
                spawnIndex = Random.Range(1, forestPlatformGroupGoList.Count);
            }
            else if(varsGoGroupList == varsContainer.spikePlatformGroupList)
            {
                //TODO
                isSpawningSpike = true;
                finalGoList = SpikePlatformGroupGoList;
                for (int i = 0; i < SpikePlatformGroupGoList.Count; i++)
                {
                    if (SpikePlatformGroupGoList[i].activeInHierarchy == false)
                    {
                        if(isLeftSpawnSpike && (i==0 || i%2==0))
                        {
                            spawningSpikeGo = SpikePlatformGroupGoList[i];
                            break;
                        }else if(!isLeftSpawnSpike && ((i+1)%2==0))
                        {
                            spawningSpikeGo = SpikePlatformGroupGoList[i];
                            break;
                        }
                    }
                }
            }
        }
        return finalGoList;
    }
}
