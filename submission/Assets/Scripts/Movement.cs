using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    // This method should be overridden by all derived classes
    public bool canMove =false;
    public int health ;
    public char direction;
    public RoomEventKey parentEvent = null;
    public RoomEventDoor parentEvent2 = null;
    private bool killed = false;
    public abstract void Move();
    public AudioClip bow_collection_sound_clip;



    public virtual void TakeDamage(int amount, char direction)
    {
        health -= amount;
        
        //newKnockback(direction);
        if (health <= 0)
        {
            if (parentEvent != null && !killed) {
                killed = true;
                parentEvent.numChildren -= 1;
                
               
            }
            if (parentEvent2 != null && !killed) {
                killed = true;
                parentEvent.numChildren -= 1;
            }
            Destroy(gameObject);
        }
    }
}