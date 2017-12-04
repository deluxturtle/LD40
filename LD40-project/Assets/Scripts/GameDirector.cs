using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Andrew Seba
/// Description: Manages the start and ends of the level
/// </summary>
public class GameDirector : MonoBehaviour
{
    public Camera mainCamera;
    public Image fadeToBlackPanel;
    public Image winPanel;
    public GameObject loadingText;
    //public List<TextAsset> levels = new List<TextAsset>();
    public TextAsset mapInformation;
    public AudioClip backgroundMusic;
    public List<TextAsset> levels = new List<TextAsset>();
    public GameObject countDownTimer;
    public GameObject goalText;

    int currentLevel = 0;
    XMLImport levelImporter;
    AudioSource audioSource;

    float countDownTime = 15f;
    bool timerGoing = false;


    //Start level
        //Keep screen Black
        //Load in level
        //Reset Start Position
        //Fade to regular

    //GameLoop

    //End Level
        //Fade to black
        //Return to start of this.
        
    private void Start()
    {
        countDownTimer.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        levelImporter = GetComponent<XMLImport>();
        StartLevelProcess();

        if(backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }

    public void StartCountdown(float timeLeft)
    {
        if (timerGoing)
            return;

        timerGoing = true;
        countDownTime = timeLeft;
        countDownTimer.SetActive(true);
        countDownTimer.GetComponent<Text>().text = countDownTime + "s till death..";
        InvokeRepeating("DeductSecond", 0, 1);
    }

    void DeductSecond()
    {
        countDownTime--;
        countDownTimer.GetComponent<Text>().text = countDownTime + "s till death..";
        if(countDownTime < 0)
        {
            Application.Quit();
        }
    }



    public void StartLevelProcess()
    {
        StartCoroutine(LoadLevelProcess());
    }

    IEnumerator LoadLevelProcess()
    {
        loadingText.SetActive(true);
        yield return StartCoroutine(levelImporter.LoadTiles(levels[currentLevel]));
        //Level is done do some pre stuff before showing the level.
        
        loadingText.SetActive(false);
        yield return new WaitForSeconds(2f);
        FadeIn(1);
    }

    public IEnumerator EndLevelProcess()
    {
        //Stop Timer
        countDownTimer.SetActive(false);
        timerGoing = false;
        CancelInvoke("DeductSecond");

        currentLevel++;
        FadeOut(1);
        yield return new WaitForSeconds(1);

        //LoadNextLevel
        if(levels.Count < currentLevel)
        {
            EndOfGame();
        }
        else
        {
            StartCoroutine(LoadLevelProcess());
        }
    }

    void EndOfGame()
    {
        StartCoroutine(GetComponent<XMLImport>().DeleteMap());
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        winPanel.gameObject.SetActive(true);
    }

#region FadeEffects
    void FadeIn(float length)
    {
        StartCoroutine(FadeInLoop(length));
    }

    void FadeOut(float length)
    {
        StartCoroutine(FadeOutLoop(length));
    }
    IEnumerator FadeOutLoop(float length)
    {
        fadeToBlackPanel.gameObject.SetActive(true);
        float timePassed = 0;
        while(timePassed <= length)
        {
            timePassed += Time.deltaTime;
            fadeToBlackPanel.color = Color.Lerp(Color.clear, Color.black, timePassed / length);
            yield return null;
        }
    }

    IEnumerator FadeInLoop(float length)
    {
        float timePassed = 0;
        

        while (timePassed <= length)
        {
            timePassed += Time.deltaTime;
            fadeToBlackPanel.color = Color.Lerp(Color.black, Color.clear, timePassed / length);
            yield return null;
        }
        fadeToBlackPanel.gameObject.SetActive(false);
        goalText.SetActive(true);
    }
#endregion

}
