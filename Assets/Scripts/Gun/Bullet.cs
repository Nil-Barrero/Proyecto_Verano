using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [Range(1,10)][SerializeField] private float speed = 5.0f; 
    [Range(1,5)][SerializeField] private float lifeTime = 3.0f;
        private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);    
    }
}
