using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeHitbox : MonoBehaviour {

    public int damage = 1;
    public int hKnockback = 200;
    public int vKnockback = 200;

    // The character holding the melee weapon
    private ActorController holder;

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

    private void OnTriggerEnter2D(Collider2D collision) {
        ActorController other = collision.gameObject.GetComponent<ActorController>();

        // If the target is an enemy and is killed, give the player gold
        int goldValue = 0;
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if(enemy != null) {
            goldValue = enemy.goldValue;
        }

        if(other != null && !holder.GetActorsHitThisFrame().Contains(other)) {
            // Someone important got hit!
            Debug.Log(collision.gameObject.name + " got smacked by " + holder.name);
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
                    //Debug.Log("Knocking back positive");
                    rb.AddForce(new Vector2(hKnockback, vKnockback));
                }
                else {
                    //Debug.Log("Knocking back negative");
                    rb.AddForce(new Vector2(-hKnockback, vKnockback));
                }
            }
            else {
                // The target died
                if (goldValue != 0) {
                    GameState state = FindObjectOfType<GameState>();
                    if(state != null) {
                        state.AddGold(enemy.goldValue);
                    }
                }
            }
        }
    }    
}
