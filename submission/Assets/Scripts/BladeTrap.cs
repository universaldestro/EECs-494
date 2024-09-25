using UnityEngine;

public class BladeTrap : Movement
{
    public float moveSpeed = 10f;  // Speed at which the Blade Trap moves towards the player
    public float resetSpeed = 15f; // Speed at which the Blade Trap returns to its initial position
    public float detectionRange = 5f;  // Detection range for the raycast
    public float rayDebugDuration = 1f;  // Duration for how long rays should be visible
    public LayerMask playerLayer;  // Layer mask to specify which layers to detect
    public float detectionCooldown = 1f; // Cooldown time between detections

    private Vector3 initialPosition;  // Initial position for reset
    private Vector3 targetPosition;  // Target position to move towards
    private Rigidbody rb;
    private bool isMoving = false;
    private bool playerDetected = false;
    private Vector3 moveDirection = Vector3.zero;
    private float lastDetectionTime = 0f;  // Last time the player was detected
    private Vector3 detectedDirection;  // Direction that detected the player

    // Directions for raycasting along the X and Y axes
    private readonly Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            DetectPlayer();
        }
        else
        {
            if (canMove)
            {
                Move();
                if (playerDetected && !IsPlayerInDetectionRange())
                {
                    // Player has moved out of the detection range, move back to the initial position immediately
                    playerDetected = false;
                    StartMovingTowardsInitialPosition();
                }
            }
        }

        // Draw debugging rays for visualization
        DrawDebugRays();
    }

    private void DetectPlayer()
    {
        if (Time.time < lastDetectionTime + detectionCooldown)
        {
            return; // Skip detection if still within the cooldown period
        }

        RaycastHit hit;
        foreach (Vector3 direction in directions)
        {
            bool hitDetected = Physics.Raycast(transform.position, direction, out hit, detectionRange, playerLayer);
            Debug.DrawRay(transform.position, direction * detectionRange, hitDetected ? Color.green : Color.red, rayDebugDuration);

            if (hitDetected && hit.collider.CompareTag("Player"))
            {
                targetPosition = hit.collider.transform.position;
                detectedDirection = direction;

                // Determine the correct axis for movement
                if (direction == Vector3.up || direction == Vector3.down)
                {
                    moveDirection = new Vector3(0, Mathf.Sign(targetPosition.y - transform.position.y), 0);
                }
                else if (direction == Vector3.left || direction == Vector3.right)
                {
                    moveDirection = new Vector3(Mathf.Sign(targetPosition.x - transform.position.x), 0, 0);
                }

                playerDetected = true;
                lastDetectionTime = Time.time;  // Update the last detection time
                StartMovingTowardsTarget();
                return;  // Exit the loop once the player is detected
            }
        }

        playerDetected = false;  // Reset detection flag if no player was detected
    }

    private bool IsPlayerInDetectionRange()
    {
        RaycastHit hit;
        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange, playerLayer)
                && hit.collider.CompareTag("Player"))
            {
                // Player is still in detection range
                return true;
            }
        }
        // Player is not detected in any direction
        return false;
    }

    private void StartMovingTowardsTarget()
    {
        isMoving = true;
        rb.velocity = moveDirection * moveSpeed;
    }

    private void StartMovingTowardsInitialPosition()
    {
        isMoving = true;
        Vector3 returnDirection = (initialPosition - transform.position).normalized;
        rb.velocity = returnDirection * resetSpeed;
    }

    public override void Move()
    {
        if (playerDetected)
        {
            if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
            {
                rb.velocity = Vector3.zero;
                isMoving = false; // Stop moving when the target is reached
            }
        }
        else
        {
            // Clamp the position to ensure it doesn't overshoot the initial position
            Vector3 clampedPosition = transform.position;
            if (detectedDirection == Vector3.up || detectedDirection == Vector3.down)
            {
                clampedPosition.y = Mathf.Clamp(clampedPosition.y, Mathf.Min(initialPosition.y, targetPosition.y), Mathf.Max(initialPosition.y, targetPosition.y));
            }
            else if (detectedDirection == Vector3.left || detectedDirection == Vector3.right)
            {
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, Mathf.Min(initialPosition.x, targetPosition.x), Mathf.Max(initialPosition.x, targetPosition.x));
            }

            transform.position = clampedPosition;

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                rb.velocity = Vector3.zero;
                isMoving = false; // Stop moving when the initial position is reached
            }
        }
    }

    private void DrawDebugRays()
    {
        foreach (Vector3 direction in directions)
        {
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red, rayDebugDuration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}