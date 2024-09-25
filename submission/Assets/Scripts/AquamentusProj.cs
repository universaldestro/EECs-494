using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    public int damage = 1; // Damage dealt to the player

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }


}