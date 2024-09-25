using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalfosMovement : Movement
{  
    public Rigidbody rb;
    public BoxCollider stalfosCollider;
    public float movementSpeed = 3f;
    
    public GameObject keyPrefab;
    
    private Vector2 currentDirection;
    private bool isMoving = false;
    private float gridSize = 1f;

    public bool hasKey = false;

    public Sprite deathSprite1;
    public Sprite deathSprite2;
    public Sprite deathSprite3;
    public Sprite deathSprite4;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stalfosCollider = GetComponent<BoxCollider>();
         health = 1;


    stalfosCollider.size = new Vector3(2.0f, 2.0f, 0.0f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
    {
        Move();
            
            //Debug.Log("Debug:::  " + transform.parent.gameObject.name);
        
    }

    public override void Move()
    {
        if (isMoving && canMove)
        {
            Vector3 targetPosition = rb.position + (Vector3)(currentDirection * gridSize);
            rb.position = Vector3.MoveTowards(rb.position, targetPosition, movementSpeed * Time.deltaTime);
            if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = new Vector3(
                    Mathf.Round(rb.position.x / gridSize) * gridSize,
                    Mathf.Round(rb.position.y / gridSize) * gridSize,
                    rb.position.z);
                isMoving = false;
            }
        }
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            ChooseRandomDirection();
            isMoving = true;

            yield return new WaitForSeconds(Random.Range(1f, 3f));


            rb.velocity = Vector2.zero;  // Stop movement
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    void ChooseRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                currentDirection = Vector2.up;
                direction = 'u';
                break;
            case 1:
                currentDirection = Vector2.down;
                direction = 'd';
                break;
            case 2:
                currentDirection = Vector2.left;
                direction = 'l';
                break;
            case 3:
                currentDirection = Vector2.right;
                direction = 'r';
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.position = new Vector3(
            Mathf.Round(rb.position.x / gridSize) * gridSize,
            Mathf.Round(rb.position.y / gridSize) * gridSize,
            rb.position.z);
        ChooseRandomDirection();
    }

    public override void TakeDamage(int amount,char direction)
    {
        health -= amount;
        //newKnockback(direction);
        if (health <= 0)
        {
            if (health <= 0)
            {
                StartCoroutine(FlickerOnDeath());
            }
        }
    }


    IEnumerator FlickerOnDeath()
    {
        
        stalfosCollider.enabled = false;
        isMoving = false;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float flickerDuration = 0.2f;
        //float flickerInterval = 0.2f;
        //float elapsedTime = 0.0f;

        spriteRenderer.material.color = new Color(0.5f, 0.0f, 0.0f, 1f);
        yield return new WaitForSeconds(flickerDuration);

        spriteRenderer.material.color = new Color(1f, 1f, 1f, 1f);

        spriteRenderer.sprite = deathSprite1;
        // i dont wanna delete this transform but it is currently not doing what i want it to do
        //transform.localScale = new Vector3(0.7f, 0.7f, 1);
        yield return new WaitForSeconds(flickerDuration);

        spriteRenderer.sprite = deathSprite2;
        yield return new WaitForSeconds(flickerDuration);

        spriteRenderer.sprite = deathSprite3;
        yield return new WaitForSeconds(flickerDuration);

        spriteRenderer.sprite = deathSprite4;
        yield return new WaitForSeconds(flickerDuration);


        if (hasKey)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log("Stalfo BYE");
        Destroy(gameObject);
    }
    /*
    IEnumerator Knockback(char direction)
    {

        Vector3 initial_position = transform.position;
        Vector3 final_position = new Vector3();
        float knockbackAmount = 0.0f;
        if (direction == 'u')
        {
            final_position = new Vector3(initial_position.x, initial_position.y - knockbackAmount, initial_position.z);
        }
        else if (direction == 'd')
        {
            final_position = new Vector3(initial_position.x, initial_position.y + knockbackAmount, initial_position.z);
        }
        else if (direction == 'r')
        {
            final_position = new Vector3(initial_position.x - knockbackAmount, initial_position.y, initial_position.z);
        }
        else if (direction == 'l')
        {
            final_position = new Vector3(initial_position.x + knockbackAmount, initial_position.y, initial_position.z);
        }

        yield return StartCoroutine(
            CoroutineUtilities.MoveObjectOverTime(transform, initial_position, final_position, .25f)
            );

        yield return null;

    }
    */
    
    void newKnockback(char direction)
    {

        float knockVal = 2;
        if (direction == 'u')
        {
            //final_position = new Vector3(initial_position.x, initial_position.y - 5, initial_position.z);
            rb.velocity = new Vector2(0, -knockVal);
        }
        else if (direction == 'd')
        {
            // final_position = new Vector3(initial_position.x, initial_position.y + 5, initial_position.z);
            rb.velocity = new Vector2(0, knockVal);
        }
        else if (direction == 'r')
        {
            //final_position = new Vector3(initial_position.x - 5, initial_position.y, initial_position.z);
            rb.velocity = new Vector2(-knockVal, 0);
        }
        else if (direction == 'l')
        {
            //final_position = new Vector3(initial_position.x + 5, initial_position.y, initial_position.z);
            rb.velocity = new Vector2(knockVal, 0);
        }
    }
}