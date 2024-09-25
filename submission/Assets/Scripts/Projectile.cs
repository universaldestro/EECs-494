using UnityEngine;

public class AquaProjectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    public int damage = 1; // Damage dealt to the player

    private Vector2 direction; // Direction of the projectile's movement

    // Initialize the projectile with a direction
    public void Initialize(Vector2 direction)
    {
        this.direction = -direction.normalized;

        // Rotate the projectile to face the direction it is moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    void Update()
    {
        // Move the projectile in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("enemy")) { 

        Destroy(gameObject);
        }
            
    }
}