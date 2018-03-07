using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    const string PLATFORM_TAG = "platform";

    public float maxSpeed = 10f;
    public float jumpPower = 100f;
    //public float inAirMovementDampeningFactor = 0.5f;

    // After landing on the ground, after this many seconds the player is able to jump again.
    public float jumpDelay = 0.1f;

    private bool facingRight = true;
    private float jumpTimer;

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
	
	// Update is called once per frame
	void FixedUpdate () {

        // Left/Right movement
        float moveX = Input.GetAxis("Horizontal");

        //float dampener = onGround ? 1 : inAirMovementDampeningFactor;
        rb.velocity = new Vector2(moveX * maxSpeed, rb.velocity.y);

        if ((moveX > 0 && !facingRight) || (moveX < 0 && facingRight)) {
            TurnAround();
        }
        

        // Jumping stuff
        if(jumpTimer < jumpDelay) {
            jumpTimer += Time.fixedDeltaTime;
        }

        if (IsOnGround()) {
            if (Input.GetAxis("Vertical") > 0 && jumpTimer > jumpDelay) {
                // jump
                rb.AddForce(Vector2.up * jumpPower);
            }
        }
	}

    void TurnAround() {
        facingRight = !facingRight;
    }

    bool IsOnGround() {
        return currentPlatform != null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == PLATFORM_TAG && currentPlatform == null) {
            if (collision.contacts[0].point.y < collid.bounds.center.y) {
                // The player object is ABOVE the platform, which means we are "on" it.
                currentPlatform = collision.gameObject;
                // Debug.Log("on ground");

                jumpTimer = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject == currentPlatform) {
            currentPlatform = null;
            // Debug.Log("Not on ground");
        }
    }
}
