using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCrumb : MonoBehaviour {

    public float destroyDelay = 2f;

	// Use this for initialization
	void Start ()
    {
        Invoke("DestroySelf", destroyDelay);

    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
