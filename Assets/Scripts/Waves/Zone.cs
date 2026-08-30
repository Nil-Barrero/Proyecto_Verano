using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public bool _disabledEnemies = false;
    private readonly List<GameObject> _spawnedEnemies = new List<GameObject>();

    public GameObject _zoneDistanceX;
    public bool isVisible;
    bool wasVisible;
    public UnityEvent onZoneAppear,onZoneDisappear;

    private void Awake()
    {
        if (_zoneDistanceX != null)
            _zoneDistanceX.SetActive(false);

        onZoneAppear.AddListener(ActiveZone);

    }

    private void OnValidate()
    {
        if (_zoneDistanceX != null)
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
        CalculateVisibility();
    }
    void CalculateVisibility()
    {
        float camW = Camera.main.orthographicSize * Camera.main.aspect;
        float camX = Camera.main.transform.position.x;
        isVisible = ((StartX <= camX + camW) && (EndX >= camX - camW));
        if (isVisible && !wasVisible)
            onZoneAppear.Invoke();
        else if (!isVisible && wasVisible)
            onZoneDisappear.Invoke();
        wasVisible = isVisible;
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
                //enemy.transform.SetParent(null);
                //enemy.SetActive(false);
            }
        }
        _spawnedEnemies.Clear();
    }

    public void SpawnEnemies()
    {
        if(!_disabledEnemies)
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

                //enemy.transform.SetParent(point.transform, false);
                enemy.GetComponent<Enemy>().assignedZoneReference = point.gameObject;
                //enemy.transform.localPosition = Vector3.zero;
                enemy.transform.position = point.transform.position;
                enemy.SetActive(true);
                _spawnedEnemies.Add(enemy);
            }
        }
    }

    public float EndX => transform.position.x + (_width/2f);
    public float StartX => transform.position.x - (_width/2f);
}
