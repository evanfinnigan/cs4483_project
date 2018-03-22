using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(Animator))]
public class Projectile : MonoBehaviour {

    public int damage = 1;

    private Rigidbody2D rb;
    //private Collider2D collid;
    //[SerializeField]
    //private Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null) {
            Debug.LogError("Projectile doesn't have a Rigidbody");
        }
        //collid = GetComponent<Collider2D>();
        //animator = GetComponent<Animator>();
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
        if(actor != null) {
            // Hit an actor
            actor.TakeDamage(damage);
            //animator.SetBool("impact_blood", true);
        }
        // Hit something other than an actor
        else if(other.tag == ActorController.PLATFORM_TAG) {
            //animator.SetBool("impact_object", true);
        }
        else {
            Debug.Log("projectile hit something else " + other.name);
        }

        // There might be things we want to 'hit' but not destroy the bullet, then we'd have to move this.
        Destroy(gameObject);
    }
}
