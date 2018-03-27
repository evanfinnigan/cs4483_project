using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class for handling the heads up display (HUD).
/// </summary>
public class HUD : MonoBehaviour
{
    public Text goldAmount;
    public Text numberCrew;
    public GameState state;
    public GameObject axeObj;
    public GameObject swordObj;
    public GameObject noneObj;
    public GameObject bowObj;
    GameObject go;

    void Start()
    {
        axeObj = GameObject.Find("Axe Panel");
        swordObj = GameObject.Find("Sword Panel");
        noneObj = GameObject.Find("None Panel");
        bowObj = GameObject.Find("Bow Panel");
        go = GameObject.Find("GameState");
        state = go.GetComponent<GameState>();
    }

    void Update ()
    {
        // update crew numbers (i.e., health), gold and weapon
        if (state != null)
        {
            goldAmount.text = "x" + state.GetGold().ToString();
            numberCrew.text = "x" + state.GetCrew().ToString();
            string weapon = state.GetWeapon().ToString();

            // show the appropriate weapon
            switch (weapon)
            {
                case "axe":
                    axeObj.SetActive(true);
                    bowObj.SetActive(false);
                    noneObj.SetActive(false);
                    swordObj.SetActive(false);
                    break;

                case "sword":
                    axeObj.SetActive(false);
                    bowObj.SetActive(false);
                    noneObj.SetActive(false);
                    swordObj.SetActive(true);
                    break;

                case "bow":
                    axeObj.SetActive(false);
                    bowObj.SetActive(true);
                    noneObj.SetActive(false);
                    swordObj.SetActive(false);
                    break;

                case "none":
                    axeObj.SetActive(false);
                    bowObj.SetActive(false);
                    noneObj.SetActive(true);
                    swordObj.SetActive(false);
                    break;

            }
        }
        
        
	}
}
