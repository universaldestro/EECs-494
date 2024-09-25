using UnityEngine;
using System.Collections;

public class Gel : Movement
{
    public float moveSpeed = 1f; // Gel movement speed
    public float changeDirectionTime = 2f; // Time to change direction
    public int maxHealth = 1; // Health for the Gel

    private Vector2 movementDirection; // Direction of movement
    private Rigidbody rb2d; // Reference to Rigidbody2D
    private int currentHealth; // Current health

    void Start()
    {
        rb2d = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
    {
        // Move Gel in the current direction
        if (canMove) { 
            Move();
        }
        
    }

    public override void Move()
    {
        rb2d.velocity = movementDirection * moveSpeed;
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (currentHealth > 0)
        {
            // Randomly choose to move either horizontally or vertically
            if (Random.value > 0.5f)
            {
                // Move horizontally
                movementDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                // Move vertically
                movementDirection = Random.Range(0, 2) == 0 ? Vector2.up : Vector2.down;
            }

            // Wait for a while before changing direction
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reverse direction on collision with walls or obstacles
        movementDirection = -movementDirection;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Handle Gel death
            Die();
        }
    }

    private void Die()
    {
        // Add death animation or effects here
        Debug.Log("Gel is defeated!");
        Destroy(gameObject);
    }

    
}