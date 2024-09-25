using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 5f; // Speed of the boomerang
    public float returnTime = 1f; // Time before the boomerang starts returning
    public int damage = 1; // Damage dealt to the player

    private Rigidbody rb2d;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isReturning = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    public void Initialize(Vector2 direction)
    {
        targetPosition = new Vector2(direction.x, direction.y) * 5f; // Adjust as necessary
        rb2d.velocity = direction * speed;

        // Start return coroutine
        StartCoroutine(ReturnBoomerang());
    }

    private IEnumerator ReturnBoomerang()
    {
        yield return new WaitForSeconds(returnTime);
        isReturning = true;
        rb2d.velocity = (startPosition - (Vector2)transform.position).normalized * speed;
    }

    void Update()
    {
        if (isReturning)
        {
            rb2d.velocity = (startPosition - (Vector2)transform.position).normalized * speed;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
   
            Destroy(gameObject);
        
    }
}