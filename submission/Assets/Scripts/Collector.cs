using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Collector : MonoBehaviour
{
    public AudioClip rupee_collection_sound_clip;
    public AudioClip rupee_collection_sound_clip2;
    public AudioClip key_collection_sound_clip;
    public AudioClip bomb_collection_sound_clip;
    public AudioClip heart_collection_sound_clip;
    public AudioClip unlockdoor_collection_sound_clip;
    public float audio_volume = 1.0f;
    Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        if(inventory == null)
        {
            Debug.LogWarning("WARNING: Gameobject with a collector has not inventory to store things in!");
        }
    }




    void OnTriggerEnter(Collider coll)
    {
        GameObject object_collided_with = coll.gameObject;

        if (object_collided_with.tag == "rupee")
        {
            if (inventory != null)
                inventory.AddRupees(1);
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(rupee_collection_sound_clip, transform.position, audio_volume);
            AudioSource.PlayClipAtPoint(rupee_collection_sound_clip2, transform.position, audio_volume);

        }
        else if (object_collided_with.tag == "key")
        {
            if (inventory != null)
                inventory.AddKeys(1);
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(key_collection_sound_clip, transform.position, audio_volume);
        }
        else if (object_collided_with.tag == "bomb")
        {
            if (inventory != null)
                inventory.AddBombs(1);
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(bomb_collection_sound_clip, transform.position, audio_volume);
        }
        else if (object_collided_with.tag == "heart")
        {
            if (inventory != null)
                inventory.AddHearts(1);
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(heart_collection_sound_clip, transform.position, audio_volume);
        }
        

    }
    private void OnCollisionEnter(Collision coll)
    {


        GameObject object_collided_with = coll.gameObject;
        if (object_collided_with.tag == "locked_door")
        {
            
            Door door = object_collided_with.GetComponent<Door>();


            
            if (inventory != null && inventory.GetKeys() > 0)
            {
                
                inventory.AddKeys(-1);
                UnlockDoor(object_collided_with,door);

                AudioSource.PlayClipAtPoint(unlockdoor_collection_sound_clip, transform.position, audio_volume);
            }
        }

    }

    void UnlockDoor(GameObject object_collided_with,Door door)
    {
        Animator doorAnimator = door.GetComponent<Animator>();
        



        switch (door.doorType)
        {
            
            case Door.DoorType.TopLeft:
            case Door.DoorType.TopRight:
                {
                    GameObject[] doors = GameObject.FindGameObjectsWithTag("locked_door");
                    for (int i = 0; i < doors.Length; ++i)
                    {

                        if (doors[i].gameObject.transform.position == object_collided_with.transform.position || (doors[i].transform.position.y == object_collided_with.transform.position.y && Math.Abs(object_collided_with.transform.position.x - doors[i].transform.position.x) == 1) )
                        {
                           
                                doors[i].GetComponent<Door>().GetComponent<Animator>().Play("Base Layer.Unlock");
                                //doorAnimator.Play("Base Layer.Unlock");
                                doors[i].tag = "door";
                                
                        }
                    }
                    //doorAnimator.SetTrigger("UnlockTopLeft");
                    //

                   // doorAnimator.SetTrigger("UnlockTopRight");
                    break;
                }
            case Door.DoorType.Right:
                doorAnimator.SetTrigger("UnlockRight");
                doorAnimator.Play("Base Layer.Unlocked_right");
                door.tag = "door";
                break;
            case Door.DoorType.Left:
                doorAnimator.SetTrigger("UnlockLeft");
                doorAnimator.Play("Base Layer.Unlocked_left");
                door.tag = "door";
                break;
        }
    }
}
