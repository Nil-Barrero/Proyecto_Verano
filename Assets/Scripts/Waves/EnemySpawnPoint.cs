using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    public string _poolName;


    public GameObject _imageTile;

    public void Awake()
    {
        if( _imageTile != null )
            _imageTile.SetActive( false );
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
