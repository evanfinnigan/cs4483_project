using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinBattle : MonoBehaviour
{

    public GameObject enemy;

    public GameObject winCanvas;
    public Image fadeImage;

    public GameObject textPanel;

    public GameObject button;

    public AudioSource buttonSound;
    public AudioSource music;
    public AudioSource ambientSound;
    public AudioClip winMusic;

    public bool runFixedUpdate = true;

    bool won = false;

    private void FixedUpdate()
    {
        if (!runFixedUpdate) {
            return;
        }

        if (enemy == null && !won)
        {
            Debug.Log("Win win win ");
            won = true;
            Win();
        }
    }

    public void Win()
    {
        winCanvas.SetActive(true);
        StartCoroutine(WinCoroutine());
    }

    public IEnumerator WinCoroutine()
    {

        while (fadeImage.color.a < 0.8f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + Time.deltaTime);
            music.volume -= Time.deltaTime;
            ambientSound.volume -= Time.deltaTime;
            yield return null;
        }

        music.volume = 0f;
        ambientSound.volume = 0f;
        music.Stop();
        ambientSound.Stop();

        yield return new WaitForSeconds(0.3f);

        buttonSound.Play();
        textPanel.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        buttonSound.Play();
        button.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        music.clip = winMusic;
        music.volume = 0.5f;
        music.Play();

        GameState g = FindObjectOfType<GameState>();
        if (g != null)
        {
            // Win, get some gold
            g.AddGold(1000);
        }

        yield return null;

    }

    public void BtnContinue()
    {
        Debug.Log("hi");
        SceneManager.LoadScene("main");
    }
}
