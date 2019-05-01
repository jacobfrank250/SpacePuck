using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks,IInRoomCallbacks 
{
    //Room Info 
    public static PhotonRoom room; //singleton
    private PhotonView PV; //Photon Views are used for sending messages from one client to another using RPCs (remote procedural calls)
    public bool isGameLoaded;
    public int currentScene;

    //Player Info
    private Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    //Delayed Start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;


    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;

            } 
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += onSceneFinishedLoading; //setting up event listener for whenver we load a new scene. Whenever we call a new scene we call our onSceneFinishedLoading method.
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= onSceneFinishedLoading;
    }

    // Here we initialize our private variables 
    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6; //if our game is filled we want to count down from 5 (in order to do that we must start at 6)
        timeToStart = startingTime;
        
    }
   
    // Inside our update method we only do code that pertains to a delayed  start 
    void Update()
    {
        if (MultiplayerSettings.multiplayerSetting.delayStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart) //if we're ready to start the count down till the game start, then we are decrementing time. Once the timer reaches zero we start the game (line 100)
                {
                    atMaxPlayers -= Time.deltaTime; //atMaxPlayers -> currentWaitTimeToStartGameWhenRoomIsFull
                    lessThanMaxPlayers = atMaxPlayers; //lessThanMaxPlayers -> "currentWaitTimeForPlayers"
                    timeToStart = atMaxPlayers;  //atMaxPlayers -> currentWaitTimeToStartGameWhenRoomIsFull
                }
                else if (readyToCount) //readyToCount -> "readyToWaitForPlayers"
                {
                    lessThanMaxPlayers -= Time.deltaTime; //lessThanMaxPlayers -> "currentWaitTimeForPlayers"
                    timeToStart = lessThanMaxPlayers;
                }
                Debug.Log("Display time to start to the players " + timeToStart);
                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }

    }

    //callback function
    //called whenever we join a room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("we are now in a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        //This sections deals with delayed start
        if(MultiplayerSettings.multiplayerSetting.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSetting.maxPlayers + ")");

            if(playersInRoom>1) //if the room has at least 2 people (multiplayers) we start the count down. readyToCount -> readyToWaitForPlayers
            {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSettings.multiplayerSetting.maxPlayers) //if our room is full we are ready to start  
            {
                readyToStart = true;
                if(!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                //if we are the master client
                PhotonNetwork.CurrentRoom.IsOpen = false; //this closes the room so that no new players can join the room until we reopen it
            }
        }
        else  //Continuous Loading Game. If we are not doing a delayed start then we go right into starting the game
        {
            StartGame();
        }
    }

    //callback function
    //called whenever another person joins our room
    //essentially the same as our OnJoinedRoom method
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room.");
        photonPlayers = PhotonNetwork.PlayerList; //update our list of players
        playersInRoom++; //update our players in room counter 

        //This sections deals with delayed start
        if (MultiplayerSettings.multiplayerSetting.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSetting.maxPlayers + ")");

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSetting.maxPlayers) //if our room is full we are ready to start  
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                //if we are the master client
                PhotonNetwork.CurrentRoom.IsOpen = false; //this closes the room so that no new players can join the room until we reopen it
            }
        }
    }

   
    //When we call this function it loads us into the multiplayer scene. This then calls the OnSceneFinishedLoadingMethod
    void StartGame() 
    {

        isGameLoaded = true;
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("not master client");
            return;
        }

        if(MultiplayerSettings.multiplayerSetting.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSetting.multiplayerScene);
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void onSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSetting.multiplayerScene)
        {
            isGameLoaded = true;

            //for a delayed start, send an rpc call to the master client, which will then send an rpc call to all the other clients, to end up creating their player game objects
            if(MultiplayerSettings.multiplayerSetting.delayStart)
            {
                //"RPC_LoadedGameScene" is a function
                //We are sending this RPC message to the master client
                PV.RPC("RPC_LoadedGameScene",RpcTarget.MasterClient); 
            }
            else //if we are not doing a delayed start and just jumping into a game we just send an rpc call to all clients to create a player game object for this player 
            {
                RPC_CreatePlayer();
               
            }
        }
    }

    [PunRPC]
    //This is called on master client, in the main scene. However, at this point the other players have not yet joined the game and entered the main scene. 
    //As a result, the master players player object is instantiated in the menu scene for the other players and gets destroyed when they enter the main scene
    private void RPC_LoadedGameScene()  
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length) //this makes sure we are not creating any duplicate player objects
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.AllBuffered); 
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        //instantiate a player prefab across the network
        //PhotonPrefabs is the name of the folder. PhotonNetorkPlayer is the name of the prefab
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), transform.position, Quaternion.identity, 0);

    }

    [PunRPC] 
    void RPC_CreateBall()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ball"), new Vector3(0,0,0), Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the room");
        playersInGame--;
    }
}
