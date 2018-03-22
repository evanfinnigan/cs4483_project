using UnityEngine;
using System.Collections;

public class EnemyController : ActorController {

    public float patrolSpeed = 8f;
    public float chaseSpeed = 20f;
    //public float turnSpeed = 40f;
    public const int visionRange = 25;

    //public Vector2[] patrolPoints;

    // Only works in one dimension right now - enemies can only patrol in X.
    public float[] patrolPointsX;
    private int currentPatrolPoint = 0;

    private Vector2? lastSeenPlayerLoc;

    // Use this for initialization
    new protected void Start() {
        base.Start();
    }

    // Movement in FixedUpdate
    new protected void FixedUpdate() {
        base.FixedUpdate();

        bool foundPlayer = LookForPlayer();

        // If you know where the player was, or you see him now, chase him
        if(lastSeenPlayerLoc != null || foundPlayer) {
            Debug.Log(name + " chasing player to " + lastSeenPlayerLoc);
            // attack them if can, else move to last known loc
            if(!MoveTo(( (Vector2)lastSeenPlayerLoc ).x, chaseSpeed)) {
                // reached last known loc; this means we have lost track of player.
                lastSeenPlayerLoc = null;
            }
        }
        // Else, patrol if a patrol path is set
        else if(patrolPointsX.Length > 0) {
            float patrolTarget = patrolPointsX[currentPatrolPoint];
            // Debug.Log(name + " moving to patrol point " + currentPatrolPoint);
            if (!MoveTo(patrolTarget, patrolSpeed)) {
                // try to move to the current patrol point. 
                // If already reached it, iterate to the next patrol point.
                currentPatrolPoint++;
                // don't go past the end of the patrol points
                if (currentPatrolPoint > patrolPointsX.Length - 1) {
                    currentPatrolPoint %= patrolPointsX.Length;
                }
            }
        }
        else {
            // No patrol set and no player in sight - just stand there
        }

    }

    // Move to a location. Returns if any moving was actually performed - ie returns false if already at location.
    bool MoveTo(float destX, float speed) {
        float xDiff = transform.position.x - destX;
        if(xDiff > 0 && facingRight || xDiff < 0 && !facingRight) {
            TurnAround();
        }

        if(Mathf.Abs(transform.position.x - destX) < 0.1) {
            // already arrived at destination
            Debug.Log("arrived");
            return false;
        }

        animator.SetFloat("speed", speed);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(destX, transform.position.y), 
            speed / 50);

        return true;
    }

    /*
    //  Returns true if any turning was performed - ie returns false if already facing dest
    bool TurnTo(Vector2 dest) {
        float remainingRotation = Vector2.SignedAngle(transform.right.normalized, 
            (dest - (Vector2)transform.position).normalized);

        // print("REMAINING ROTATION" + remainingRotation);
        if (turnSpeed >= Mathf.Abs(remainingRotation)) {
            transform.Rotate(new Vector3(0, 0, remainingRotation));
            // finished turning
            return false;
        }

        transform.Rotate(new Vector3(0, 0, turnSpeed * Mathf.Sign(remainingRotation)));
        return true;
    }
    */

    // Check to see if player is in line of vision. If so, set lastSeenPlayerLoc, and return true.
    bool LookForPlayer() {
        Vector3 raySource = transform.position;
        // We have to cast the ray from lower because the enemy's sprite is much bigger than the enemy appears
        // so, to prevent casting the ray too high:
        raySource.y -= 1;

        Vector3 rayDir = ( facingRight ? 1 : -1 ) * transform.right * visionRange;

        Debug.DrawRay(raySource, rayDir, Color.white);
        RaycastHit2D hit = Physics2D.Raycast(raySource, rayDir);
        if(!hit) {
            return false;
        }

        GameObject other = hit.collider.gameObject;
        //Debug.Log("enemy eye ray hit " + other.name);

        if (other.GetComponent<PlayerController>() != null) {
            // we can see the player, we have to chase them
            // For now, set y-component to self's y, since enemies have no vertical movement.
            lastSeenPlayerLoc = new Vector2(hit.collider.gameObject.transform.position.x, transform.position.y);
            //Debug.Log("I see the player at " + lastSeenPlayerLoc);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null) {
            // enemy is bumping into player
            lastSeenPlayerLoc = collision.gameObject.transform.position;

            MeleeAttack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        OnCollisionEnter2D(collision);
    }
}
