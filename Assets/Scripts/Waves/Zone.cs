using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// El siguiente script debe de situarse en el gameobject que se encarga de llevar cada sección
/// </summary>

public class Zone : MonoBehaviour
{
    //Actualmente no lo uso, en un futuro puede que le de uso, en caso de que no, me lo cargaré
    [Header("Zone Type")]
    public ZoneType _type = ZoneType.NORMAL;

    [Header("Size")]
    public float _width = 0, _height = 0;


    public Tilemap foreground, background, terrain;

    private bool _isActive = false;
    private readonly List<GameObject> _spawnedEnemies = new List<GameObject>();

    public GameObject _zoneDistanceX;

    private void Awake()
    {
        if(_zoneDistanceX != null)
            _zoneDistanceX.SetActive(false);
        
    }

    private void OnValidate()
    {
        if(_zoneDistanceX != null)
        {
            Vector3 scale = _zoneDistanceX.transform.localScale;
            scale.x = _width;
           // scale.x = terrain.size.x;
            _zoneDistanceX.transform.localScale = scale;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _width = terrain.size.x;
        _height = terrain.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveZone()
    {
        if (_isActive) return;
        _isActive = true;
        //Add the things to do
        SpawnEnemies();
    }

    public void ResetZone()
    {
        _isActive = false;
        //Reset the things to do
        foreach (GameObject enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                enemy.transform.SetParent(null);
                enemy.SetActive(false);
            }
        }
        _spawnedEnemies.Clear();
    }

    public void SpawnEnemies()
    {
        EnemySpawnPoint[] spawnPoints = GetComponentsInChildren<EnemySpawnPoint>(true);

        foreach (EnemySpawnPoint point in spawnPoints)
        {

            GameObject enemy = PoolingManager.instance.GetInstanceOfClass(point._poolName);

            if (enemy == null)
            {
                Debug.LogWarning($"Zone '{name}': no hay enemigos disponibles en el pool '{point._poolName}'.");
                continue;
            }

            enemy.transform.SetParent(point.transform, false);
            enemy.transform.localPosition = Vector3.zero;
            enemy.SetActive(true);
            _spawnedEnemies.Add(enemy);
        }
    }

    public float EndX => transform.position.x + (_width/2f);
    public float StartX => transform.position.x - (_width/2f);
}
