using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController {

    //Movement
    private float jumpTimer;
    public float maxSpeed = 10f;
    public float jumpPower = 100f;
    public float jumpDelay = 0.1f;      // Delay between landing and being able to jump again


    // Tracked by CollisionEnter/Exit
    private GameObject currentPlatform;


    // Use this for initialization
    new protected void Start() {
        base.Start();

        // player is able to jump immediately.
        jumpTimer = jumpDelay;
    }

    // Handle all player movement in FixedUpdate since we're using RBs
    new protected void FixedUpdate() {
        base.FixedUpdate();

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
            //Debug.Log("floaty");
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

    new protected void Update() {
        base.Update();
        
        animator.SetFloat("speed", rb.velocity.magnitude);
        animator.SetBool("jumping", !IsOnGround());

        //Space button to shoot only if cooldown allows it
        if (Input.GetButton("Jump")) {
            // if bow is equipped && canShoot
            // no bow yet, so don't execute this
            if("".Equals(" ")) {
                RangedAttack();
            }
            else {              // sword equipped
                MeleeAttack();
            }
        }
    }

    bool IsOnGround() {
        return currentPlatform != null;
    } 

    private void OnCollisionEnter2D(Collision2D collision) {

        //Debug.Log("player begin colliding");
        if (collision.contacts.Length > 0) {
            // Detect if the player is standing on a new platform. The player is only "on" one platform at a time.
            //if (collision.gameObject.tag == PLATFORM_TAG && currentPlatform == null) {
                // The other collider is the player's
                if (collision.contacts[0].point.y < collision.otherCollider.bounds.center.y) {
                    // The player object is ABOVE the platform, which means we are on it.
                    currentPlatform = collision.gameObject;
                    Debug.Log("on ground");
                    jumpTimer = 0;
                }
            //}
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        // Debug.Log("player staying colliding");
        // seems hacky
        OnCollisionEnter2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject == currentPlatform) {
            currentPlatform = null;
            Debug.Log("Not on ground");
        }
    }


}
