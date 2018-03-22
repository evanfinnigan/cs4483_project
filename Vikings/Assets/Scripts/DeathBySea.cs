using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathBySea : MonoBehaviour
{

    public GameObject deathCanvas;
    public Image fadeImage;

    public GameObject textPanel;

    public GameObject button1;
    public GameObject button2;

    public AudioSource buttonSound;
    public AudioSource music;
    public AudioClip deathMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = null;
        if ((player = collision.GetComponent<PlayerController>()) != null)
        {
            deathCanvas.SetActive(true);
            StartCoroutine(DrownCoroutine());
            player.KillPlayer();
        }
    }

    public IEnumerator DrownCoroutine()
    {

        while (fadeImage.color.a < 0.8f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + Time.deltaTime);
            music.volume -= Time.deltaTime;
            yield return null;
        }

        music.volume = 0f;
        music.Stop();

        yield return new WaitForSeconds(0.3f);

        buttonSound.Play();
        textPanel.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        buttonSound.Play();
        button1.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        buttonSound.Play();
        button2.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        music.clip = deathMusic;
        music.volume = 0.5f;
        music.Play();

        yield return null;

    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Sea Monster");
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("main");
    }
}
