using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    //Singleton
    public static MultiplayerSettings multiplayerSetting;

    public bool delayStart; //if false then we have a continuous loading game. Otherwise we have a delayed start game.

    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if (MultiplayerSettings.multiplayerSetting == null)
        {
            MultiplayerSettings.multiplayerSetting = this;
        }
        else
        {
            if(MultiplayerSettings.multiplayerSetting != this)
            {
                Destroy(this.gameObject); 
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
