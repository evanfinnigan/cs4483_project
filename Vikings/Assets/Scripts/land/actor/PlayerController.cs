using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController {

    //Movement
    public float maxSpeed = 10f;
    public float jumpPower = 200f;
    public float jumpDelaySecs = 0.5f;      // Delay between subsequent jumps
    // Set the animator component's controller in the editor to the melee controller,
    // then set this one to the ranged controller.
    public RuntimeAnimatorController meleeAnimationController;
    public RuntimeAnimatorController rangedAnimationController;

    private bool canJump = true;
    private bool meleeMode = true;

    // Tracked by CollisionEnter/Exit
    private GameObject currentPlatform;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Handle all player movement in FixedUpdate since we're using RBs
    protected override void FixedUpdate() {
        base.FixedUpdate();

        float movementDampener = IsOnGround() ? 1 : 0.5f;

        // In the air, keep momentum, but can't influence horizontal speed
        //Debug.Log("ground");
        // Left/Right movement
        float moveX = Input.GetAxis("Horizontal") * movementDampener;

        rb.velocity = new Vector2(moveX * maxSpeed, rb.velocity.y);

        if ((moveX > 0 && !facingRight) || (moveX < 0 && facingRight)) {
            TurnAround();
        }
        
        // Jumping stuff
        if (Input.GetAxis("Vertical") > 0) {
            if(canJump) {
                // jump
                rb.AddForce(Vector2.up * jumpPower);
                StartCoroutine(ToggleCanJump());
            }
        }
    }

    protected override void Update() {
        base.Update();
        
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        animator.SetFloat(ANIM_SPEED, Mathf.Abs(rb.velocity.x));
        animator.SetBool(ANIM_JUMPING, !IsOnGround());

        // Attack
        if (Input.GetButtonDown("Jump") /*|| Input.GetButton("Fire1")*/) {
            //Debug.Log("Attack!");
            if(meleeMode) {
                MeleeAttack();
            }
            else {
                RangedAttack();
            }
        }
        
        // Switching weapons
        if (Input.GetKeyDown(KeyCode.Tab)) {
            meleeMode = !meleeMode;
            animator.runtimeAnimatorController = meleeMode ? meleeAnimationController : rangedAnimationController;
            Debug.Log("Now in " + (meleeMode ? "melee" : "ranged") + " mode");
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
                // Debug.Log("on ground");
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

    protected override void Die(bool fromFalling=false) {
        Debug.Log(name + " is dying");

        // If player died from falling, skip this
        if(!fromFalling) {
            FreezeThenThrowPlayer(0);
        }
        else {
            base.Die();
        }
    }

    // death "animation"
    protected void FreezeThenThrowPlayer(float freezeLen) {
        // Freeze the player briefly
        //rb.simulated = false;
        // remove vertical velocity - otherwise this can result in some weirdness
        rb.velocity = new Vector2(rb.velocity.x, 0);
        animator.enabled = false;
        collid.enabled = false;

        //yield return new WaitForSeconds(freezeLen);

        // now throw them up in the air, and let them fall through the map to their death
        rb.simulated = true;
        rb.AddForce(Vector2.up * jumpPower);
        // also turn player upside down cause it's funny
        sprenderer.flipY = true;

        // then they die from falling
    }

    public void KillPlayer() {
        Die(false);
    }

}
