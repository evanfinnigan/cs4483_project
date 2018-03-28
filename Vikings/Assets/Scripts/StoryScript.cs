using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the way the story is displayed on the start
/// screen.
/// </summary>
public class StoryScript : MonoBehaviour
{

    private float beginTimer;
    private float elapsedTime;
    private bool isActive = true;

    public Text story;
    public TextAsset storyFile;
    public string[] textLines;
    public GameObject textBox;


    public float typeSpeed;
    public int currentLine;
    public int endAtLine;


	void Start ()
    {
        beginTimer = Time.time;

        if(storyFile != null)
        {
            textLines = (storyFile.text.Split('\n'));
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

	}
	

	void Update ()
    {
        if(isActive)
        {
            elapsedTime = Time.time - beginTimer;
            if (elapsedTime > 8.0f)
            {
                beginTimer = Time.time;
                textBox.SetActive(true);
                if (currentLine <= endAtLine)
                {
                    StartCoroutine(TextScroll(textLines[currentLine]));
                }
                    
            }
        }
        
	}

    /// <summary>
    /// Handles the scrolling.
    /// </summary>
    /// <param name="lineOfText"></param>
    /// <returns></returns>
    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        story.text = "";

        while(letter < lineOfText.Length - 1)
        {
            story.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        story.text = lineOfText;
        currentLine += 1;
        if(currentLine > endAtLine)
        {
            yield return new WaitForSeconds(2.0f);
            textBox.SetActive(false);
            isActive = false;
        }
            
        
    }
}
