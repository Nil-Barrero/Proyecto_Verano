using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker instance;

    public ZoneManager _zoneManager;

    private short _enemiesDeads;

    private void Awake() { instance = this; }

    public void AddEnemyDead()
    {
        _enemiesDeads++;

        short condition = _zoneManager.GetWaveEnemiesRequired();
        if (condition == _enemiesDeads)
        {
            _enemiesDeads = 0;
            _zoneManager.NextWave();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
