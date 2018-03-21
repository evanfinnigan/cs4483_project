using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController {

    //Movement
    private float jumpTimer;
    public float maxSpeed = 10f;
    public float jumpPower = 100f;
    public float jumpDelaySecs = 0.5f;      // Delay between subsequent jumps 

    private bool canJump = true;

    // Tracked by CollisionEnter/Exit
    private GameObject currentPlatform;

    // Use this for initialization
    new protected void Start() {
        base.Start();
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
           // Debug.Log("Not on ground");
        }

        if (IsOnGround()) {
            // make sure to update jumpTimer before this
            if (Input.GetAxis("Vertical") > 0) {
                if(canJump) {
                    // jump
                    rb.AddForce(Vector2.up * jumpPower);
                    StartCoroutine(ToggleCanJump());
                }
            }
        }
    }

    new protected void Update() {
        base.Update();
        

    }

    protected void LateUpdate() {
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("jumping", !IsOnGround());

        // Attack
        if (Input.GetButton("Jump") || Input.GetButton("Fire1")) {
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

    IEnumerator ToggleCanJump() {
        canJump = false;
        yield return new WaitForSeconds(jumpDelaySecs);
        canJump = true;
    }

    bool IsOnGround() {
        return currentPlatform != null;
    } 

    private void OnCollisionEnter2D(Collision2D collision) {

        //Debug.Log("player begin colliding");
        if (collision.contacts.Length > 0) {
            // Detect if the player is standing on a new platform. The player is only "on" one platform at a time.
            // The other collider is the player's
            if (collision.contacts[0].point.y <= collision.otherCollider.bounds.min.y) {
                // The player object is ABOVE the platform, which means we are on it.
                //Debug.Log("Collided at " + collision.contacts[0].point + " with player who's at " + collision.otherCollider.bounds.min);
                currentPlatform = collision.gameObject;
                Debug.Log("on ground");
                jumpTimer = 0;
            }
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

    new protected void Die() {
        Debug.Log(name + " is dying");
        maxSpeed = 0;
        rb.AddForce(Vector2.up * jumpPower * 2);
        collid.enabled = false;

        base.Die();
    }

}
