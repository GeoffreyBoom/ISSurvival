using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

public class AliensDeathListener : Photon.MonoBehaviour {


    private int health = 3;
    Animator anim;


    void Start()
    {
        if(this.gameObject.tag == "Queen")
        {
            health = 30;
        }
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health == 0)
        {
            anim.SetBool("dead", true);

            if (this.gameObject.tag == "Queen")
            {
                //RTS player won the game 
                GameObject winnerScreen = GameObject.FindGameObjectWithTag("Screen");
                winnerScreen.SetActive(true);
                GameObject.Find("winnerText").GetComponent<Text>().text = "|SURVIVOR|"; 
                
                Debug.Log("RTS player won the game");
            }
            else
            {
                photonView.RPC("deleteThis", PhotonTargets.MasterClient, this.gameObject.name);
            }
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            anim.SetBool("hit", true);
            health--;
        }

        anim.SetBool("hit", false);

        if (anim.GetBool("start") == false)
        {
            anim.SetBool("start", true);
        }
        else
        {
            anim.SetBool("start", false);
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
