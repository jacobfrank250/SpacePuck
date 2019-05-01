using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour
{

    public Rigidbody2D ship;
    public Image acceleratorGague;
    public float hAx;
    public float handlingSpeed;
    public float movementSpeed, acceleration, maxMovementSpeed;
    public bool isAccelerating;
    bool canMove;



    float mouseDistance = 0;
    private Vector3 lastPosition;
    private bool trackMouse = false;

    public float turnAmount;

    public int leftTouchID;
    public int rightTouchID;

    float fingerDistance = 0;
    //


    void OnEnable()
    {
        //GoalManager.OnGameStateChanged += GoalManager_OnGameStateChanged;
    }

    void FixedUpdate()
    {
       //Debug.Log("returning?");

        //if (!canMove) return; 

        //ship.AddTorque(-hAx * handlingSpeed);
        //ship.AddTorque(-turnAmount * handlingSpeed);
        //Debug.Log("move rotation?");
        ship.MoveRotation(Mathf.LerpAngle(ship.rotation, -turnAmount * 360, handlingSpeed * Time.deltaTime));
        //ship.MoveRotation(-turnAmount * 360);

        ship.AddForce(ship.transform.up * movementSpeed);
    }

    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        //cam.SetActive(true);

        GetInputs();

        //SetAcceleratorGague();



       
    }

   

    void GetInputs()
    {
        checkTouches();
        //GET TURN AMOUNT
        getTurnAmount();
        //GET MOVEMENT SPEED
        getMovementSpeed();

    }
    void checkTouches()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Debug.Log("touch Position: " + touch.position);
                Debug.Log("screen.width/2: " + Screen.width / 2);
                if (touch.position.x < Screen.width / 2) //LEFT SIDE OF SCREEN
                {
                    Debug.Log("left side touched");
                    leftTouchID = touch.fingerId;
                    Debug.Log("left touch id: " + leftTouchID);

                }
                else
                {
                   
                    Debug.Log("right side touched");
                    if (touch.fingerId != leftTouchID) //check the touch registered on the right side is not a touch that was started on the left side
                    {
                        rightTouchID = touch.fingerId;
                        Debug.Log("right touch id: " + rightTouchID);

                    }
                }
            }
        }
    }

    void getTurnAmount()
    {

        //#if !UNITY_EDITOR
#if !UNITY_STANDALONE
        //FIND TOUCH WITH CORRECT FINGER ID
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].fingerId == leftTouchID)
            {
                //We have found the touch with the correct finger id!
                Touch myTouch = Input.touches[i];

                //CHECK DISTANCE MOVED 
                if (myTouch.phase == TouchPhase.Moved)
                {
                    int direction;
                    if (myTouch.deltaPosition.x > 0) direction = 1;
                    else direction = -1;

                    fingerDistance += myTouch.deltaPosition.magnitude * direction;

                    turnAmount = fingerDistance / Screen.width;

                }

            }
        }

#else
        float mouseDistToReturn;

        if (Input.GetMouseButtonDown(0))
        {
            trackMouse = true;
            lastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            Debug.Log("Mouse moved " + mouseDistance + " while button was down.");
            //turnAmount = mouseDistance/(Screen.width); //ratio to screen wdith
            //mouseDistance = 0;
            //return mouseDistance;

        }
        if (trackMouse)
        {
            var newPosition = Input.mousePosition;
            // If you just want the x-axis:
            //mouseDistance += Mathf.Abs(newPosition.x - lastPosition.x);
            //mouseDistance += newPosition.x - lastPosition.x;
            int direction;
            if((newPosition.x-lastPosition.x)>0){
                direction = 1;
            } 
            else{
                direction = -1;
            }

            // If you just want the y-axis,change newPosition.x to newPosition.y and lastPosition.x to lastPosition.y
            // If you want the entire distance moved (not just the X-axis, use:
            mouseDistance += (newPosition - lastPosition).magnitude * direction;


            turnAmount = mouseDistance / Screen.width;

            lastPosition = newPosition;
        }

#endif
    }

  

    void getMovementSpeed()
    {
        //CHECK IF USER IS PRESSING ACCELERATION BUTTON
        //#if !UNITY_EDITOR
#if !UNITY_STANDALONE

        bool found = false;
        Debug.Log("number of touches: " + Input.touches.Length);
        for (int i = 0; i < Input.touches.Length; i++)
        {
            
            if (Input.touches[i].fingerId == rightTouchID) 
            {
                Debug.Log("right touch id = " + rightTouchID);
                Debug.Log("left touch id = " + leftTouchID);
                Debug.Log("touch number: " + i);
                isAccelerating = true;
                found = true;
            }
        }
        if (!found) isAccelerating = false;

#else //Testing on computer so using space bar for accelorate


        if (Input.GetKey("space")) //Returns true while the user HOLDS down the key 
        {
            isAccelerating = true;
        }
        else
        {
            isAccelerating = false;
        }
#endif

        if (isAccelerating && movementSpeed <= maxMovementSpeed) 
        {
            movementSpeed += acceleration * Time.deltaTime;
        }
        else if (movementSpeed > 0) 
        {
            movementSpeed -= acceleration * 2 * Time.deltaTime;
        }
        if (movementSpeed <= 0) {
            movementSpeed = 0;
        }

    }


    //void SetAcceleratorGague()
    //{
    //    acceleratorGague.fillAmount = Mathf.Lerp(acceleratorGague.fillAmount, movementSpeed / maxMovementSpeed, Time.deltaTime * 2);
    //    acceleratorGague.color = Color.Lerp(acceleratorGague.color, GetAcceleratorColor(acceleratorGague.fillAmount), Time.deltaTime * 2);
    //}

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
        //canMove = state; 
        canMove = true; 
        if (!state)
        {
            movementSpeed = 0;
            acceleratorGague.fillAmount = 0;
            isAccelerating = false;
        }

    }

}
