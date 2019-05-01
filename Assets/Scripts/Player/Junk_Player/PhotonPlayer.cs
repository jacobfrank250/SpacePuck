using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView PV;

    public GameObject myAvatar;

    //public GameObject myCam;


    public int myTeam;
    // Start is called before the first frame update
    void Start()
    {
        //PV = this.GetComponent<PhotonView>();
        if(PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }

        //PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);


    }

    void Update()
    {
        if(myAvatar == null && myTeam != 0) //prevents us from executing this code after we have instanciated an avatar and before we know what team a player belongs to 
        {
            if (myTeam == 1)
            {
                PV.RPC("RPC_DebugLog", RpcTarget.AllBuffered, "myTeam=1");
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsBlueTeam.Length);
                if (PV.IsMine)
                {
                    Debug.Log("team 1 minne");
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AvatarController"), GameSetup.GS.spawnPointsBlueTeam[spawnPicker].position, GameSetup.GS.spawnPointsBlueTeam[spawnPicker].rotation, 0);
                    PV.RPC("RPC_DebugLog", RpcTarget.AllBuffered, "the pv id is " + PV.ViewID);
                    //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), GameSetup.GS.spawnPointsBlueTeam[spawnPicker].position, GameSetup.GS.spawnPointsBlueTeam[spawnPicker].rotation, 0);

                }
                else
                {
                    Debug.Log("myTeam = 1 but pv not mine");
                }
            }
            else
            {
                PV.RPC("RPC_DebugLog", RpcTarget.AllBuffered, "myTeam=2");
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsOrangeTeam.Length);
                if (PV.IsMine)
                {
                    Debug.Log("team 2 minne");

                    myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AvatarController"), GameSetup.GS.spawnPointsOrangeTeam[spawnPicker].position, GameSetup.GS.spawnPointsOrangeTeam[spawnPicker].rotation, 0);
                    PV.RPC("RPC_DebugLog", RpcTarget.AllBuffered, "the pv id is " + PV.ViewID);

                    //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), GameSetup.GS.spawnPointsOrangeTeam[spawnPicker].position, GameSetup.GS.spawnPointsOrangeTeam[spawnPicker].rotation, 0);
                }
                else
                {
                    Debug.Log("myTeam = 2 but pv not mine");
                }
            }
            Debug.Log("do we get here?");
            myAvatar.GetComponentInChildren<AvatarSetup>().team = myTeam;

            //myCam = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerCamera"), new Vector3(0,0,-40), new Quaternion(0,0,0,0), 0);
        }
        else
        {
            if(myAvatar ==null)
            {
                Debug.Log("myavatar == null", this.gameObject);
                Debug.Log("my team = " + myTeam,this.gameObject);
            }
            else
            {
                //Debug.Log("My avatar is not null?", this.gameObject);
            }
        }
    }

    //local player sends request to master client to set what team they are on. Then master client tells other players what 

    [PunRPC]
    void RPC_GetTeam() //only executed on MASTER CLIENT
    {
        myTeam = GameSetup.GS.nextPlayersTeam; 
        GameSetup.GS.updateTeam(); //updates the value of the next players team 
        //master client now knows what team this player is on. Now we must call another RPC to tell the other clients
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }

    [PunRPC]
    void RPC_DebugLog(string message)
    {
        Debug.Log(message);
    }
}
