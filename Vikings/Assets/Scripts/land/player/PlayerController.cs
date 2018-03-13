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

    //Projectiles - This stuff should be moved out of this class.
    public Vector2 projectileVelocity;
    public GameObject projectilePrefab;
    public float projectileCooldown = 1f;
    private bool canShoot = true;
    private Vector2 projectileOffset = new Vector2(0.4f, 0.1f);

    private Rigidbody2D rb;

    // Tracked by CollisionEnter/Exit
    private GameObject currentPlatform;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // player is able to jump immediately.
        jumpTimer = jumpDelay;
    }

    // Handle all player movement in FixedUpdate since we're using RBs
    void FixedUpdate() {
        // In the air, keep momentum, but can't influence horizontal speed
        if (IsOnGround()) {
            //Debug.Log("ground");
            // Left/Right movement
            float moveX = Input.GetAxis("Horizontal");

            //float dampener = onGround ? 1 : inAirMovementDampeningFactor;
            rb.velocity = new Vector2(moveX * maxSpeed, rb.velocity.y);

            if ((moveX > 0 && !facingRight) || (moveX < 0 && facingRight)) {
                TurnAround();
            }
        }
        else {
            Debug.Log("floaty");
        }

        // Jumping stuff
        if (jumpTimer < jumpDelay) {
            jumpTimer += Time.fixedDeltaTime;
        }

        if (IsOnGround()) {
            // make sure to update jumpTimer before this
            if (Input.GetAxis("Vertical") > 0 && jumpTimer >= jumpDelay) {
                // jump
                rb.AddForce(Vector2.up * jumpPower);
            }
        }
    }

    void Update() {
        //Space button to shoot only if cooldown allows it
        if (Input.GetButtonDown("Jump") && canShoot) {
            int direction = facingRight ? 1 : -1;

            // match projectile rotation and direction to player's
            Vector3 eulers = transform.rotation.eulerAngles;
            // The projectile sprite is rotated 45 degrees up. It would be better to fix this in the editor.
            eulers.z -= 45 * direction;

            //Create a projectile object
            GameObject projectile = Instantiate(projectilePrefab,
                    (Vector2)transform.position + direction * projectileOffset * rb.transform.localScale.x,
                    Quaternion.Euler(eulers.x, eulers.y, eulers.z));

            //Set its velocity in the directon of movement
            projectile.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(direction * projectileVelocity.x * rb.transform.localScale.x, 
                    projectileVelocity.y);

            projectile.GetComponent<SpriteRenderer>().flipX = !facingRight;

            //start cooldown timer
            StartCoroutine(ToggleCanShoot());
        }
    }

    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    IEnumerator ToggleCanShoot() {
        canShoot = false;
        yield return new WaitForSeconds(projectileCooldown);
        canShoot = true;
    }

    // Sprite and Animator will use this to figure out which way to face the player.
    void TurnAround() {
        facingRight = !facingRight;
        //Debug.Log("Now" + (facingRight ? "" : " not") + " facing right");
    }

    bool IsOnGround() {
        return currentPlatform != null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        //Debug.Log("player begin colliding");
        if (collision.contacts.Length > 0) {
            // Detect if the player is standing on a new platform. The player is only "on" one platform at a time.
            if (collision.gameObject.tag == PLATFORM_TAG && currentPlatform == null) {
                // The other collider is the player's
                if (collision.contacts[0].point.y < collision.otherCollider.bounds.center.y) {
                    // The player object is ABOVE the platform, which means we are on it.
                    currentPlatform = collision.gameObject;
                    Debug.Log("on ground");
                    jumpTimer = 0;
                }
            }
            else if (collision.gameObject.tag == ENEMY_TAG) {
                //Taking a hit from an enemy pushes you back in the opposite direction
                if (collision.contacts[0].point.x > collision.otherCollider.bounds.center.x) {
                    rb.AddForce(new Vector2(-1, 1) * jumpPower);
                }
                else {
                    rb.AddForce(new Vector2(1, 1) * jumpPower);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        // Debug.Log("player staying colliding");
        // OnCollisionEnter2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject == currentPlatform) {
            currentPlatform = null;
            Debug.Log("Not on ground");
        }
    }
}
