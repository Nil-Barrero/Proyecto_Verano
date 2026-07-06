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

        [Header("Movement Variables")]
        private float move = 0.0f;
        private Vector2 velocity;
        [SerializeField] private float moveVelocity;
        [Range(0, 1)][SerializeField] private float linearDamping;
        [SerializeField] private float jumpForce = 5.0f;
        [SerializeField] private bool isGrounded;
        [SerializeField] private LayerMask layer;

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
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

    if (hit.collider != null)
    {
        Debug.Log("Golpeaste: " + hit.collider.name);
        Destroy(hit.collider.gameObject);
    }
    }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundManager.position, groundBoxSize);
        }
    }  
}
