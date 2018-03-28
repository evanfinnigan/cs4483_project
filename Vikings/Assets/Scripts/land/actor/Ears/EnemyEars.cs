using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyEars : MonoBehaviour {

    protected EnemyController owner;

    // Use this for initialization
    void Start() {
        owner = GetComponentInParent<EnemyController>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        // Debug.Log("Initialized the ears of " + owner.name);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Hear(collision.gameObject);
    }

    protected abstract void Hear(GameObject obj);
}
