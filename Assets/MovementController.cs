using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private bool canRotate;
    private float turnAmount;

    public Rigidbody2D ship;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //if (!canMove) return;
        if (canRotate)
        {
            ship.rotation = ship.rotation + turnAmount;
        }
        //ship.MoveRotation(Mathf.LerpAngle(ship.rotation, -turnAmount * 360, handlingSpeed * Time.deltaTime));
        //ship.MoveRotation(-turnAmount * 360);
        //ship.MoveRotation(Mathf.LerpAngle(ship.rotation, ship.rotation+turnAmount, Time.deltaTime));
        //ship.MoveRotation(ship.rotation + 10*turnAmount);

        Debug.Log("ships rotation: " + ship.rotation);

        //ship.AddForce(ship.transform.up * movementSpeed);
    }


    public void OnTurnPressed(float amount)
    {
        //ship.rotation = 
        canRotate = true;
        Debug.Log("turning?");
        turnAmount = 10*amount;

    }

  

    public void OnTurnReleased()
    {
        //ship.rotation = 
        canRotate = false;
        //turnAmount = turnAmount;

    }
}
