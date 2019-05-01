using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;

    private void Awake()
    {
        lobby = this; // Creates the singleton, which lives within the main menu scene 
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //connects to master photon server
    }

    public override void  OnConnectedToMaster()
    {
        Debug.Log("Player has connected to master photon server");
        PhotonNetwork.AutomaticallySyncScene = true; //when master client loads scene, all other clients connected to master will also load that scene.
        battleButton.SetActive(true);
    }

    public void onBattleButtonClicked()
    {
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }
    public void onCancelButtonClicked()
    {
        battleButton.SetActive(true);
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();  
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("tried to join random room but failed. there must be no rooms available");

        CreateRoom();
    }
    void CreateRoom()
    {
        Debug.Log("trying to create a room");
        int randomRoomName = Random.Range(0, 10000); 
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers =(byte)MultiplayerSettings.multiplayerSetting.maxPlayers};
        PhotonNetwork.CreateRoom("Random" + randomRoomName, roomOps);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("tried to create a room but failed. there must already be a room with that name");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("room joined");
    }
   
}
