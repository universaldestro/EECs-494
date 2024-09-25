using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    private Ray ray;
    public LayerMask layersToHit;

    // Start is called before the first frame update
    void Start()
    {

        

        CheckForColliders();
    }
    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        /* Here you are drawing the first ray, you did not have a duration set
        for this one */
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue, 10f);
    }

    void CheckForColliders()
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name + " was hit ");

            // Here you are drawing the line again.
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 10f);
        }
    }
}
