using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue; //stores the whichCharacter value (parameter of RPC_AddCharacter) that we send across the network
    public GameObject myCharacter;

    public int playerHealth;
    public int playerDamage;

    //public Camera myCam;
    public AudioListener myAL;

    public int team;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) //if we are the local player
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySlectedCharacter); //AllBuffered allows players who have joined the game after this RPC call to still receive it
        }
       
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter) //the value of which character we have selected is the data we are trying to sync across the network
    {
        characterValue = whichCharacter; //save the value that we sent across the network
        //instantiate the character we have selected
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[whichCharacter], transform.position, transform.rotation, this.transform); //instantiates the character selected as a child of our avatar at the avatars position and rotation
    }

}