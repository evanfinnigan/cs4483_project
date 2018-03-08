using UnityEngine;

public class Projectile : MonoBehaviour {

    //projectile will be destroyed when it hits a platform
    private void OnCollisionEnter2D(Collision2D collision) {
       Debug.Log("hit");
        if(collision.gameObject.tag == "platform") {
            Destroy(gameObject);
        }
    }
}
