using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class ActorController : MonoBehaviour {

    public const string PLATFORM_TAG = "platform";

    protected Rigidbody2D rb;
    protected Collider2D collid;
    protected SpriteRenderer sprenderer;
    protected Animator animator;

    protected MeleeHitbox[] meleeHitboxes;

    public int hp = 1;

    protected bool canShoot = true;
    protected bool canMelee = true;

    // TODO set this properly - IE check the actor's heading to see if they are looking in the negative (left)
    // or positive (right) direction instead of assuming they are looking right.
    // This variable and TurnAround() are not used by enemies yet.
    protected bool facingRight = true;

    protected float projectileCooldown = 1f;
    protected float meleeCooldown = 1f;

    // Use this for initialization
    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<Collider2D>();
        sprenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        meleeHitboxes = gameObject.GetComponentsInChildren<MeleeHitbox>(true);
    }

    // Update is called once per frame
    protected void Update() {
        //Debug.Log("Updating controller for " + name);
        //Debug.Log(transform.position);
        if(transform.position.y < -10) {
            Debug.Log(name + " fell off the map");
            Die();
        }
    }

    protected void FixedUpdate() {
        
    }

    protected void LateUpdate() {
    }

    protected void TurnAround() {
        facingRight = !facingRight;
        sprenderer.flipX = !facingRight;
        Debug.Log(name + " now" + (facingRight ? "" : " not") + " facing right");
    }

    protected void RangedAttack() {
        if(!canShoot) {
            return;
        }

        ProjectileManager.instance().NewProjectile(transform, facingRight);

        //start cooldown timer
        StartCoroutine(ToggleCanShoot());
    }

    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    protected IEnumerator ToggleCanShoot() {
        canShoot = false;
        yield return new WaitForSeconds(projectileCooldown);
        canShoot = true;
    }

    protected void MeleeAttack() {
        if(!canMelee) {
            //Debug.Log(name + " can't melee yet");
            return;
        }

        //Debug.Log(name + " attack!");
        animator.SetBool("attacking", true);

        //Debug.Break();
        StartCoroutine(ToggleCanMelee());
    }

    // We want to only attack once, so we unset attacking variable as soon as animation begins.
    protected void UnsetAttacking() {
        animator.SetBool("attacking", false);
    }

    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    protected IEnumerator ToggleCanMelee() {
        canMelee = false;
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
    }

    // The int parameter is actually a bool, ie 0 -> false, (not 0) -> true. 
    // For some reason this function is compatible with Animation Events if it has an INT parameter,
    // but NOT if it has a BOOL parameter.
    protected void ToggleMeleeHitbox(int newState) {
        // Debug.Log(name + " toggling melee hitboxes");
        foreach(MeleeHitbox hb in meleeHitboxes) {
            bool on = 0 != newState;
            hb.gameObject.SetActive(on);

            if(on) {
                // Adjust the hitbox's position to match the direction the player is facing
                hb.MoveToCorrectSideOfActor(facingRight);
                //Debug.Log("Moved HB to " + hb.transform.localPosition);
            }
        }
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        Debug.Log(name + " got hurt and now has " + hp + " hp");
        if(!IsAlive()) {
            Die();
        }
    }

    public bool IsAlive() {
        return this != null && hp > 0;
    }

    // Note that this die DESTROYS this object. So, if you want to do any on-death stuff in subclass,
    // call this AFTER.
    public void Die() {
        Debug.Log(name + " is dead");
        Destroy(gameObject);
    }
}
