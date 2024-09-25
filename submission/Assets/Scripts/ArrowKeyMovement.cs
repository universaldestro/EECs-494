using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Bow,
    Bomb
}

public class ArrowKeyMovement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody rb;
    public BoxCollider player;
    public float movement_speed = 5;
    public bool player_movement = true;
    public HasHealth health;
    public char direction;

    public Sprite idleRightFootForward, idleRightFootBackward, idleLeftFootForward, idleLeftFootBackward;
    public Sprite idleUpFootForward, idleUpFootBackward, idleDownFootForward, idleDownFootBackward;
    public Sprite swordRight, swordLeft, swordUp, swordDown;
    public Sprite arrowRight, arrowLeft, arrowUp, arrowDown;

    public Sprite swordProjectileSprite;

    public GameObject arrowPrefab;
    public GameObject swordProjectilePrefab;
    public GameObject swordPrefab;

    // For when bow is collected
    public GameObject bowPlayerPrefab;
    private GameObject bowPlayerInstance;
    private bool isFrozen = false;


    public GameObject bombPrefab;
    public float bombExplosionRadius = 2.0f;
    public float bombExplosionDelay = 2.0f;
    public LayerMask enemyLayerMask;
    public GameObject smokePrefab;
    private Animator bombanimator;

    public bool hasBow = false;

    private float spriteChangeTime = 0.1f;
    private float spriteTimer = 0f;
    private bool isRightFootForward = true;
    private float backwardsPrevention = 0.4f;

    private bool isThrusting = false;
    public float swordRange = 2.0f;
    public string currentRoom;
    public float raycastDistance = 10f; // Distance for the raycast
    public LayerMask layerMask;

    private bool canUseSword = true;
    private float swordCooldown = 0.5f;

    public Inventory inventory;

    public int rupeesPerArrow = 1;
    private float swordSeconds = 0.3f;

    public WeaponType currentWeapon = WeaponType.Sword;

    public AudioClip bow_collection_sound_clip, bow_collection_sound_clip2, bow_used_clip;
    public AudioClip sword_used_clip, sword_projectile_clip;
    public AudioClip bomb_used_clip;
    public AudioClip enemy_attack_clip;
    private AudioSource mainCameraAudioSource;

    private Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCameraAudioSource = mainCamera.GetComponent<AudioSource>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        player = GetComponent<BoxCollider>();

        player.size = new Vector3((float)0.67, (float)0.67, 0);
        currentRoom = "Room (2,0)";

        start = transform.position;

    }

    void Update()
    {

        PerformRaycast();


        if (!isFrozen && player_movement)
        {
            Vector2 current_input = GetInput();
            rb.velocity = current_input * movement_speed;

            if (current_input != Vector2.zero)
            {
                spriteTimer += Time.deltaTime;
                if (spriteTimer >= spriteChangeTime)
                {
                    spriteTimer = 0f;
                    isRightFootForward = !isRightFootForward;
                }
            }

            OnToggleWeapon();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] wallMasters = GameObject.FindGameObjectsWithTag("wallMaster");

        foreach (GameObject e in enemies)
        {
            Debug.Log(e);
            if ( e!= null && e.transform.parent != null && currentRoom == e.transform.parent.gameObject.name)
            {

                e.GetComponent<Movement>().canMove = true;
            }
            else
            {
                e.GetComponent<Movement>().canMove = false;
            }

        }

        foreach (GameObject e in wallMasters)
        {
            if (e.transform.parent != null && currentRoom == e.transform.parent.gameObject.name)
            {

                e.GetComponent<Movement>().canMove = true;
            }
            else
            {
                e.GetComponent<Movement>().canMove = false;
            }
        }
    }


    // Update is called once per frame

    //private void OnToggleWeapon() {
    //    if (Input.GetKeyDown(KeyCode.X))//Standard weapon
    //    {
    //        StandardorAltweapon = true;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Z)) 
    //    { //alt weapon
    //        StandardorAltweapon = false;

    //    }
    //    else if (Input.GetKeyDown(KeyCode.Space)) {
    //        StandardorAltweapon = !StandardorAltweapon;

    //    }
    //}

    private void OnToggleWeapon()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentWeapon == WeaponType.Sword)
            {
                isThrusting = true;
                StartCoroutine(ThrustSword());
            }
            else if (currentWeapon == WeaponType.Bow && hasBow)
            {
                if (inventory.GetRupees() >= rupeesPerArrow)
                {
                    inventory.AddRupees(-1);
                    StartCoroutine(ShootBow());
                }
                else
                {
                    Debug.Log("not enough rupees");
                }
            } else if (currentWeapon == WeaponType.Bomb)
                if (inventory.GetBombs() >= 1)
                {
                    AudioSource.PlayClipAtPoint(bomb_used_clip, transform.position, 1);
                    inventory.AddBombs(0);
                    StartCoroutine(PlaceBomb());
                }
        }
    }

    private void SwitchWeapon()
    {
        if (currentWeapon == WeaponType.Sword)
        {
            currentWeapon = WeaponType.Bomb;
            Debug.Log("Switched to bomb");
        }
        else if (currentWeapon == WeaponType.Bomb && hasBow)
        {
            currentWeapon = WeaponType.Bow;
            Debug.Log("Switched to bow");
        }
        else if (currentWeapon == WeaponType.Bow)
        {
            currentWeapon = WeaponType.Sword;
            Debug.Log("Switched to sword");
        }
    }

    private void PerformRaycast()
    {
        // Define the origin and direction of the ray
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.back; // Z-axis negative direction

        // Visualize the ray in the Unity Editor for debugging purposes
        Debug.DrawRay(origin, direction * raycastDistance, Color.green);

        // Perform the raycast
        RaycastHit hit;

        for (float i = 0; i < raycastDistance + 0.1f; i += 0.1f)
        {
            if (Physics.Raycast(origin, direction, out hit, raycastDistance))
            {
                if (hit.collider.gameObject.transform.parent) { 
                    currentRoom = hit.collider.gameObject.transform.parent.gameObject.transform.name;
                }
                // If the ray hits an object, log the object's name
                


            }
        }
    }

    IEnumerator PlaceBomb()
    {
        Vector3 bombPosition = transform.position;
        float offset = 0.5f;

        switch (direction)
        {
            case 'r':
                bombPosition += new Vector3(offset, 0, 0);
                break;
            case 'l':
                bombPosition += new Vector3(-offset, 0, 0);
                break;
            case 'u':
                bombPosition += new Vector3(0, offset, 0);
                break;
            case 'd':
                bombPosition += new Vector3(0, -offset, 0);
                break;
        }

        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);


        yield return new WaitForSeconds(bombExplosionDelay);

        if (bomb != null) { 

            Collider[] hitColliders = Physics.OverlapSphere(bombPosition, bombExplosionRadius, enemyLayerMask);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("enemy"))
            {
                Movement enemyMovement = hitCollider.GetComponent<Movement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(3, direction);
                }
            }
        }


        Vector3[] smokePositions = {
            new Vector3(0.5f, 0.5f, 0),  // Top-right
            new Vector3(-0.5f, 0.5f, 0), // Top-left
            new Vector3(0.5f, -0.5f, 0), // Bottom-right
            new Vector3(-0.5f, -0.5f, 0), // Bottom-left
            new Vector3(0, 0.7f, 0),     // Center-top
            new Vector3(0, -0.7f, 0)     // Center-bottom
        };

        foreach (Vector3 pos in smokePositions)
        {
            GameObject smoke = Instantiate(smokePrefab, bombPosition + pos, Quaternion.identity);
            Animator smokeAnimator = smoke.GetComponent<Animator>();

            if (smokeAnimator != null)
            {
                smokeAnimator.SetTrigger("StartSmoke");
                StartCoroutine(DestroySmokeAfterAnimation(smoke, smokeAnimator));
            }
        }

            Destroy(bomb);
    }
    }

    IEnumerator DestroySmokeAfterAnimation(GameObject smoke, Animator animator)
    {
        if (smoke != null && animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            if (smoke != null)
            {
                Destroy(smoke);
            }
        }
    }


    Vector2 GetInput()
    {

        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal_input) > 0.0f)
            vertical_input = 0.0f;
        // rb.position = new Vector3(rb.position.x - rb.position.x % (float).5, rb.position.y - rb.position.y % (float).5, rb.position.z);

        if ((direction == 'r' || direction == 'l') && vertical_input > 0.0f)
        {
            rb.position = new Vector3(rb.position.x - rb.position.x % (float).25, rb.position.y, rb.position.z);
        }
        else if ((direction == 'u' || direction == 'd') && horizontal_input > 0.0f)
        {
            rb.position = new Vector3(rb.position.x, rb.position.y - rb.position.y % (float).25, rb.position.z);
        }

        if (horizontal_input > 0.0f)
        {
            direction = 'r';
            spriteRenderer.sprite = isThrusting ? swordRight : (isRightFootForward ? idleRightFootForward : idleRightFootBackward);
        }
        else if (horizontal_input < 0.0f)
        {
            direction = 'l';
            spriteRenderer.sprite = isThrusting ? swordLeft : (isRightFootForward ? idleLeftFootForward : idleLeftFootBackward);
        }
        else if (vertical_input > 0.0f)
        {
            direction = 'u';
            spriteRenderer.sprite = isThrusting ? swordUp : (isRightFootForward ? idleUpFootForward : idleUpFootBackward);
        }
        else if (vertical_input < 0.0f)
        {
            direction = 'd';
            spriteRenderer.sprite = isThrusting ? swordDown : (isRightFootForward ? idleDownFootForward : idleDownFootBackward);
        }

        isThrusting = false;
        return new Vector2(horizontal_input, vertical_input);
    }

    IEnumerator ThrustSword()
    {
        if (!canUseSword) yield break;

        canUseSword = false;

        AudioSource.PlayClipAtPoint(sword_used_clip, transform.position, 1);

        if ( inventory.GetHearts() == inventory.maxHearts)
        {
            StartCoroutine(ShootSwordProjectiles());
        }
        else
        {
            StartCoroutine(SpawnAndRemoveSword());
        }

        switch (direction)
        {
            case 'r':

                transform.position += new Vector3(backwardsPrevention, 0, 0);
                spriteRenderer.sprite = swordRight;
                RaycastInDirection(Vector3.right);
                break;
            case 'l':
                transform.position += new Vector3(-backwardsPrevention, 0, 0);
                spriteRenderer.sprite = swordLeft;
                RaycastInDirection(Vector3.left);
                break;
            case 'u':
                transform.position += new Vector3(0, backwardsPrevention, 0);
                spriteRenderer.sprite = swordUp;
                RaycastInDirection(Vector3.up);
                break;
            case 'd':
                // TODO: gotta fix this later
                transform.position += new Vector3(0, -backwardsPrevention, 0);
                spriteRenderer.sprite = swordDown;
                RaycastInDirection(Vector3.down);
                break;
        }

        yield return new WaitForSeconds(swordSeconds);

        switch (direction)
        {
            case 'r':
                transform.position += new Vector3(-backwardsPrevention, 0, 0);
                break;
            case 'l':
                transform.position += new Vector3(backwardsPrevention, 0, 0);
                break;
            case 'u':
                transform.position += new Vector3(0, -backwardsPrevention, 0);
                break;
            case 'd':
                transform.position += new Vector3(0, backwardsPrevention, 0);
                break;
        }

        isThrusting = false;
        ResetToIdleSprite();

        yield return new WaitForSeconds(swordCooldown);
        canUseSword = true;
    }

    IEnumerator SpawnAndRemoveSword()
    {
        Vector3 swordOffset = Vector3.zero;
        Quaternion swordRotation = Quaternion.identity;
        float offset = 1.1f;
        switch (direction)
        {
            case 'r':
                swordOffset = new Vector3(offset, 0, 0);
                swordRotation = Quaternion.Euler(0, 0, 0);
                break;
            case 'l':
                swordOffset = new Vector3(-offset, 0, 0);
                swordRotation = Quaternion.Euler(0, 180, 0);
                break;
            case 'u':
                swordOffset = new Vector3(0, offset, 0);
                swordRotation = Quaternion.Euler(0, 0, 90);
                break;
            case 'd':
                swordOffset = new Vector3(0, -offset, 0);
                swordRotation = Quaternion.Euler(0, 0, -90);
                break;
        }

        Vector3 swordSpawnPosition = transform.position + swordOffset;
        GameObject tempSword = Instantiate(swordPrefab, swordSpawnPosition, swordRotation);

        yield return new WaitForSeconds(swordSeconds);

        Destroy(tempSword);
    }

    IEnumerator ShootSwordProjectiles()
    {
        // TODO: changed
        //yield return new WaitForSeconds(swordSeconds);
        yield return new WaitForSeconds(0.0f);

        Vector3 directionToShoot = Vector3.zero;
        Vector3 spawnPosition = transform.position;

        float swordOffset = 0.9f;
        swordOffset += backwardsPrevention;

        switch (direction)
        {
            case 'r':
                directionToShoot = Vector3.right;
                spawnPosition += new Vector3(swordOffset, 0, 0);
                break;
            case 'l':
                directionToShoot = Vector3.left;
                spawnPosition += new Vector3(-swordOffset, 0, 0);
                break;
            case 'u':
                directionToShoot = Vector3.up;
                spawnPosition += new Vector3(0, swordOffset, 0);
                break;
            case 'd':
                directionToShoot = Vector3.down;
                spawnPosition += new Vector3(0, -swordOffset, 0);
                break;
        }

        GameObject swordProjectile = Instantiate(swordProjectilePrefab, spawnPosition, Quaternion.identity);
        ArrowProj projectile = swordProjectile.GetComponent<ArrowProj>();

        projectile.SetDirection(directionToShoot, true);

        SpriteRenderer projectileRenderer = swordProjectile.GetComponent<SpriteRenderer>();
        projectileRenderer.sprite = swordProjectileSprite;

        if (directionToShoot == Vector3.right)
            swordProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (directionToShoot == Vector3.left)
            swordProjectile.transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (directionToShoot == Vector3.up)
            swordProjectile.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (directionToShoot == Vector3.down)
            swordProjectile.transform.rotation = Quaternion.Euler(0, 0, -90);

        AudioSource.PlayClipAtPoint(sword_projectile_clip, transform.position, 1);
    }


    void ResetToIdleSprite()
    {
        switch (direction)
        {
            case 'r':
                spriteRenderer.sprite = isRightFootForward ? idleRightFootForward : idleRightFootBackward;
                break;
            case 'l':
                spriteRenderer.sprite = isRightFootForward ? idleLeftFootForward : idleLeftFootBackward;
                break;
            case 'u':
                spriteRenderer.sprite = isRightFootForward ? idleUpFootForward : idleUpFootBackward;
                break;
            case 'd':
                spriteRenderer.sprite = isRightFootForward ? idleDownFootForward : idleDownFootBackward;
                break;
        }
    }

    private void RaycastInDirection(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position;

        for (float i = 0; i < swordRange + .1f; i += 0.1f)
        {

            if (Physics.Raycast(rayOrigin, direction, out hit, i))
            {
                if (hit.collider.CompareTag("enemy"))
                {
                    Debug.Log("Enemy hit with sword raycast!");
                    Movement enemy = hit.collider.GetComponent<Movement>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(1, this.direction);
                    }
                }
            }
        }

    }

    IEnumerator ShootBow()
    {
        if (inventory.GetRupees() >= rupeesPerArrow)
        {
            AudioSource.PlayClipAtPoint(bow_used_clip, transform.position, 1);
            inventory.AddRupees(-rupeesPerArrow);

            Vector3 offset = Vector3.zero;
            float arrowPosition = 1.0f;
            switch (direction)
            {
                case 'r': offset = new Vector3(arrowPosition, 0, 0); break;
                case 'l': offset = new Vector3(-arrowPosition, 0, 0); break;
                case 'u': offset = new Vector3(0, arrowPosition, 0); break;
                case 'd': offset = new Vector3(0, -arrowPosition, 0); break;
            }
            Vector3 arrowSpawnPosition = transform.position + offset;

            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPosition, Quaternion.identity);
            ArrowProj arrowProjectile = arrow.GetComponent<ArrowProj>();


            arrowProjectile.SetDirection(GetArrowDirection(), false);

            SpriteRenderer arrowSpriteRenderer = arrow.GetComponent<SpriteRenderer>();
            switch (direction)
            {
                case 'r': arrowSpriteRenderer.sprite = arrowRight; break;
                case 'l': arrowSpriteRenderer.sprite = arrowLeft; break;
                case 'u': arrowSpriteRenderer.sprite = arrowUp; break;
                case 'd': arrowSpriteRenderer.sprite = arrowDown; break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private Vector3 GetArrowDirection()
    {
        switch (direction)
        {
            case 'r': return Vector3.right;
            case 'l': return Vector3.left;
            case 'u': return Vector3.up;
            case 'd': return Vector3.down;
            default: return Vector3.right;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        //rb.position = new Vector3((float)Math.Round(rb.position.x), (float)Math.Round(rb.position.y), rb.position.z);
        //rb.position = new Vector3(rb.position.x-rb.position.x%1, rb.position.y - rb.position.y % 1, rb.position.z);

        if (other.gameObject.CompareTag("Tile"))
        {
            currentRoom = other.gameObject.transform.parent.name;

        }


        if (other.gameObject.CompareTag("enemy"))
        {
            Movement enemy = other.gameObject.GetComponent<Movement>();

            Debug.Log("COLLIDEDDDDDD");
            if (isThrusting && IsSwordFacingEnemy(enemy))
            {
                enemy.TakeDamage(1, direction);
                AudioSource.PlayClipAtPoint(enemy_attack_clip, transform.position, 1);
                Debug.Log("Enemy hit with sword!");
            }
            else
            {
                health.TakeDamage(enemy.direction);
                Debug.Log("Player hit by enemy!");
            }

            // added
            isThrusting = false;

            ResetToIdleSprite();
        }
        
            
        
    }
    public void WallmasterReset() {
        
            //reset position to start
            transform.position = start;
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3((float)(39.5), (float)7, -10);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("bow"))
        {
            hasBow = true;
            Destroy(other.gameObject);

            StartCoroutine(PlayBowCollectionSequence());
        }
    }

    IEnumerator PlayBowCollectionSequence()
    {
        mainCameraAudioSource.Pause();
        isFrozen = true;
        player_movement = false;
        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = false;
        Vector3 tempOffset = new Vector3(0, 0.4f, 0);

        bowPlayerInstance = Instantiate(bowPlayerPrefab, transform.position + tempOffset, Quaternion.identity);

        AudioSource.PlayClipAtPoint(bow_collection_sound_clip, transform.position, 1);
        AudioSource.PlayClipAtPoint(bow_collection_sound_clip2, transform.position, 1);

        yield return null;

        Animator bowPlayerAnimator = bowPlayerInstance.GetComponent<Animator>();
        yield return new WaitForSeconds(bow_collection_sound_clip.length);

        Destroy(bowPlayerInstance);

        spriteRenderer.enabled = true;
        isFrozen = false;
        player_movement = true;

        mainCameraAudioSource.UnPause();
    }

    public void ToggleMovement()
    {
        player_movement = !player_movement;

        spriteRenderer.enabled = !spriteRenderer.enabled;

    }

    public void OnlyMoveToggle()
    {
        player_movement = !player_movement;
        Debug.Log("toggled");
    }

    private bool IsSwordFacingEnemy(Movement enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Vector3 playerPosition = transform.position;

        switch (direction)
        {
            case 'r':
                return enemyPosition.x > playerPosition.x;
            case 'l':
                return enemyPosition.x < playerPosition.x;
            case 'u':
                return enemyPosition.y > playerPosition.y;
            case 'd':
                return enemyPosition.y < playerPosition.y;
            default:
                return false;
        }
    }

}