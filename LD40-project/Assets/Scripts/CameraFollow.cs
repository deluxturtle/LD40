using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    Transform target;

    // Update is called once per frame
    void Update ()
    {
        //Vector2 targetDir = target.transform.position - transform.position;
        //targetDir.Normalize();

        //Simple Follow //@TODO: Make a smooth follow.
        if(target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        }
	}

    public void SetTarget(Transform pTarget)
    {
        target = pTarget;
    }
}
