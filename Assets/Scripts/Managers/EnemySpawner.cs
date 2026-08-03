using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnPositionsData
{
    public string spawnPositionsID;
    public List<Transform> spawnPoints {  get; private set; }
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SpawnPositionsData spawnPositionsData;
}
