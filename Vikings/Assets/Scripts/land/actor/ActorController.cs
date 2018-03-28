using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class ActorController : MonoBehaviour {

    public const string 
            PLATFORM_TAG = "platform",

            ANIM_SPEED = "speed",
            ANIM_JUMPING = "jumping",
            ANIM_ATTACKING = "attacking";

    protected Rigidbody2D rb;
    protected Collider2D collid;
    protected SpriteRenderer sprenderer;
    protected Animator animator;
    protected MeleeHitbox[] meleeHitboxes;

    // prevents hitting the same actor with multiple hitboxes in one frame
    // This logic should be in meleehitbox, but it needs to be per-actor
    private List<ActorController> actorsHitThisFrame;

    public int hp = 1;
    public int deathDepth = -40;

    // We store this for all actors, including melee-only enemies
    // Could subclass to make a ranged type which applies to the player and ranged enemies.
    public Vector2 projectileOffset = new Vector2(1f, -0.5f);
    public Vector2 projectileSpeed = new Vector2(30, 0);
    public GameObject projectilePrefab;

    protected bool canShoot = true;
    protected bool canMelee = true;
    protected bool canMove = true;

    protected bool facingRight;

    protected float projectileCooldown = 1.2f;
    protected float meleeCooldown = 1f;

    // Use this for initialization
    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<Collider2D>();
        sprenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        meleeHitboxes = gameObject.GetComponentsInChildren<MeleeHitbox>(true);

        actorsHitThisFrame = new List<ActorController>();

        facingRight = !sprenderer.flipX;

        if(projectilePrefab == null) {
            Debug.LogWarning("No projectile prefab set for actor " + name);
        }
    }

    // Update is called once per frame
    protected virtual void Update() {
        //Debug.Log("Updating controller for " + name);
        //Debug.Log(transform.position);
        if(transform.position.y < deathDepth) {
            Debug.Log(name + " fell off the map");
            Die(true);
        }
    }

    protected virtual void FixedUpdate() {
        
    }

    protected virtual void LateUpdate() {
    }

    protected virtual void TurnAround() {
        facingRight = !facingRight;
        sprenderer.flipX = !facingRight;
        //Debug.Log(name + " now" + (facingRight ? "" : " not") + " facing right");
    }

    protected virtual void RangedAttack() {
        if(!IsAlive() || !canShoot) {
            return;
        }

        // The bow attack animation will then call StartShoot, Shoot, EndShoot
        animator.SetBool(ANIM_ATTACKING, true);
        //Debug.Log("RangedAttack");

        // start cooldown timer
        //Debug.Log("Can't shoot");
        canShoot = false;
        StartCoroutine(ToggleCanShoot());
        
        //Debug.Break();
    }

    // called BY THE ANIMATION to root actor while they fire
    public void StartShoot() {
        animator.SetBool(ANIM_ATTACKING, false);
        //Debug.Log("StartShoot");
        canMove = false;
    }

    // called BY THE ANIMATION to make sure projectile spawn lines up with animation
    public void Shoot() {
        //Debug.Log("Shoot");
        ProjectileManager.instance().NewProjectile(this, facingRight);
    }

    // called BY THE ANIMATION to unroot actor once they're done firing
    public void EndShoot() {
        //Debug.Log("EndShoot");
        canMove = true;
    }

    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    protected IEnumerator ToggleCanShoot() {
        yield return new WaitForSeconds(projectileCooldown);
        //Debug.Log("Can shoot");
        canShoot = true;
    }

    protected void MeleeAttack() {
        if(!IsAlive() || !canMelee) {
            //Debug.Log(name + " can't melee yet");
            return;
        }

        //Debug.Log(name + " attack!");
        animator.SetBool(ANIM_ATTACKING, true);

        //Debug.Break();
        StartCoroutine(ToggleCanMelee());
    }

    // We want to only attack once, so we unset attacking variable as soon as animation begins.
    protected void UnsetAttacking() {
        animator.SetBool(ANIM_ATTACKING, false);
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

            if(on) {
                // Relies on the hitboxes having names with "left" and "right"
                if((facingRight && hb.name.ToLower().Contains("right")) 
                    || (!facingRight && hb.name.ToLower().Contains("left"))) {

                    hb.gameObject.SetActive(true);
                }
            }
            else {
                // disable all
                hb.gameObject.SetActive(false);
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
    protected virtual void Die(bool fromFalling=false) {
        Debug.Log(name + " is dead");
        Destroy(gameObject);
    }

    public List<ActorController> GetActorsHitThisFrame() {
        return actorsHitThisFrame;
    }
}
