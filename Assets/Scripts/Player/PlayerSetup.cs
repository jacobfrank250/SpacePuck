using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerSetup : MonoBehaviour
{
    public int team;
    public PhotonView PV;

    public bool shipAdded;

    // Start is called before the first frame update
    void Start()
    {
        shipAdded = false;
        if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeamFromMaster", RpcTarget.MasterClient);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (team != 0 && !shipAdded) //prevents us from executing this code after we have added a ship and before we know what team a player belongs to 
        {
            int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsBlueTeam.Length);
            if (team == 1)
            {
                if (PV.IsMine)
                {
                    PV.RPC("moveToStartPosition", RpcTarget.AllBuffered, GameSetup.GS.spawnPointsBlueTeam[spawnPicker].position, GameSetup.GS.spawnPointsBlueTeam[spawnPicker].rotation);
                    PV.RPC("RPC_AddShip", RpcTarget.AllBuffered, PlayerInfo.PI.mySlectedCharacter);
                }

            }
            else
            {
                if (PV.IsMine)
                {
                    PV.RPC("moveToStartPosition", RpcTarget.AllBuffered, GameSetup.GS.spawnPointsOrangeTeam[spawnPicker].position, GameSetup.GS.spawnPointsOrangeTeam[spawnPicker].rotation);
                    PV.RPC("RPC_AddShip", RpcTarget.AllBuffered, PlayerInfo.PI.mySlectedCharacter);
                }
            }

        }

    }

    [PunRPC]
    void RPC_GetTeamFromMaster() //only executed on MASTER CLIENT
    {
        team = GameSetup.GS.nextPlayersTeam;
        GameSetup.GS.updateTeam(); //updates the value of the next players team 
        //master client now knows what team this player is on. Now we must call another RPC to tell the other clients
        PV.RPC("RPC_SetTeam", RpcTarget.OthersBuffered, team);
    }

    [PunRPC]
    void RPC_SetTeam(int whichTeam)
    {
        team = whichTeam;
    }

    [PunRPC]
    void RPC_AddShip(int whichCharacter) //the value of which character we have selected is the data we are trying to sync across the network
    {
       
        GetComponent<SpriteRenderer>().sprite = PlayerInfo.PI.ships[whichCharacter].ShipSprite; //add the sprite for the ship that the character picked to our game object.
        this.gameObject.AddComponent<PolygonCollider2D>(); // must add the collider after so it automatically fits the sprite--it does not do this if we add it before.
        shipAdded = true; 
    }

    [PunRPC]
    void moveToStartPosition(Vector3 startPos, Quaternion startRot)
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
