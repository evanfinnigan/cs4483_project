using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FollowObject))]
public class Projectile : MonoBehaviour {

    public int damage = 1;

    private Rigidbody2D rb;
    //private Collider2D collid;
    private SpriteRenderer sprenderer;
    private Animator animator;

    private FollowObject followObj;

    private ActorController creator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        //collid = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        followObj = GetComponent<FollowObject>();
        sprenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        //transform.position += currentSpeed * transform.up * Time.deltaTime;

    }

    private void Update() {
        if(transform.position.x < -100 || transform.position.x > 100) {
            Debug.Log("Projectile OOB");
            Destroy(gameObject);
        }
    }

    //projectile will be destroyed when it hits a platform
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;
        if(other == null) {
            return;
        }
        Debug.Log("projectile hit " + other.name);

        ActorController actor = other.GetComponent<ActorController>();
        if (actor != null) {
            // Hit an actor
            actor.TakeDamage(damage);
            OnHit(other, true);
        }
        else {
            Debug.Log("projectile hit something else " + other.name);
            OnHit(other, false);
        }

        // Let the animation destroy this
        // Destroy(gameObject);
    }

    public void OnHit(GameObject other, bool isBlood) {
        if(isBlood) {
            animator.SetBool("impact_blood", true);
        }
        else {
            animator.SetBool("impact_spark", true);
        }

        // arrow animation follows actor it hit
        if (followObj != null) {
            Vector3 offset = transform.position - other.transform.position;
            followObj.SetOffset(offset);
            followObj.SetTarget(other.transform);
            followObj.Enable();
        }
    }

    public void SetCreator(ActorController creator_) {
        creator = creator_;
    }

    public bool IsPlayerProjectile() {
        return creator.GetComponent<PlayerController>() != null;
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
