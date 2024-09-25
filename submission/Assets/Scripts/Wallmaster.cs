using UnityEngine;
using System.Collections;

public class Wallmaster : Movement
{
    public float moveSpeed = 5f; // Speed at which the Wallmaster moves
    public float detectionRange = 10f; // Range in which the Wallmaster can detect the player
    public float resetTime = 2f; // Time delay before resetting position
    public LayerMask playerLayer; // Layer to detect the player
    public Transform returnPosition; // Position to return the player to upon grabbing

    private Vector3 initialPosition; // Initial position for reset
    private Rigidbody rb;
    private bool isMoving = false;
    private Transform playerTransform;
    ArrowKeyMovement player;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            DetectPlayer();
        }
    }

    private void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        if (hitColliders.Length > 0)
        {
            isMoving = true;
            Debug.Log("Player detected, Wallmaster is moving.");
        }
    }

    void Update()
    {
        if (isMoving && canMove)
        {
            Move();
        }
        else {
            ResetWallmaster();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Tile_WALL")
        {
            gameObject.layer = 4;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Water"), LayerMask.NameToLayer("Default"), true); // Ignore collisions with walls;
        }
        else
        {
            gameObject.layer = 0;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Water"), LayerMask.NameToLayer("Default"), false); // Ignore collisions with walls;}
        }
    }

    public override void Move()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

        // If Wallmaster reaches the player
        if (Vector3.Distance(transform.position, playerTransform.position) <= 1f)
        {
            GrabPlayer();

        }
    }

    private void GrabPlayer()
    {
        Debug.Log("Player grabbed by Wallmaster!");

        playerTransform.position = returnPosition.position; // Return player to start
        player.WallmasterReset();
        StartCoroutine(ResetWallmaster());
    }

    private IEnumerator ResetWallmaster()
    {
        isMoving = false;
        yield return new WaitForSeconds(resetTime);
        rb.position = initialPosition;
        Debug.Log("Wallmaster reset to initial position.");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection range for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}