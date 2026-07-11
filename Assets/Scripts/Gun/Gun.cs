using UnityEngine;

public class Gun : MonoBehaviour
{
     [Header("Shoot & Aim Variable")]
     private Vector3 mousePos;
     private Transform crosshair;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AimMouse();
    }

    private void AimMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        transform.localRotation = Quaternion.Euler(0,0,angle);
        
        //crosshair.position = mousePos;
    }
}
