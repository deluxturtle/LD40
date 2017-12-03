using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : Interactable {

    GameDirector gameDirector;


    private void Start()
    {
        gameDirector = FindObjectOfType<GameDirector>();

    }

    public override void DoInteract()
    {
        gameDirector.EndLevelProcess();
        base.DoInteract();
    }
}
