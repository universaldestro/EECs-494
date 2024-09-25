using UnityEngine;
using System.Collections;

public class Goriya : Movement
{
    public float moveSpeed = 2f; // Goriya movement speed
    public float changeDirectionTime = 2f; // Time to change direction
    public GameObject boomerangPrefab; // The boomerang to be thrown
    public Transform shootingPoint; // The point from where the boomerang is thrown
    public float attackInterval = 3f; // Interval between attacks
    private Transform player; // Reference to the player

    private Vector2 movementDirection; // Direction of movement
    private Rigidbody rb2d; // Reference to Rigidbody2D
    private bool isMoving = true; // Check if the Goriya is moving

    void Start()
    {
        rb2d = GetComponent<Rigidbody>();
        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(AttackRoutine());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        
        if (isMoving && canMove)
        {
            Move();
        }
    }

    public override void Move()
    {
        rb2d.velocity = movementDirection * moveSpeed;
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // Randomly change direction
            movementDirection = GetRandomDirection();
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    private Vector2 GetRandomDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            case 3: return Vector2.right;
            default: return Vector2.up;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reverse direction on collision with walls or obstacles
        movementDirection = -movementDirection;
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            ThrowBoomerang();
        }
    }

    private void ThrowBoomerang()
    {
        if (boomerangPrefab != null && shootingPoint != null)
        {
            GameObject boomerang = Instantiate(boomerangPrefab, shootingPoint.position, Quaternion.identity);
            Vector2 direction = (player.position - shootingPoint.position).normalized;
            boomerang.GetComponent<Boomerang>().Initialize(direction);
            Debug.Log("Boomerang thrown!");
        }
    }
}