using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class KeeseMovement : Movement
{
    public float moveSpeed = 2f; 
    public float changeDirectionTime = 2f; 
    private Vector2 movementDirection; 
    private Rigidbody rb;
    Animator anim;
   
    
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ChangeDirectionRoutine());
        anim = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
        health = 1;
        
    }

    void Update()
    {
        // Move Keese in the current direction
        //todo implement Keese animation
        
        
       if(canMove) Move();
    }

    public override void Move()
    {
        rb.velocity = movementDirection * moveSpeed;
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            
            movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

           
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        movementDirection = -movementDirection;
       
    }

}