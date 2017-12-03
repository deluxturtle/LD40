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
    public GameObject loadingText;
    //public List<TextAsset> levels = new List<TextAsset>();
    public TextAsset mapInformation;
    int currentLevel = 0;
    XMLImport levelImporter;


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
        mainCamera = Camera.main;
        levelImporter = GetComponent<XMLImport>();
        StartLevelProcess();
    }

    public void StartLevelProcess()
    {
        StartCoroutine(LoadLevelProcess());
    }

    IEnumerator LoadLevelProcess()
    {
        loadingText.SetActive(true);
        yield return StartCoroutine(levelImporter.LoadTiles(mapInformation));
        //Level is done do some pre stuff before showing the level.
        
        loadingText.SetActive(false);
        yield return new WaitForSeconds(2f);
        FadeIn(1);
    }

    public void EndLevelProcess()
    {
        FadeOut(1);
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
    }
#endregion

}
