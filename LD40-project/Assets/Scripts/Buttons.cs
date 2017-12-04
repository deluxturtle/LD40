using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	public void _StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void _MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
