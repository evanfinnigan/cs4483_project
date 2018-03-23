using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    // From wikipedia: Karvi
    // The Karvi(or karve) is the smallest vessel that is considered a longship.
    // According to the 10th-century Gulating Law, a ship with 13 rowing benches 
    // is the smallest ship suitable for military use.A ship with 6 to 16 benches 
    // would be classified as a Karvi. These ships were considered to be "general 
    // purpose" ships, mainly used for fishing and trade, but occasionally 
    // commissioned for military use. While most longships held a length to width 
    // ratio of 7:1, the Karvi ships were closer to 4.5:1.[citation needed] The 
    // Gokstad Ship is a famous Karvi ship, built around the end of the 9th century, 
    // excavated in 1880 by Nicolay Nicolaysen.It was approximately 23 m (75 feet) 
    // long with 16 rowing positions.
    private static int maxCrew = 16;
    private static int crew = 16;

    public enum Weapon
    {
        none,
        sword,
        axe,
        bow
    }

    private static Weapon equippedWeapon = Weapon.none;

    private static int gold;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Save / Load

    public void LoadStats()
    {
        if (PlayerPrefs.HasKey("crew"))
        {
            crew = PlayerPrefs.GetInt("crew");
        }

        if (PlayerPrefs.HasKey("weapon"))
        {
            equippedWeapon = (Weapon)PlayerPrefs.GetInt("weapon");
        }

        if (PlayerPrefs.HasKey("gold"))
        {
            gold = PlayerPrefs.GetInt("gold");
        }
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("crew", crew);
        PlayerPrefs.SetInt("weapon", (int)equippedWeapon);
        PlayerPrefs.SetInt("gold", gold);
    }

    // Crew
    
    public int GetCrew()
    {
        return crew;
    }

    public void SetCrew(int c)
    {
        crew = c;
    }

    public int AddCrew()
    {
        crew++;
        if (crew > maxCrew)
        {
            crew = maxCrew;
        }
        return crew;
    }

    public int RemoveCrew()
    {
        crew--;
        if (crew < 0)
        {
            crew = 0;
        }
        return crew;
    }

    // Weapon

    public Weapon GetWeapon()
    {
        return equippedWeapon;
    }

    public void SetWeapon(Weapon w)
    {
        equippedWeapon = w;
    }

    // Gold

    public int GetGold()
    {
        return gold;
    }

    public void SetGold(int g)
    {
        gold = g;
    }

    public int AddGold(int g)
    {
        gold += g;
        return gold;
    }

    public int SpendGold(int g)
    {
        if (gold >= g)
        {
            gold -= g;
            return gold;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return -1; // indicates failure
        }
    }

}
