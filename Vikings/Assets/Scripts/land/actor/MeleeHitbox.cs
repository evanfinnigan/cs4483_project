using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeHitbox : MonoBehaviour {

    public int damage = 1;
    public int hKnockback = 200;
    public int vKnockback = 200;

    // Set the hitbox's x position in the editor to match the hitbox on the sprite's right side.
    // Then set this variable to the x position which matches the hitbox on the sprite's LEFT side.
    // When the actor turns around, the hitbox's position will swap.
    public float leftXLocalPosition;
    public float rightXLocalPosition;

    // The character holding the melee weapon - can be set in editor
    public ActorController holder;

    //private Collider2D collid;

    //private bool initialized = false;

    // Use this for initialization
    void Start() {
        //collid = GetComponent<Collider2D>();
        if(holder == null) {
            holder = GetComponentInParent<ActorController>();
            if(holder == null) {
                Debug.LogError("Couldn't find actor holding weapon " + name);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        holder.GetActorsHitThisFrame().Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ActorController other = collision.gameObject.GetComponent<ActorController>();
        if(other != null && !holder.GetActorsHitThisFrame().Contains(other)) {
            // Someone important got hit!
            Debug.Log(collision.gameObject.name + " got smacked by " + name);
            other.TakeDamage(damage);
            holder.GetActorsHitThisFrame().Add(other);

            if(other.IsAlive()) {
                // they survived the hit
                // Knock them back
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

                // Sometimes collision.contacts.Length == 0 for some reason - need to investigate
                // Collision.contacts documentation says should always be at least 1
                /*
                if (collision.contacts.Length > 0 && 
                    collision.contacts[0].point.x > collision.otherCollider.bounds.center.x) {*/
                if(holder.transform.position.x < other.transform.position.x) {
                    //Debug.Log("Knocking back 1");
                    rb.AddForce(new Vector2(hKnockback, vKnockback));
                }
                else {
                    //Debug.Log("Knocking back 2");
                    rb.AddForce(new Vector2(-hKnockback, vKnockback));
                }
            }
        }
    }    

    public void MoveToCorrectSideOfActor(bool isFacingRight) {
        if(isFacingRight) {
            transform.localPosition = new Vector2(rightXLocalPosition, transform.localPosition.y);
        }
        else {
            transform.localPosition = new Vector2(leftXLocalPosition, transform.localPosition.y);
        }
    }
}
