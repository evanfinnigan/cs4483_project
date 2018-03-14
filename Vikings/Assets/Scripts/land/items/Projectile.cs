using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(Animator))]
public class Projectile : MonoBehaviour {

    public int damage = 1;

    private Rigidbody2D rb;
    private Collider2D collid;
    //[SerializeField]
    //private Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<Collider2D>();
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
        if(collision.gameObject == null) {
            return;
        }

        ActorController actor = collision.gameObject.GetComponent<ActorController>();
        if(actor != null) {
            Debug.Log("Projectile hit an actor and the projectile's velocity is " + rb.velocity);
            actor.GetShot(damage);
            //animator.SetBool("impact_blood", true);
        }
        // Hit something other than an actor
        else if(collision.gameObject.tag == ActorController.PLATFORM_TAG) {
            Debug.Log("Bullet hit " + collision.gameObject.name);
            //animator.SetBool("impact_object", true);
        }

        Destroy(gameObject);
    }
}
