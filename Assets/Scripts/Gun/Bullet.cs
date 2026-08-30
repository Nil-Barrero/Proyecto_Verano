using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [Range(1,10)][SerializeField] private float speed = 5.0f;
    [Range(1,10)][SerializeField] private float knockbackForce = 2.0f;
    public GameObject spawner;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject != spawner) {
            if(collision.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
                hb.Damage();
            this.gameObject.SetActive(false); 
        }
        Debug.Log("Colision con: " + collision.gameObject.name);
    }

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
