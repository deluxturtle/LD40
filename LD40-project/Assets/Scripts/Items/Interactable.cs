using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public bool isInteractable = true;
    public AudioClip pickupSound;
    AudioSource source;

	public virtual void DoInteract()
    {
        if(GetComponent<AudioSource>() == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = pickupSound;
        }
        source.Play();
    }
}
