using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PlayerDeathListener : Photon.MonoBehaviour {

    /*
    void Update()
    {
        if(GameObject.Find("").active == true)
        {
            if (GameObject.Find("").GetComponent<"RTS">().currentHealth <= 0)
            {
                Debug.Log("RTS Player is dead");

            }
        }
    }*/

    [PunRPC]
    void deleteThis(string dot)
    {
        if (gameObject.name == dot)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
