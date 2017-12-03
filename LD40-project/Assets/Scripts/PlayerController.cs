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

    public List<AudioClip> footSteps = new List<AudioClip>();

    public GameObject toolTipCanvasPrefab;
    public GameObject soundCrumbPrefab;

    GameObject toolTipCanvas;
    float speed;
    Vector2 inputVector;
    bool running = false;
    bool interact = false;

    private void Start()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
        toolTipCanvas = Instantiate(toolTipCanvasPrefab);
        toolTipCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        HandleInput();

        if (running)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        //Currently no facing is needed. 
        if(!running)
        {
            foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable"))
            {
                if(Vector2.Distance(interactable.transform.position, transform.position) < interactDistance)
                {
                    toolTipCanvas.transform.position = (Vector2)interactable.transform.position + new Vector2(0, 0.5f);
                    toolTipCanvas.SetActive(true);
                    if(interact)
                        interactable.GetComponent<Interactable>().DoInteract();

                }
                else
                {
                    toolTipCanvas.SetActive(false);
                }
                break;
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
        if(inputVector.magnitude > 1)
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
    
    void CreateSoundCrumb(AudioClip pClip, float pVolume)
    {
        AudioSource source = Instantiate(soundCrumbPrefab).GetComponent<AudioSource>();
        source.volume = pVolume;
        source.clip = pClip;
        source.Play();
    }
}
