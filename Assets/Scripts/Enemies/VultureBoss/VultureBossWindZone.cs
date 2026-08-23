using Character;
using UnityEngine;

public class VultureBoss_WindZone : MonoBehaviour
{
    [SerializeField] Vector2 windForce;
    Rigidbody2D playerRb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Controller>(out Controller c))
            playerRb = collision.attachedRigidbody;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Controller>(out Controller c))
            playerRb = null;
    }
    private void FixedUpdate()
    {
        if (playerRb)
            playerRb.AddForce(windForce);
    }
}