using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HasHealth : MonoBehaviour
{
    // Start is called before the first frame update
    
    private bool invincible = false;
    public bool iframe = false;
    public Death deadFunc;
    public char type;
    public Inventory inventory;
    public ArrowKeyMovement movement;
    public Rigidbody rb;

    private void Update()
    {
        invincible = inventory.godmode;
        rb = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
   
    
    public void TakeDamage(char direction = 'k')
    {
        if (!invincible && !iframe)
        {

            inventory.AddHearts(-1);
            movement.OnlyMoveToggle();
            iframe = true;
            Debug.Log("tried");
            StartCoroutine(FlashColorFilter());
            StartCoroutine(Iframes());
            newKnockback(direction);

            StartCoroutine(Knockback(direction));

            
            Debug.Log("worked");
            if (inventory.GetHearts() == 0)
            {
                deadFunc.HandleDeath(type);
            }
        }
    }


    IEnumerator Iframes() {

        
        yield return StartCoroutine(CoroutineUtilities.IframeOverTime(1.0f));
        iframe = false;
        //movement.OnlyMoveToggle();


    }

    IEnumerator FlashColorFilter()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float totalFlashDuration = 2.0f;
        float flashInterval = 0.4f;
        float elapsedTime = 0.0f;

        while (elapsedTime < totalFlashDuration)
        {
            spriteRenderer.material.color = new Color(0.5f, 0.0f, 0.0f, 1f);
            yield return new WaitForSeconds(flashInterval / 3);

            spriteRenderer.material.color = new Color(1f, 0.8f, 0.8f, 1f);
            yield return new WaitForSeconds(flashInterval / 3);

            spriteRenderer.material.color = new Color(0.5f, 0.5f, 1f, 1f);
            yield return new WaitForSeconds(flashInterval / 3);

            elapsedTime += flashInterval;
        }

        spriteRenderer.material.color = new Color(1f, 1f, 1f, 1f);
    }


    IEnumerator Knockback(char direction)
    {

        /*Vector3 initial_position = transform.position;
        Vector3 final_position = new Vector3();
        if (direction == 'u')
        {
            final_position = new Vector3(initial_position.x, initial_position.y - 5, initial_position.z);
        }
        else if (direction == 'd')
        {
            final_position = new Vector3(initial_position.x, initial_position.y + 5, initial_position.z);
        }
        else if (direction == 'r')
        {
            final_position = new Vector3(initial_position.x - 5, initial_position.y, initial_position.z);
        }
        else if (direction == 'l')
        {
            final_position = new Vector3(initial_position.x + 5, initial_position.y, initial_position.z);
        }


        yield return StartCoroutine(
            CoroutineUtilities.MoveObjectOverTime(transform, initial_position, final_position, .25f)
            );
        movement.OnlyMoveToggle();
        yield return null;*/

        yield return StartCoroutine(CoroutineUtilities.IframeOverTime(.3f));
        
        movement.OnlyMoveToggle();
    }

    void newKnockback(char direction) {

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