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

        /*
        Object[] loaded = Resources.LoadAll("Art/land/");
        Debug.Log("Loaded " + loaded.Length + " items");
        foreach(Object o in loaded) {
            Debug.Log("Loaded " + o.name);
        }*/

        // projectileOffset = projectilePrefab.collider2D.
    }

    // Update is called once per frame
    void Update() {

    }

    public void NewProjectile(Transform shooter, bool facingRight) {
        int direction = facingRight ? 1 : -1;

        // Be careful not to use 'transform' here, use 'shooter' instead.

        Debug.Log("prefab rot " + projectilePrefab.transform.rotation);
        // match projectile rotation and direction to player's
        Vector3 eulers = new Vector3(shooter.rotation.eulerAngles.x, shooter.rotation.eulerAngles.y, 
            projectilePrefab.transform.rotation.z);
        // The projectile sprite is rotated 45 degrees up. It would be better to fix this in the editor,
        // but apparently rotating the prefab doesn't work
        eulers.z -= 45 * direction;

        //Create a projectile object
        GameObject projectile = Instantiate(projectilePrefab,
                (Vector2)shooter.position + direction * projectileOffset * shooter.localScale.x,
                Quaternion.Euler(eulers.x, eulers.y, eulers.z));    

        /*
        GameObject projectile = UnityEditor.PrefabUtility.InstantiatePrefab(projectilePrefab) as GameObject;

        projectile.transform.position = (Vector2)shooter.position + direction * projectileOffset * shooter.localScale.x;
        projectile.transform.rotation = Quaternion.Euler(eulers);
        */

        if (projectile.GetComponent<Projectile>() == null) {
            Debug.LogError("Error: projectilePrefab is not a projectile!");
        }

        //Set its velocity in the directon of movement
        projectile.GetComponent<Rigidbody2D>().velocity = 
                new Vector2(direction * projectileVelocity.x * shooter.localScale.x, 
                projectileVelocity.y);

        // flip the sprite in X if facing left
        // Keep in mind the SpriteRenderer is on a child GO of the projectile.
        projectile.GetComponentInChildren<SpriteRenderer>().flipX = !facingRight;

        //Debug.Break();
    }
}
