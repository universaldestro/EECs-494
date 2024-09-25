using UnityEngine;
using System.Collections;

public class FragmentProjectile : MonoBehaviour
{
    public float speed = 8f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void StartDestructionCountdown()
    {
        Destroy(gameObject, 0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // should i be changing the block name or prefab name or create new tags
        //  if (other.CompareTag("WALL") || other.gameObject.name.Equals("Tile_WALL"))
        if (other.CompareTag("WALL"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("enemy"))
        {
            Movement enemy = other.gameObject.GetComponent<Movement>();
            if (enemy != null)
            {
                enemy.TakeDamage(1, 'r');
            }
            Destroy(gameObject);
        }
    }
}
