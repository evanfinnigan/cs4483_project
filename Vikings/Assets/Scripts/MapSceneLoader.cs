using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneLoader : MonoBehaviour {

	public void Port()
    {
        SceneManager.LoadScene("land");
    }

    public void Battle()
    {
        SceneManager.LoadScene("Sea Monster");
    }

}
