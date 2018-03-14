using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : ActorController {

    //Movement
    private float jumpTimer;
    public float maxSpeed = 10f;
    public float jumpPower = 100f;
    public float jumpDelay = 0.1f;      // Delay between landing and being able to jump again

    private Rigidbody2D rb;
    private SpriteRenderer sprenderer;

    // Tracked by CollisionEnter/Exit
    private GameObject currentPlatform;


    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprenderer = GetComponent<SpriteRenderer>();

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
        if (Input.GetButton("Jump") && canShoot) {
            Shoot();
        }
    }

    void Shoot() {
        ProjectileManager.instance().NewProjectile(transform, facingRight);

        //start cooldown timer
        StartCoroutine(ToggleCanShoot());
    }

    // Sprite and Animator will use this to figure out which way to face the player.
    void TurnAround() {
        facingRight = !facingRight;
        sprenderer.flipX = !facingRight;

        // move the eye to show where the player is facing :P
        // temporary until we have a real sprite.
        Transform eye = GameObject.Find("playerEye").transform;
        eye.localPosition = new Vector3(-eye.localPosition.x, eye.localPosition.y, eye.localPosition.z);

        Debug.Log("Now" + (facingRight ? "" : " not") + " facing right");
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
