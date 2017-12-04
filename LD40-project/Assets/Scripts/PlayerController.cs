using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Description: Controls the player.
/// </summary>
public class PlayerController : MonoBehaviour {

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float interactDistance = 2f;
    
    public GameObject toolTip;

    Animator animator;
    GameObject toolTipCanvas;
    float speed;
    Vector2 inputVector;
    bool running = false;
    bool interact = false;

    private void Start()
    {
        toolTip = GameObject.FindGameObjectWithTag("ToolTip");
        animator = GetComponentInChildren<Animator>();
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
        toolTip.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        HandleInput();
        

        if (running)
        {
            speed = runSpeed;
            animator.SetFloat("moveSpeed", 6);//These didn't go the way i planned but it will work.. :/
        }
        else
        {
            speed = walkSpeed;
            animator.SetFloat("moveSpeed", 3);
        }

        if(inputVector.magnitude <= 0.1f)
        {
            animator.SetFloat("moveSpeed", 0);
        }



        //Pick up things
        if (!running)
        {
            GameObject closest = null;
            foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable"))
            {
                if(closest == null)
                {
                    closest = interactable;
                }
                else if( Vector2.Distance(transform.position, interactable.transform.position) < Vector2.Distance(closest.transform.position, transform.position))
                {
                    closest = interactable;
                }


                if(Vector2.Distance(closest.transform.position, transform.position) < interactDistance)
                {
                    if (closest.GetComponent<Interactable>().isInteractable)
                    {
                        toolTip.transform.position = (Vector2)closest.transform.position + new Vector2(0, -0.5f);
                        toolTip.SetActive(true);
                        if (interact)
                        {
                            closest.GetComponent<Interactable>().DoInteract();
                            toolTip.SetActive(false);
                        }


                    }

                }
                else
                {
                    toolTip.SetActive(false);
                }
            }
        }

	}

    private void FixedUpdate()
    {
        transform.position += (Vector3)inputVector * speed * Time.deltaTime;
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        inputVector = new Vector2(horizontal, vertical);
        if (inputVector.magnitude > 1)
        {
            inputVector.Normalize();
        }

        if (Input.GetButton("Fire3"))
            running = true;
        else
            running = false;

        if (Input.GetButtonDown("Jump"))
            interact = true;
        else
            interact = false;
    }
    

}
