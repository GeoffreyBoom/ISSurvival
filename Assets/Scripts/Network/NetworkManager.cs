using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkManager : Photon.PunBehaviour {

    private string VERSION = "v0.0.1";
    public string roomName = "ISS";

    [SerializeField]
    string playerPrefabName = "";
    [SerializeField]
    string queenPrefabName = "";

    // Use this for initialization
    void Start ()
    {

        PhotonNetwork.ConnectUsingSettings(VERSION);
        PhotonNetwork.autoJoinLobby = true;
    }

    void OnGUI()
    {

        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    void OnPhotonRandomJoinFailed()
    {

     //   Debug.Log("Can't join random room!  DON'T MIND ME, I'LL JUST MAKE MY OWN ROOM.");
        PhotonNetwork.CreateRoom(null);
    }


    //Spawn player:
    void OnJoinedRoom()
    {
         
        if (DataHolder.player == false)
        {
        //when we want to instantiate objects that we want ALL players to see:
           PhotonNetwork.Instantiate(playerPrefabName, this.transform.position, this.transform.rotation, 0);
        }
        else
        {
            PhotonNetwork.Instantiate(queenPrefabName, this.transform.position + new Vector3(2,0,2), this.transform.rotation, 0);
        }

    }
}
