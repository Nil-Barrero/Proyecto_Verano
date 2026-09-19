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

        //Debug.Log(_enemiesDeads);

        short condition = _zoneManager.GetWaveEnemiesRequired();
        if (_enemiesDeads >= condition)
        {
            _enemiesDeads = 0;
            _zoneManager.NextWave();
        }
    }
}
