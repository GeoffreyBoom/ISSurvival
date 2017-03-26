using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkManager : Photon.PunBehaviour {

    private string VERSION = "v0.0.1";
    public string roomName = "ISS";

    [SerializeField]
    string playerPrefabName = "";



    // Use this for initialization
    void Start () {

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
        //create a room for multiplayer:
      /*  RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 2; // 2 player per room 
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);*/
    }


    void OnPhotonRandomJoinFailed()
    {

        Debug.Log("Can't join random room!  DON'T MIND ME, I'LL JUST MAKE MY OWN ROOM.");
        PhotonNetwork.CreateRoom(null);
    }


    //Spawn player:
    void OnJoinedRoom()
    {

        Debug.Log(DataHolder.player);
         
        if (DataHolder.player == false)
        {
            Debug.Log("I am here");
        //when we want to instantiate objects that we want ALL player to see:
        PhotonNetwork.Instantiate(playerPrefabName, this.transform.position, this.transform.rotation, 0);
        }
    
    }
}
