using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class Controller : MonoBehaviour
    {
        public static Controller instance;
        [Header("Character Components")]
        private GameObject character;
        private Rigidbody2D rb;
        [SerializeField] private Transform groundManager;
        [SerializeField] private Vector2 groundBoxSize;
        [SerializeField] private float KnockbackForce;
        [SerializeField] private LayerMask KnockbackLayer;

        [Header("Movement Variables")]
        private float move = 0.0f;
        private Vector2 velocity;
        [SerializeField] private float moveVelocity;
        [Range(0, 1)][SerializeField] private float linearDamping;
        [SerializeField] private float jumpForce = 5.0f;
        [SerializeField] private bool isGrounded;
        [SerializeField] private LayerMask layer;

        [Header("Gun Variables")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawn;
        [Range(0.1f, 1f)][SerializeField] private float fireRate = 0.5f;

     [Header("Shoot & Aim Variable")]
     private Vector3 mousePos;
     private Transform crosshair;


        private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal") * moveVelocity;
        isGrounded = Physics2D.OverlapBox(groundManager.position, groundBoxSize, 0.0f, layer);

        AimMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

        private void FixedUpdate()
        {
            Movement(move * Time.deltaTime);
        }

        private void Movement(float move)
        {
            Vector2 objectiveVel = new Vector2(move, rb.linearVelocityY);
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, objectiveVel, ref velocity, linearDamping);
        }

        private void Jump()
        {
            if(isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }      
        }

        private void AimMouse()
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;

            //crosshair.position = mousePos;
        }

        private void Shoot()
        {
            //Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            GameObject bullet = PoolingManager.instance.GetInstanceOfClass("Bullet");
            bullet.transform.position = bulletSpawn.position;
            bullet.transform.rotation = bulletSpawn.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().spawner = this.gameObject;
            bullet.GetComponent<Bullet>().SetLayer("PlayerBullet");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundManager.position, groundBoxSize);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D contact = collision.GetContact(0);
           
            Knockback(collision.gameObject, contact.normal);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Vector2 direction = this.transform.position - collision.transform.position;
            Knockback(collision.gameObject, direction);
        }

        void Knockback(GameObject other, Vector2 dir)
        {
            //Activar cuando el jugador tenga vida
            //if (GetComponent<HealthBehaviour>().IsInvincible()) return;

            if (((1 << other.gameObject.layer) & KnockbackLayer) != 0)
            {
                rb.linearVelocity = Vector2.zero;

                Vector2 direction;
                if (Mathf.Abs(dir.normalized.y) > 0.7f)
                {
                    float side = transform.position.x >= other.transform.position.x ? 1 : -1;
                    //La latura esta harcodeada para que empieze un poco más arriba
                    direction = new Vector2(side, 0.3f);
                }
                else
                    direction = dir;

                    rb.AddForce(direction.normalized * KnockbackForce, ForceMode2D.Impulse);
            }
        }
    }  
}
