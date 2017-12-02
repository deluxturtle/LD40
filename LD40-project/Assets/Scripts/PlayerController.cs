using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Description: Controls the player.
/// </summary>
public class PlayerController : MonoBehaviour {

    public float speed = 3f;
    Vector2 inputVector;
	
	// Update is called once per frame
	void Update ()
    {
        HandleInput();

        transform.position += (Vector3)inputVector * speed * Time.deltaTime;
	}

    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        inputVector = new Vector2(horizontal, vertical);
        if(inputVector.magnitude > 1)
        {
            inputVector.Normalize();
        }
    }
}
