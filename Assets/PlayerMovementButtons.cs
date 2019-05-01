using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementButtons : MonoBehaviour
{
    public Rigidbody2D ship;

    //int turnDirection;
    public int turnAmount;
    public int turnSpeed;
    float rotateAngle;
    bool canRotate;
    bool isAccelerating;
    public ParticleSystem PS;
    public float movementSpeed, acceleration, maxMovementSpeed;


    void Start()
    {
        ship = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementSpeed();
        
    }

    void LateUpdate()
    {
        //ship.AddTorque(-turnDirection * turnAmount);
        ship.AddForce(ship.transform.up * movementSpeed);


    }



    void GetMovementSpeed()
    {
        if (isAccelerating && movementSpeed <= maxMovementSpeed)
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
    }




    ////Handles UI button events
    //public void onLeftButtonDown()
    //{
    //    canRotate = true;
    //    turnDirection = -1;
    //}

    //public void onLeftButtonUp()
    //{
    //    canRotate = false;
    //    turnDirection = 0;
    //}

    //public void onRightButtonDown()
    //{
    //    turnDirection = 1;
    //}

    //public void onRightButtonUp()
    //{
    //    turnDirection = 0;
    //}

    //public void onAccelerateButtonDown()
    //{
    //    isAccelerating = true;
    //    PS.enableEmission = true;
    //}

    //public void onAccelerateButtonUp()
    //{
    //    isAccelerating = false;
    //    PS.enableEmission = false;
    //}



}
