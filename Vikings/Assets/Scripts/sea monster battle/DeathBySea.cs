using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathBySea : MonoBehaviour
{

    public GameObject playerObject;

    public GameObject deathCanvas;
    public GameObject deathByMonsterCanvas;
    public Image fadeImage;

    public GameObject textPanel;
    public GameObject monsterDeathTextPanel;

    public GameObject button1;
    public GameObject button2;

    public AudioSource buttonSound;
    public AudioSource music;
    public AudioClip deathMusic;

    private bool died = false;

    private void FixedUpdate()
    {
        if (playerObject == null && !died)
        {
            died = true;
            deathByMonsterCanvas.SetActive(true);
            StartCoroutine(DrownCoroutine(true));
            PlayerController player = null;
            if ((player = GetComponent<PlayerController>()) != null)
            {
                player.KillPlayer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = null;
        if ((player = collision.GetComponent<PlayerController>()) != null)
        {
            died = true;
            deathCanvas.SetActive(true);
            StartCoroutine(DrownCoroutine());
            player.KillPlayer();
        }
    }

    public IEnumerator DrownCoroutine(bool monsterDeath = false)
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
        if (monsterDeath)
        {
            monsterDeathTextPanel.SetActive(true);
        }
        else
        {
            textPanel.SetActive(true);
        }

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

        GameState g = GetComponent<GameState>();
        if (g != null)
        {
            // Die, lose a life
            g.RemoveCrew();
        }

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
