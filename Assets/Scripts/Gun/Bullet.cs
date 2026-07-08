using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [Range(1,10)][SerializeField] private float speed = 5.0f; 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);   
        }
        Destroy(this.gameObject); 
        Debug.Log("Colision con: " + collision.gameObject.name);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);       
    }
}
