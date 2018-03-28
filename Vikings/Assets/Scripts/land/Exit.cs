using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject other = collision.gameObject;
        if(other.GetComponent<PlayerController>() != null) {
            if(FindObjectsOfType<EnemyController>().Length > 0) {
                Debug.Log("Enemies remain");
            }
            else {
                Debug.Log("Good job ur done the level");
                WinBattle win = FindObjectOfType<WinBattle>();
                if(win != null) {
                    win.Win();
                }
                else {
                    Debug.LogError("No object with WinBattle behaviour");
                }
                GameState state = FindObjectOfType<GameState>();
                if(state != null) {
                    state.AddGold(500);
                    state.AddCrew();
                }
                else {
                    Debug.LogError("No object with GameState behaviour");
                }
            }
        }
    }
}
