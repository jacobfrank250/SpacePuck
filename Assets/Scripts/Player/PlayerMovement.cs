using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region variables

    private PhotonView PV;

    public float turnAmount;

    public float movementSpeed, acceleration, maxMovementSpeed;


    public Rigidbody2D shipRB;


    //public ParticleSystem PS;
    private bool canMove;

    #endregion

    private void OnEnable()
    {
        GoalManager.OnGameStateChanged += GoalManager_OnGameStateChanged;

    }


    private void Goal_OnGoalScored(Enums.Teams obj)
    {
        shipRB.velocity = new Vector2(0f, 0f);
        PV.RPC("RPC_RespawnPlayer", RpcTarget.AllBuffered);
    }

    void Start()
    {
        shipRB = GetComponent<Rigidbody2D>();

        PV = GetComponent<PhotonView>();

        SetStats();
        canMove = true;

    }

    void SetStats()
    {
        shipRB.mass = PlayerInfo.PI.ships[PlayerInfo.PI.mySlectedCharacter].Mass;
        acceleration = PlayerInfo.PI.ships[PlayerInfo.PI.mySlectedCharacter].Acceleration;
        maxMovementSpeed = PlayerInfo.PI.ships[PlayerInfo.PI.mySlectedCharacter].Speed;
        turnAmount = PlayerInfo.PI.ships[PlayerInfo.PI.mySlectedCharacter].Handling;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            GetInputs();

        }
    }


    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            if(canMove)
            {
                shipRB.AddTorque(-ControlManager.CM.turnDir * turnAmount);
                shipRB.AddForce(shipRB.transform.up * movementSpeed);
            }
           
        }

    }


    void GetInputs()
    {
        GetMovementSpeed();
    }



    void GetMovementSpeed()
    {
        if (ControlManager.CM.isAccelerating && movementSpeed <= maxMovementSpeed)
        {
            movementSpeed += acceleration * Time.deltaTime;
        }
        else if (movementSpeed > 0)
        {
            movementSpeed -= acceleration * 2 * Time.deltaTime;
        }
        if (movementSpeed <= 0)
        {
            movementSpeed = 0;
        }
        PV.RPC("ToggleFlame", RpcTarget.AllBuffered, PV.ViewID, ControlManager.CM.isAccelerating);

    }

    [PunRPC]
    void ToggleFlame(int PSToToggle, bool setActive)
    {
        var emission = PhotonView.Find(PSToToggle).gameObject.GetComponentInChildren<ParticleSystem>().emission;
        emission.enabled = setActive;
    }


    [PunRPC]
    void RPC_RespawnPlayer()
    {
        if (GetComponent<PlayerSetup>().team == 1) //blue team
        {
            shipRB.position = GameSetup.GS.spawnPointsBlueTeam[0].position;
            shipRB.rotation = GameSetup.GS.spawnPointsBlueTeam[0].rotation.z;
        }
        else
        {
            shipRB.position = GameSetup.GS.spawnPointsOrangeTeam[0].position;
            shipRB.rotation = GameSetup.GS.spawnPointsOrangeTeam[0].rotation.z;
        }
    }

    void GoalManager_OnGameStateChanged(bool state)
    {
       
        PV.RPC("RPC_RespawnPlayer", RpcTarget.AllBuffered);
        canMove = state;
        if (!state)
        {
            shipRB.velocity = Vector2.zero;
            movementSpeed = 0;
            ControlManager.CM.isAccelerating = false;
        }

    }

}
