using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *This script is attached to the plane in the testCamera scene. it enables and disable cameras based on the player selection from the UI scene.
 * DataHolder.player is a static variable which is set to false if the player selection is TPS, true otherwise.  
 * 
 */

public class Instantiator : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Camera currentCamera = null;

        if (DataHolder.player == false)
        {
            currentCamera = GameObject.Find("TPS").GetComponent<Camera>();

            currentCamera.enabled = true;

            GameObject.Find("RTS").gameObject.SetActive(false);
            GameObject.Find("RTSInterface").gameObject.SetActive(false);
            GameObject.Find("Mini Map RTS").gameObject.SetActive(false);
         }
        else 
        {
            currentCamera = GameObject.Find("RTS").GetComponent<Camera>();

            currentCamera.enabled = true;

            GameObject.Find("TPS").gameObject.SetActive(false);
            GameObject.Find("TPSInterface").gameObject.SetActive(false);
            GameObject.Find("Mini Map TPS").gameObject.SetActive(false);

        }
    }
}
