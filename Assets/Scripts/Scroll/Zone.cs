using UnityEngine;

/// <summary>
/// El siguiente script debe de situarse en el gameobject que se encarga de llevar cada sección
/// </summary>

public class Zone : MonoBehaviour
{
    [Header("Zone Type")]
    public ZoneType _type = ZoneType.NORMAL;

    [Header("Size")]
    public float _width = 0, _height = 0;

    private bool _isActive = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }

    public void ResetZone()
    {
        _isActive = false;
        //Reset the things to do
    }

    public float EndX => transform.position.x + (_width/2f);
    public float StartX => transform.position.x - (_width/2f);
}
