using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS; //singleton

    public Transform[] spawnPointsBlueTeam;

    public Transform[] spawnPointsOrangeTeam;


    public int nextPlayersTeam;

    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
   
    public void updateTeam()
    {
        if(nextPlayersTeam ==1)
        {
            nextPlayersTeam = 2;
        }
        else
        {
            nextPlayersTeam = 1;
        }
    }

    public void DiscconectPlayer()
    {
        Destroy(PhotonRoom.room.gameObject);
        StartCoroutine(DisconnectAndLoad()); 
    }

    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        //while(PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSetting.menuScene);
    }

}
