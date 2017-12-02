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
        levelImporter = GetComponent<XMLImport>();
        StartLevelProcess();
    }

    public void StartLevelProcess()
    {
        StartCoroutine(LoadLevelProcess());
    }

    IEnumerator LoadLevelProcess()
    {
        yield return StartCoroutine(levelImporter.LoadTiles(mapInformation));
        loadingText.SetActive(false);
        yield return new WaitForSeconds(2f);
        FadeIn(1);
    }

    void FadeIn(float length)
    {
        StartCoroutine(FadeInLoop(length));
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

}
