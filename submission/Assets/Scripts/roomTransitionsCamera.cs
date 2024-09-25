using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomTransitionsCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public ArrowKeyMovement player;
    
    private bool moving = false;

    private void Start()
    {
        Screen.SetResolution(1024,960, true);
        gameObject.transform.position = new Vector3(   (float)(39.5),(float)7, -10  );
    }

    public void MoveCamera(Collision other) {

        OnCollisionEnter(other);
    }

    
    private void OnCollisionEnter(Collision other)
    {

        Vector3 South1 = new Vector3(8, 1, 0);
        Vector3 South2 = new Vector3(7, 1, 0);
        Vector3 North1 = new Vector3(8, 9, 0);
        Vector3 North2 = new Vector3(7, 9, 0);
        Vector3 East = new Vector3(14, 5, 0);
        Vector3 West = new Vector3(1, 5, 0);

        if (!moving && !other.gameObject.CompareTag("locked_door")) { 
            
            if ((other.gameObject.transform.localPosition == South1 || other.gameObject.transform.localPosition == South2) && other.gameObject.CompareTag("door")) {
                moving = true;
                StartCoroutine(RoomTransition(0, -11));
        }
        else if ((other.gameObject.transform.localPosition == North1 || other.gameObject.transform.localPosition == North2) && other.gameObject.CompareTag("door")) {
            moving = true;
            StartCoroutine(RoomTransition(0, 11));
            }
        else if (other.gameObject.transform.localPosition == East && other.gameObject.CompareTag("door")) {
           
    
                moving = true;
            StartCoroutine(RoomTransition(16,0));
            
                
            }
        else if (other.gameObject.transform.localPosition == West && other.gameObject.CompareTag("door")) { 
                StartCoroutine(RoomTransition(-16, 0));
                moving = true;
            }
        }
        
    }
    IEnumerator RoomTransition(float x ,float y ) {

        
            Vector3 initial_position = transform.position;
            Vector3 final_position = new Vector3(initial_position.x+x, initial_position.y+y,-10);
        yield return StartCoroutine(
            CoroutineUtilities.MoveObjectOverTime(transform, initial_position, final_position, 2.5f)
            );
            moving = false;
            yield return null;
        
    }


}


