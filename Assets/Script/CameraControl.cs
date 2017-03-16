using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform camPosAndRot;
    [SerializeField]
    float smoothSpeedCam = 0.1f;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position,camPosAndRot.position, smoothSpeedCam);
        transform.rotation = Quaternion.Slerp(transform.rotation, camPosAndRot.rotation, smoothSpeedCam);    
    }

    public void setNextSpace(Transform nextSpace)
    {     
        camPosAndRot = nextSpace;
    }

}
