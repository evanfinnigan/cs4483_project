using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EquippableItem : MonoBehaviour {

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;

        if(other.GetComponent<PlayerController>() != null) {
            // Player picks up item

            // These cause the player to "hold" the item- looks like garbage right now
            transform.parent = other.transform;
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 90);

            // remove physics
            rb.simulated = false;
            Debug.Log(other.name + " picked up " + name);
        }
    }
}
