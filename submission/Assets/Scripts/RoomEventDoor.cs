using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEventDoor : MonoBehaviour
{
    public int numChildren;
    bool done = false;
    public GameObject door;
    public ArrowKeyMovement player;
    private bool locked = false;
    public Sprite lockSprite;
    private Sprite defSprite;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentRoom == transform.name && !locked) {
            //lock door
            door.transform.tag = "locked_door";
            locked = true;
            door.GetComponent<SpriteRenderer>().sprite = lockSprite;

        }
        


        if (numChildren == 0 && !done)
        {

            //animation to be played unlock door
            door.GetComponent<SpriteRenderer>().sprite = defSprite;
            door.transform.tag = "door";
            
            done = true;

        }
    }
}
