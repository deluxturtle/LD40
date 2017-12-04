using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomFootStep : MonoBehaviour {

    public List<AudioClip> footSteps = new List<AudioClip>();
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

    }

    public void PlayFoot()
    {
        int random = Random.Range(0, 2);

        source.clip = footSteps[random];
        source.Play();
    }
}
