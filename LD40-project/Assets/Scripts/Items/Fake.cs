using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake : Interactable {

    public float poisonSpeed = 15f;
    public GameObject duplicatedProp;
    List<GameObject> myDuplicates = new List<GameObject>();

    public override void DoInteract()
    {
        isInteractable = false;
        InvokeRepeating("DuplicateSelf", 0, 0.25f);
        GameObject.FindObjectOfType<GameDirector>().StartCountdown(poisonSpeed);
        base.DoInteract();
    }

    void DuplicateSelf()
    {
        GameObject tempObj = Instantiate(duplicatedProp, transform.position, Quaternion.identity);
        myDuplicates.Add(tempObj);
        float x = Random.Range(-10, 10);
        float y = Random.Range(-10, 10);
        tempObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y).normalized);
    }

    private void OnDestroy()
    {
        while (myDuplicates.Count > 0)
        {
            GameObject objToRemove = myDuplicates[0];
            myDuplicates.Remove(objToRemove);
            Destroy(objToRemove);
        }
    }

}
