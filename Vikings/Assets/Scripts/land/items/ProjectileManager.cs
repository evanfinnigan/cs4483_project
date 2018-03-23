using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour {

    private static ProjectileManager _instance;

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

    public void NewProjectile(ActorController shooter, bool facingRight) {
        if(!shooter.IsAlive()) {
            return;
        }

        int direction = facingRight ? 1 : -1;

        // Be careful not to use 'transform' here, use 'shooter' instead.

        //Debug.Log("prefab rot " + projectilePrefab.transform.rotation);
        // match projectile rotation and direction to shooter's
        Vector3 eulers = new Vector3(shooter.transform.rotation.eulerAngles.x, shooter.transform.rotation.eulerAngles.y, 
            shooter.projectilePrefab.transform.rotation.z);
        // The projectile sprite is rotated 45 degrees up. It would be better to fix this in the editor,
        // but apparently rotating the prefab doesn't work
        eulers.z -= 45 * direction;

        Vector2 offset = new Vector2(direction * shooter.projectileOffset.x, shooter.projectileOffset.y);
        Vector2 projectileLocation = (Vector2)shooter.transform.position + offset;
        // We want to adjust the x offset for direction, but not y

        //Create a projectile object
        GameObject projectile = Instantiate(shooter.projectilePrefab, projectileLocation, 
            Quaternion.Euler(eulers.x, eulers.y, eulers.z));    

        /*
        GameObject projectile = UnityEditor.PrefabUtility.InstantiatePrefab(projectilePrefab) as GameObject;

        projectile.transform.position = (Vector2)shooter.position + direction * projectileOffset * shooter.localScale.x;
        projectile.transform.rotation = Quaternion.Euler(eulers);
        */

        if (projectile.GetComponent<Projectile>() == null) {
            Debug.LogError("Error: projectilePrefab is not a projectile!");
        }

        //Debug.Log("projectile created at " + projectile.transform.position);

        //Set its velocity to line up with the way the actor's facing
        Vector2 vel = new Vector2(direction * shooter.projectileSpeed.x, shooter.projectileSpeed.y);
        projectile.GetComponent<Rigidbody2D>().velocity = vel;

        //Debug.Log("projectile given velocity " + vel);

        // flip the sprite in X if facing left
        // Keep in mind the SpriteRenderer is on a child GO of the projectile.
        projectile.GetComponentInChildren<SpriteRenderer>().flipX = !facingRight;

        //Debug.Break();
    }
}
