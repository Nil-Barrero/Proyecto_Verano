using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingClass
{
    public GameObject prefabToPool;
    public string poolName;
    public int poolAmount = 0;
    public bool createOtherIfExceded = false;
    public List<GameObject> pools = new List<GameObject>();
    public GameObject container { private set; get; }
    public void SetContainer(GameObject newContainer) { this.container = newContainer; }

}
public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;
    public List<PoolingClass> poolers = new List<PoolingClass>();
    private void Awake()
    {
        instance = this;
        foreach (PoolingClass pooler in poolers)
        {
            GameObject container = new GameObject(pooler.poolName);
            container.transform.SetParent(this.transform);
            container.name = pooler.poolName;
            pooler.SetContainer(container);
            pooler.pools.Clear();
            for (int i = 0; i < pooler.poolAmount; i++)
            {
                GameObject instantiated = GameObject.Instantiate(pooler.prefabToPool, container.transform);
                instantiated.SetActive(false);
                pooler.pools.Add(instantiated);
            }
        }
    }
    public GameObject GetInstanceOfClass(string className)
    {
        foreach (PoolingClass pooler in poolers)
        {
            if (pooler.poolName == className)
            {
                foreach (GameObject instance in pooler.pools)
                {
                    if (!instance.activeInHierarchy)
                        return instance;
                }
                if (pooler.createOtherIfExceded)
                {
                    GameObject instantiated = GameObject.Instantiate(pooler.prefabToPool, pooler.container.transform);
                    pooler.pools.Add(instantiated);
                    return instantiated;
                }

            }
        }
        return null;
    }
}