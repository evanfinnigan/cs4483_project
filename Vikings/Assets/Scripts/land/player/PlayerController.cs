using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    const string PLATFORM_TAG = "platform";
    const string ENEMY_TAG = "enemy";

    //Movement
    private float jumpTimer;
    public float maxSpeed = 10f;
    public float jumpPower = 100f;
    public float jumpDelay = 0.1f; // Delay between landing and being able to jump again
    private bool facingRight = true;
    //public float inAirMovementDampeningFactor = 0.5f;

    //Projectiles
    public Vector2 projectileVelocity;
    public GameObject projectilePrefab;
    public float projectileCooldown = 1f;
    private bool canShoot = true;
    private Vector2 projectileOffset = new Vector2(0.4f, 0.1f);

    private Rigidbody2D rb;
    private Collider2D collid;

    // Tracked by CollisionEnter/Exit
    private GameObject currentPlatform;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<Collider2D>();

        // player is able to jump immediately.
        jumpTimer = jumpDelay;
	}

	// Handle all player movement in FixedUpdate since we're using RBs
	void FixedUpdate () {
        // In the air, keep momentum, but can't influence horizontal speed
        if (IsOnGround()) {
            // Left/Right movement
            float moveX = Input.GetAxis("Horizontal");

            //float dampener = onGround ? 1 : inAirMovementDampeningFactor;
            rb.velocity = new Vector2(moveX * maxSpeed, rb.velocity.y);

            if ((moveX > 0 && !facingRight) || (moveX < 0 && facingRight)) {
                TurnAround();
            }
        }

        // Jumping stuff
        if(jumpTimer < jumpDelay) {
            jumpTimer += Time.fixedDeltaTime;
        }

        if (IsOnGround()) {
            if (Input.GetAxis("Vertical") > 0 && jumpTimer >= jumpDelay) {
                // jump
                rb.AddForce(Vector2.up * jumpPower);
            }
        }
	}

    void Update() {
        //Space button to shoot only if cooldown allows it
        if(Input.GetButtonDown("Jump") && canShoot)
        {
            //Create a projectile object
            GameObject projectile = (GameObject) Instantiate(projectilePrefab, 
                    (Vector2)transform.position + projectileOffset * rb.transform.localScale.x, 
                    Quaternion.identity);

            //Set its velocity in the directon of movement
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileVelocity.x * rb.transform.localScale.x, projectileVelocity.y);

            //start cooldown timer
            StartCoroutine(CanShoot());
        }
    }

    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(projectileCooldown);
        canShoot = true;
    }

    // Sprite and Animator will use this to figure out which way to face the player.
    void TurnAround() {
        facingRight = !facingRight;
    }

    bool IsOnGround() {
        return currentPlatform != null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.contacts.Length > 0)
        {
            // Detect if the player is standing on a new platform. The player is only "on" one platform at a time.
            if(collision.gameObject.tag == PLATFORM_TAG && currentPlatform == null) {
                if(collision.contacts[0].point.y < collid.bounds.center.y) {
                    // The player object is ABOVE the platform, which means we are on it.
                    currentPlatform = collision.gameObject;
                    //Debug.Log("on ground");
                    jumpTimer = 0;
                }
            }
            else if(collision.gameObject.tag == ENEMY_TAG)
            {
                //Taking a hit from an enemy pushes you back in the opposite direction
                if(collision.contacts[0].point.x > collid.bounds.center.x)
                {
                    rb.AddForce(new Vector2(-1,1) * jumpPower);
                }
                else
                {
                    rb.AddForce(new Vector2(1,1) * jumpPower);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject == currentPlatform) {
            currentPlatform = null;
            //Debug.Log("Not on ground");
        }
    }
}
