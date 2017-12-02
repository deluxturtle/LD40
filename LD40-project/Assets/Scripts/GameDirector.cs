using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Description: Manages the start and ends of the level
/// </summary>
public class GameDirector : MonoBehaviour
{

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

    public void Awake()
    {
        levelImporter = new XMLImport();

    }

    public void StartLevel()
    {
        levelImporter.LoadNewLevel();
    }

}
