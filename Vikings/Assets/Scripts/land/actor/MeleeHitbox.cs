using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeHitbox : MonoBehaviour {

    public int damage = 1;
    public int hKnockback = 1;
    public int vKnockback = 1;

    public static 

    //private Collider2D collid;

    //private bool initialized = false;

    // Use this for initialization
    void Start() {
        //collid = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnEnable() {
        Debug.Log(name + " enable");
        //Debug.Break();
        //if (initialized) {
        //    StartCoroutine(DisableSelfInOneFrame());
        //}
        //else {
        //    initialized = true;
        //}
    }

    private IEnumerator DisableSelfInOneFrame() {
        yield return 0;
        Debug.Log("Disabling self");
        enabled = false;
    }

    private void OnDisable() {
        Debug.Log(name + " disable");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ActorController actor = collision.gameObject.GetComponent<ActorController>();
        if(actor != null) {
            // Someone important got hit!
            Debug.Log(collision.gameObject.name + " got smacked by " + name);
            actor.TakeDamage(damage);

            if(actor.IsAlive()) {
                // they survived the hit
                // Knock them back
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

                // Sometimes collision.contacts.Length == 0 for some reason - need to investigate
                // Collision.contacts documentation says should always be at least 1
                if (collision.contacts.Length > 0 && 
                    collision.contacts[0].point.x > collision.otherCollider.bounds.center.x) {

                    rb.AddForce(new Vector2(-hKnockback, vKnockback));
                }
                else {
                    rb.AddForce(new Vector2(hKnockback, vKnockback));
                }
            }
        }
    }                
}
