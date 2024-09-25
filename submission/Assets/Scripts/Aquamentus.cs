using UnityEngine;
using System.Collections;

public class AquamentusMovement : Movement
{
    public float moveSpeed = 2f; // Speed of movement
    public float movementRange = 5f; // Total range of movement (distance from the initial local position to either side)
    public GameObject projectilePrefab; // The projectile prefab to fire
    public Transform firePoint; // The point from which the projectiles will be fired
    public float attackInterval = 3f; // Interval between attacks
    public Transform player; // Reference to the player

    private float leftBoundary; // Left boundary in local space
    private float rightBoundary; // Right boundary in local space
    private bool movingRight = true; // Direction of movement

    void Start()
    {
        // Store the initial local position of the object
        Vector3 initialLocalPosition = transform.localPosition;

        // Calculate boundaries based on the initial local position and movement range
        leftBoundary = initialLocalPosition.x - movementRange;
        rightBoundary = initialLocalPosition.x + movementRange;

        // Start the attack coroutine
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        if(canMove)
        Move();
    }

    public override void Move()
    {
        if (movingRight)
        {
            transform.localPosition += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.localPosition.x >= rightBoundary)
            {
                movingRight = false; // Reverse direction
            }
        }
        else
        {
            transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.localPosition.x <= leftBoundary)
            {
                movingRight = true; // Reverse direction
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            if(canMove)
            FireProjectiles();
        }
    }

    private void FireProjectiles()
    {
        if (player == null) return;

        Vector3 playerPosition = player.position;
        Vector3 firePosition = firePoint.position;

        // Calculate the directions for the three projectiles
        Vector2 directionToPlayer = (playerPosition - firePosition).normalized;
        Vector2 upperDirection = (playerPosition + new Vector3(0, 1, 0) - firePosition).normalized;
        Vector2 lowerDirection = (playerPosition + new Vector3(0, -1, 0) - firePosition).normalized;

        // Instantiate and initialize the projectiles
        InstantiateProjectile(directionToPlayer);
        InstantiateProjectile(upperDirection);
        InstantiateProjectile(lowerDirection);
    }

    private void InstantiateProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<AquaProjectile>().Initialize(direction);
    }
}