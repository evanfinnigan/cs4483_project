using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

    public int damage = 1;

    private Rigidbody2D rb;
    private Collider2D collid;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        //transform.position += currentSpeed * transform.up * Time.deltaTime;
    }

    //projectile will be destroyed when it hits a platform
    private void OnCollisionEnter2D(Collision2D collision) {
        ActorController actor = collision.gameObject.GetComponent<ActorController>();
        if(actor != null) {
            // ignore projectiles that are just chillin
            if(rb.velocity.x != 0) {
                actor.GetShot(damage);
                Destroy(gameObject);
            }
        }
        // Hit something other than an actor
        else if(collision.gameObject.tag == ActorController.PLATFORM_TAG) {
            Debug.Log("Bullet hit " + collision.gameObject.name);
            // Let gravity do its work; this causes the projectiles to fall to the ground
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1;
            //Destroy(gameObject);
        }
    }
}
