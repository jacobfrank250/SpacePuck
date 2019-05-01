using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void onClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySlectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }
}
