using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;
    public int mySlectedCharacter;

    public GameObject[] allCharacters; //holds all of the posible characters we can choose from

    public Sprite[] shipSprites;

    public Ship[] ships;

    private void OnEnable()
    {
        if(PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else
        {
            if(PlayerInfo.PI != this) //if the current instance of this class does not equal the singleton
            {
                //Reset the singleton (to the new value of "this")
                Destroy(PlayerInfo.PI.gameObject);
                PlayerInfo.PI = this;
            }
        }
        DontDestroyOnLoad(this.gameObject); //we want to make sure this persists between scenes


    }

    void Start()
    {
        if(PlayerPrefs.HasKey("MyCharacter"))
        {
            mySlectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else //else we want to set the player prefs and mySelectedCharacter variable
        {

            mySlectedCharacter = 0; //this value will be updated in MenuController whenever the player selects a character in the menu scene 
            PlayerPrefs.SetInt("MyCharacter", mySlectedCharacter);
        }
    }

   
}
