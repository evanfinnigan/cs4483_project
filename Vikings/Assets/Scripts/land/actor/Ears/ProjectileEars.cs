using UnityEngine;
using System.Collections;

// Listens for nearby Bullets
public class ProjectileEars : EnemyEars {

    public int chaseRange = 20;

    protected override void Hear(GameObject obj) {
        // The enemy has a 'hearing radius'. If the player or a projectile passes through this circle, the enemy is alerted.

        Vector2? sourceOfNoise = null;
        Projectile proj = obj.GetComponent<Projectile>();
        if (proj != null && proj.IsPlayerProjectile()) {
            Debug.Break();
            // The enemy is able to determine where the bullet was fired from from the bullet's rotation.
            Vector2 bulletSourceDirection = Quaternion.AngleAxis(45, Vector3.forward) * -proj.transform.right;
            Debug.Log("Shot from " + bulletSourceDirection);

            //RaycastHit2D hit = Physics2D.Raycast(proj.transform.position, bulletSourceDirection, chaseRange);
            //sourceOfNoise = hit.point;
            //Debug.Log("Hit point " + sourceOfNoise);
            sourceOfNoise = bulletSourceDirection * chaseRange;
            Debug.DrawRay(proj.transform.position, (Vector3)sourceOfNoise, Color.green, 1);
        }

        if (sourceOfNoise != null) {
            // Debug.Log("SourceOfNoise: " + sourceOfNoise);
            owner.lastSeenPlayerLoc = sourceOfNoise;
        }
    }
}
