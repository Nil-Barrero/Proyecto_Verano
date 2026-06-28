using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Character Components")]
    private GameObject character;
    private Rigidbody2D rb;

    [Header("Movement Variables")]
    private float move = 0.0f;
    private Vector2 velocity;
    [SerializeField] private float moveVelocity;
    [Range(0, 1)][SerializeField] private float linearDamping;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal") * moveVelocity;
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
}
