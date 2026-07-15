using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Header("Waves")]
    public List<Wave> _waves;

    [Header("Movement")]
    public float _speed = 3f;

    [Header("Camera")]
    public Camera _camera;

    public const short _zonesToActive = 3;

    private readonly List<Zone> _activeZones = new List<Zone>();
    private short _zonesIndex = 0;
    private short _waveIndex = 0;
    private bool _canAdvance;

    private void Awake()
    {
        if(_camera == null)
            _camera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _waveIndex = 0;
        _zonesIndex = 0;

        SpawnZone(_waves[_waveIndex]._zones[_zonesIndex], 0f);

        if (_activeZones.Count < _zonesToActive)
            NextZone();

        _activeZones[0].ActiveZone();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Zone zone in _activeZones)
        {
            zone.transform.position += Vector3.left * (_speed * Time.deltaTime); 
        }

        foreach(Zone zone in _activeZones)
            zone.ActiveZone();

        RemoveZone();

        if(_activeZones.Count < _zonesToActive)
            NextZone();
    }

    void RemoveZone()
    {
        if(_activeZones.Count > 0 && _activeZones[0].EndX < (_camera.transform.position.x - (_camera.orthographicSize * _camera.aspect)))
        {
            Zone i = _activeZones[0];
            i.ResetZone();
            _activeZones.RemoveAt(0);
            Destroy(i.gameObject);
        }
    }

    void NextZone()
    {
        bool shouldAdvance = _canAdvance || !_waves[_waveIndex].requiresCondition;

        if(shouldAdvance)
        {
            _canAdvance = false;
            _waveIndex++;
            _zonesIndex = 0;

            if(_waveIndex >= _waves.Count)
            {
                //End of level
                _waveIndex = (short)(_waves.Count - 1);
            }

            SpawnZone(_waves[_waveIndex]._zones[0], _activeZones[_activeZones.Count - 1].EndX);
        }
        else
        {
            _zonesIndex = (short)((_zonesIndex + 1) % _waves[_waveIndex]._zones.Count);
            SpawnZone(_waves[_waveIndex]._zones[_zonesIndex], _activeZones[_activeZones.Count - 1].EndX);
        }
    }

    void SpawnZone(Zone prefab, float x)
    {
        Zone instance = Instantiate(prefab, new Vector3((x + (prefab._width/2f)), 0f, 0f), Quaternion.identity);
        instance.ResetZone();
        _activeZones.Add(instance);
    }

    public void NextWave() { _canAdvance = true; }

    public short GetWaveEnemiesRequired() { return _waves[_waveIndex].enemiesRequired; }
}
