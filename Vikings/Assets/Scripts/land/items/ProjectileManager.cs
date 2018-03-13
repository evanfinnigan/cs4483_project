using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour {

    private static ProjectileManager _instance;

    public Vector2 projectileVelocity;
    public GameObject projectilePrefab;
    private Vector2 projectileOffset = new Vector2(1f, 0);

    // Singleton pattern
    public static ProjectileManager instance() {
        if(_instance == null) {
            Debug.LogError("Null ProjectileManager; make sure it is attached to exactly one game object in the scene");
        }
        return _instance;
    }

    // Use this for initialization
    void Start() {
        if(_instance != null) {
            Debug.LogError("Warning: multiple ProjectileManager instances");
        }
        _instance = this;

        // projectileOffset = projectilePrefab.collider2D.
    }

    // Update is called once per frame
    void Update() {

    }

    public void NewProjectile(Transform shooter, bool facingRight) {
        int direction = facingRight ? 1 : -1;

        // Be careful not to use 'transform' here, use 'shooter' instead.

        // match projectile rotation and direction to player's
        Vector3 eulers = shooter.rotation.eulerAngles;
        // The projectile sprite is rotated 45 degrees up. It would be better to fix this in the editor.
        eulers.z -= 45 * direction;

        //Create a projectile object
        GameObject projectile = Instantiate(projectilePrefab,
                (Vector2)shooter.position + direction * projectileOffset * shooter.localScale.x,
                Quaternion.Euler(eulers.x, eulers.y, eulers.z));

        if(projectile.GetComponent<Projectile>() == null) {
            Debug.LogError("Error: projectilePrefab is not a projectile!");
        }

        //Set its velocity in the directon of movement
        projectile.GetComponent<Rigidbody2D>().velocity = 
                new Vector2(direction * projectileVelocity.x * shooter.localScale.x, 
                projectileVelocity.y);

        // flip the sprite in X if facing left
        projectile.GetComponent<SpriteRenderer>().flipX = !facingRight;
    }
}
