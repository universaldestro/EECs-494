using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomEventPushableBlock : MonoBehaviour
{
    // Start is called before the first frame update
    Animator doorAnimator;
    public GameObject Block;
    Door door;
    Vector3 BlockInit = new Vector3(7,5,0.1f);

    public ArrowKeyMovement player;
    void Start()
    {
      doorAnimator = GetComponent<Animator>();
        door = GetComponent<Door>();
        


    }

    private void ResetRoom()
    { 
        Block.transform.localPosition = BlockInit;
        doorAnimator.Play("Base Layer.Locked_left");
        door.tag = "locked_door";
    }

    // Update is called once per frame
    void Update()
    {
        if (Block.transform.localPosition == new Vector3(6, 5, Block.transform.localPosition.z))
        {
            doorAnimator.Play("Base Layer.Unlocked_left");
            door.tag = "door";
        }
        if (player.currentRoom == "Room (2,3)" || player.currentRoom == "Room (1,2)") {
            ResetRoom();
            Debug.Log("woorks");
        }
        

    }
}
