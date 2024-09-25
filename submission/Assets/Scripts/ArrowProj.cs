using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProj : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private bool willDisperse = false;
    public GameObject swordFragmentPrefab;

    public Sprite fragmentSpriteRightUp;
    public Sprite fragmentSpriteLeftUp;
    public Sprite fragmentSpriteLeftDown;
    public Sprite fragmentSpriteRightDown;


    public void SetDirection(Vector3 dir, bool disperse = false)
    {
        direction = dir.normalized;
        willDisperse = disperse;

    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (willDisperse)
        {
            willDisperse = false;
            StartCoroutine(DisperseSword());
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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


    public IEnumerator DisperseSword()
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);

        Vector3[] directions = {
        new Vector3(1, 1, 0).normalized,   // 45
        new Vector3(-1, 1, 0).normalized,  // 135
        new Vector3(-1, -1, 0).normalized, // 215
        new Vector3(1, -1, 0).normalized   // -45
        };

        Sprite[] fragmentSprites = {
            fragmentSpriteRightUp,
            fragmentSpriteLeftUp,
            fragmentSpriteLeftDown,
            fragmentSpriteRightDown
        };

        for (int i = 0; i < directions.Length; i++)
        {
            GameObject fragment = Instantiate(swordFragmentPrefab, transform.position, Quaternion.identity);
            FragmentProjectile fragmentProjectile = fragment.GetComponent<FragmentProjectile>();
            fragmentProjectile.SetDirection(directions[i]);

            SpriteRenderer fragmentRenderer = fragment.GetComponent<SpriteRenderer>();
            fragmentRenderer.sprite = fragmentSprites[i];

            fragmentProjectile.StartDestructionCountdown();
        }
    }
}