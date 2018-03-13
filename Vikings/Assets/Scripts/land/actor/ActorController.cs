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
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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
