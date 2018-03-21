using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class ActorController : MonoBehaviour {

    public const string PLATFORM_TAG = "platform";
    public const string ENEMY_TAG = "enemy";

    protected Rigidbody2D rb;
    protected SpriteRenderer sprenderer;
    protected Animator animator;

    public int hp = 1;

    public bool canShoot = true;
    public bool canMelee = true;

    // TODO set this properly
    protected bool facingRight = true;

    public float projectileCooldown = 1f;
    public float meleeCooldown = 0.2f;

    // Use this for initialization
    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    protected void Update() {
        //Debug.Log("Updating controller for " + name);
        //Debug.Log(transform.position);
        if(transform.position.y < -15) {
            Debug.Log(name + " fell off the map");
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate() {
        
    }

    protected void RangedAttack() {
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
        Debug.Log(name + " attack!");
        animator.SetBool("attacking", true);
        StartCoroutine(ToggleCanMelee());
    }

    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    protected IEnumerator ToggleCanMelee() {
        canMelee = false;
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
        OnMeleeAnimationFinish();
    }

    protected void OnMeleeAnimationFinish() {
        Debug.Log("stop attacking");
        animator.SetBool("attacking", false);
    }

    public void GetShot(int damage) {
        hp -= damage;
        if(hp <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log(name + " is dead");
        Destroy(gameObject);
    }
}
