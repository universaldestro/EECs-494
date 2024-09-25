using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEventKey : MonoBehaviour
{
    public int  numChildren ;
    bool done = false;
    public GameObject keyPrefab;
    public Vector3 spawn;
    public AudioClip keyAppearClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (numChildren == 0 && ! done) {
            AudioSource.PlayClipAtPoint(keyAppearClip, transform.position, 1);
            Instantiate(keyPrefab,spawn, Quaternion.identity);
            done = true;
            
        }
    }

    


}
