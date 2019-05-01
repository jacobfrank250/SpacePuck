using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ship 
{
    public float Mass;
    public float Acceleration;
    //We will keep angular drag and linear drag constant for each vehicle and only change playermovement.maxspeed and playermovement.turnspeed.
    public float Speed; 
    public float Handling;
    public Sprite ShipSprite;

}
