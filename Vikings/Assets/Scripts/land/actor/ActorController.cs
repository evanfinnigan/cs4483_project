using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour {

    public const string PLATFORM_TAG = "platform";
    public const string ENEMY_TAG = "enemy";

    public int hp = 1;

    protected bool canShoot = true;

    // TODO set this properly
    protected bool facingRight = true;
    public float projectileCooldown = 1f;

    // Use this for initialization
    protected void Start() {

    }

    // Update is called once per frame
    protected void Update() {
        //Debug.Log("Updating controller for " + name);
        //Debug.Log(transform.position);
        if(transform.position.y < -15) {
            Debug.Log(name + " fell off the map");
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate() {
        
    }


    //Toggles the canShoot variable to false for [cooldown] amount of seconds
    protected IEnumerator ToggleCanShoot() {
        canShoot = false;
        yield return new WaitForSeconds(projectileCooldown);
        canShoot = true;
    }

    public void GetShot(int damage) {
        hp -= damage;
        if(hp <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log(name + " is dead");
        Destroy(gameObject);
    }
}
