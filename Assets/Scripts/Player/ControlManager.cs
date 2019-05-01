using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public static ControlManager CM;

    public int turnDir;
    public bool isAccelerating;





    private void OnEnable()
    {
        if (ControlManager.CM == null)
        {
            ControlManager.CM = this;
        }
        else
        {
            if (ControlManager.CM != this) //if the current instance of this class does not equal the singleton
            {
                //Reset the singleton (to the new value of "this")
                Destroy(ControlManager.CM.gameObject);
                ControlManager.CM = this;
            }
        }
        DontDestroyOnLoad(this.gameObject); //we want to make sure this persists between scenes


    }

   

    //Handles UI button events
    public void onLeftButtonDown()
    {
        turnDir = -1;
    }

    public void onLeftButtonUp()
    {

        turnDir = 0;
    }

    public void onRightButtonDown()
    {
        turnDir = 1;
    }

    public void onRightButtonUp()
    {
        turnDir = 0;
    }

    public void onAccelerateButtonDown()
    {
        isAccelerating = true;
    }

    public void onAccelerateButtonUp()
    {
        isAccelerating = false;
    }




}
