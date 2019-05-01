using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMoverment2 : MonoBehaviour
{

    #region variables
    //PHOTON TRANSFORM VIEW compenent syncronizes all the changes within our transform across the network 
    //as a result we can create this player movement script and we dont need to send messages to our clients because photon transform view takes care of that for us 
    private PhotonView PV;
    //public float rotationSpeed;

    public float turnAmount;

    //public bool isAccelerating;

    public float movementSpeed, acceleration, maxMovementSpeed;


    public Rigidbody2D ship;

    //public float handlingSpeed;



    public ParticleSystem PS;
    private bool canMove;
    private bool canRotate;

    //public int turnDirection;
    //public int turnDir;
    //float mouseDistance = 0;
    //private bool trackMouse = false;
    //private Vector3 lastPosition;
    //public int leftTouchID;
    //public int rightTouchID;
    //float fingerDistance = 0;
    #endregion

    private void OnEnable()
    {
        //Goal.OnBallEnteredGoal += Goal_OnBallEnteredGoal;
        Goal.OnGoalScored += Goal_OnGoalScored;
        //GoalManager.OnGameStateChanged += GoalManager_OnGameStateChanged;

    }



    private void Goal_OnBallEnteredGoal(Transform obj)
    {
        //ship.velocity = new Vector2(0f, 0f);
        //PV.RPC("respawnPlayer", RpcTarget.AllBuffered);

    }

    private void Goal_OnGoalScored(Enums.Teams obj)
    {
        //throw new NotImplementedException();
        ship.velocity = new Vector2(0f, 0f);
        PV.RPC("RPC_RespawnPlayer", RpcTarget.AllBuffered);
    }

    void Start()
    {
        ship = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();

    }

    // Update is called once per frame
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
            ship.AddTorque(-ControlManager.CM.turnDir * turnAmount);
            ship.AddForce(ship.transform.up * movementSpeed);
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
        PhotonView.Find(PSToToggle).gameObject.GetComponentInChildren<ParticleSystem>().enableEmission = setActive;

    }


    [PunRPC]
    void RPC_RespawnPlayer()
    {
        //if(PV.IsMine)
        //{
            if (GetComponent<AvatarSetup>().team == 1) //blue team
            {
            //transform.position = GameSetup.GS.spawnPointsBlueTeam[0].position;
            ship.position = GameSetup.GS.spawnPointsBlueTeam[0].position;


        }
        else
            {
            //transform.position = GameSetup.GS.spawnPointsOrangeTeam[0].position;
            ship.position = GameSetup.GS.spawnPointsOrangeTeam[0].position;

        }
        //}


    }

    void GoalManager_OnGameStateChanged(bool state)
    {
        Debug.Log("game state changed to " + state);
        canMove = state;
        if (!state)
        {
            ship.velocity = Vector2.zero;
            movementSpeed = 0;
            //acceleratorGague.fillAmount = 0;
            ControlManager.CM.isAccelerating = false;
        }

    }




}
