using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomTransitions : MonoBehaviour
{
    // Start is called before the first frame update
    public ArrowKeyMovement player;
    private Rigidbody rg;
    private bool moving = false;
    public roomTransitionsCamera camTransition;
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        Vector3 South1 = new Vector3(8, 1, 0);
        Vector3 South2 = new Vector3(7, 1, 0);
        Vector3 North1 = new Vector3(8, 9, 0);
        Vector3 North2 = new Vector3(7, 9, 0);
        Vector3 East = new Vector3(14, 5, 0);
        Vector3 West = new Vector3(1, 5, 0);
        camTransition.MoveCamera(other);
        Vector3 playerPosition = player.gameObject.transform.position;

        if (!moving && !other.gameObject.CompareTag("locked_door")) { 
            
            if ((other.gameObject.transform.localPosition == South1 || other.gameObject.transform.localPosition == South2) && other.gameObject.CompareTag("door")) {
                player.ToggleMovement();
                moving = true;
                StartCoroutine(RoomTransition(0, -3.5f));
                
                moving = true;
            }
            else if ((other.gameObject.transform.localPosition == North1 || other.gameObject.transform.localPosition == North2) && other.gameObject.CompareTag("door")) {
                player.ToggleMovement();
                moving = true;
                StartCoroutine(RoomTransition(0, 3.5f));
            
                
                }
            else if (other.gameObject.transform.localPosition == East && other.gameObject.CompareTag("door"))
                {
                player.ToggleMovement();
                Debug.Log("started");
                    moving = true;
                StartCoroutine(RoomTransition(3.5f,0));
            
                
                }
            else if (other.gameObject.transform.localPosition == West && other.gameObject.CompareTag("door")) {


                player.ToggleMovement();
                    moving = true;
                    StartCoroutine(RoomTransition(-3.5f, 0));
            
                }

          


        }
    }


    IEnumerator RoomTransition(float x ,float y ) {
            Vector3 initial_position = transform.position;
            Vector3 final_position = new Vector3(initial_position.x+x, initial_position.y+y, initial_position.z);
            yield return StartCoroutine(
                CoroutineUtilities.MoveObjectOverTime(transform, initial_position, final_position, 3f)
                );
        player.ToggleMovement();
        moving = false;
            yield return null;  
    }


}


