using UnityEngine;
using System.Collections;

public class PushableBlock : MonoBehaviour
{
    public float moveDistance = 1f; // Distance the block moves per push (assuming 1 unit per tile)
    public float moveTime = 0.2f; // Time it takes to move to the next tile
    public LayerMask obstaclesLayer; // Layer to detect obstacles

    private bool isMoving = false;

    void OnCollisionStay(Collision collision)
    {
        if (!isMoving && collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDirection = GetPushDirection(collision.transform);
            if (pushDirection != Vector3.zero)
            {
                Vector3 targetPosition = transform.position + pushDirection * moveDistance;
                if (CanMoveTo(targetPosition))
                {
                    StartCoroutine(MoveBlock(targetPosition));
                }
            }
        }
    }

    private Vector3 GetPushDirection(Transform playerTransform)
    {
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return new Vector3(Mathf.Sign(direction.x), 0, 0);
        }
        else
        {
            return new Vector3(0, Mathf.Sign(direction.y), 0);
        }
    }

    private bool CanMoveTo(Vector3 targetPosition)
    {
        Collider[] hitColliders = Physics.OverlapBox(targetPosition, transform.localScale / 2, Quaternion.identity, obstaclesLayer);
        return hitColliders.Length == 0; // Return true if no obstacles are detected at the target position
    }

    private IEnumerator MoveBlock(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}