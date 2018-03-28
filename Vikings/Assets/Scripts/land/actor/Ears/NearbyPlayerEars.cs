using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 
// If the player is within a small radius, the enemy hears them
public class NearbyPlayerEars : EnemyEars {

    
    private List<Player> collidingPlayers = new List<Player>();

    private void Update() {
        collidingPlayers.ForEach(player => {
            if (listenForPlayer(player)) {
                SendHearNoise(player);
            }
        });
    }

    protected void OnTriggerExit2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            collidingPlayers.Remove(player);
        }
    }

    protected override void Hear(GameObject obj) {
        // The enemy has a 'hearing radius'. If the player passes through this circle, the enemy is alerted.
        Vector2? sourceOfNoise = null;
        Player player = obj.GetComponent<Player>();
        if (player != null) {

            // Can't hear through walls
            */
            /*
            LayerMask layerMask = -1;
            // Ignore Raycast layer
            int mask = 1 << 2;
            layerMask.value &= ~mask;
            // 
            mask = 1 << 8;
            layerMask.value &= ~mask;
            */

        /*
            
            collidingPlayers.Add(player);
            if(listenForPlayer(player)) {
                // Send the enemy after the player if the ray hits the player
                SendHearNoise(player);
            }
        }

        if (sourceOfNoise != null) {
            Debug.Log("SourceOfNoise: " + sourceOfNoise);
        }
    }

    // Cast a ray of 'sound' towards the player. If it hits the player, the enemy hears the player, and chases him.
    private bool listenForPlayer(Player player) {
        // Debug.Log("Listening for player " + player.gameObject.name);
        RaycastHit2D castHit = Physics2D.Raycast(owner.transform.position, player.transform.position - owner.transform.position);// Mathf.Infinity, layerMask);
        // Debug.Log("The cast hit " + castHit.transform.gameObject.name + " tagged " + castHit.transform.gameObject.tag);

        Debug.DrawRay(owner.transform.position, player.transform.position - owner.transform.position, Color.red);
        return castHit.transform != null && castHit.transform.gameObject.CompareTag("Player");
    }

    private void SendHearNoise(Player player) {
        // Only send the enemy after the player one time per time player gets heard (Enters ear radius)
        collidingPlayers.Remove(player);
        owner.HearNoise(new Enemy.PlayerLocation((Vector2)player.transform.position, 1));
    }
}

 */
