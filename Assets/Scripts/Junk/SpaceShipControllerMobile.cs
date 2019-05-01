using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;

public class SpaceShipControllerMobile : MonoBehaviour
{

    public Rigidbody2D ship;
    public Image acceleratorGague;
    public float hAx;
    public float handlingSpeed;
    public float movementSpeed, acceleration, maxMovementSpeed;
    public bool isAccelerating;
    bool canMove;

    void OnEnable()
    {
        GoalManager.OnGameStateChanged += GoalManager_OnGameStateChanged;
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        ship.AddTorque(-hAx * handlingSpeed);
        ship.AddForce(ship.transform.up * movementSpeed);
    }

    void Update()
    {
        GetInputs();
        SetAcceleratorGague();
    }

    void GetInputs()
    {
        hAx = CnControls.CnInputManager.GetAxis("Horizontal"); 
        if (isAccelerating && movementSpeed <= maxMovementSpeed) movementSpeed += acceleration * Time.deltaTime;
        else if (movementSpeed > 0) movementSpeed -= acceleration * 2 * Time.deltaTime;
        if (movementSpeed <= 0) movementSpeed = 0;
    }

    void SetAcceleratorGague()
    {
        acceleratorGague.fillAmount = Mathf.Lerp(acceleratorGague.fillAmount, movementSpeed / maxMovementSpeed, Time.deltaTime * 2);
        acceleratorGague.color = Color.Lerp(acceleratorGague.color, GetAcceleratorColor(acceleratorGague.fillAmount), Time.deltaTime * 2);
    }

    Color GetAcceleratorColor(float gagueValue)
    {
        if (gagueValue > 0 && gagueValue < 0.25f) return Color.green;
        else if (gagueValue >= 0.25f && gagueValue < 0.5f) return Color.yellow;
        else if (gagueValue >= 0.5f && gagueValue < 0.75f) return new Color(1, 1, 0);
        else return Color.red;
    }


    public void OnPressAcceleration()
    {
        isAccelerating = true;
    }

    public void OnReleaseAcceleration()
    {
        isAccelerating = false;  
    }

    void GoalManager_OnGameStateChanged(bool state)
    {
        canMove = state;
        if (!state)
        {
            movementSpeed = 0;
            acceleratorGague.fillAmount = 0;
            isAccelerating = false;
        }

    }


}
