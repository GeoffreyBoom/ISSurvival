using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class AliensDeathListener : Photon.MonoBehaviour {


    private int health = 3; 

    void Start()
    {
        if(this.gameObject.tag == "Queen")
        {
            health = 30;
        }
    }

    void Update()
    {
        if(health == 0)
        {
            if(this.gameObject.tag == "Queen")
            {
                //RTS player won the game 
                Debug.Log("RTS player won the game");
            }else
            {
                photonView.RPC("deleteThis", PhotonTargets.MasterClient, this.gameObject.name);
            }
        }

        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health--;
        }
    }

    [PunRPC]
    void deleteThis(string dot)
    {
        if (gameObject.name == dot)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
