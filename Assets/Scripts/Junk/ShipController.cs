using UnityEngine;
using System.Collections;

#if !UNITY_ANDROID
//using XInputDotNetPure;


//// Put this on a GetComponent<Rigidbody2D>() object and instantly
//// have 2D spaceship controls like OverWhelmed Arena
//// that you can tweak to your heart's content.

//[RequireComponent(typeof(Rigidbody2D))]
//public class ShipController : MonoBehaviour
//{
//    //The ship's rigidbody
//    public Rigidbody2D ship;
//    //The speed multiplyer of the ship
//    public float speed, speedDefault, speedBoost;
//    //The rotation multiplyer of the ship
//    public float rotationSpeed, rotationSpeedDefault, rotationSpeedDrifting;
//    //Are we drifting?
//    public bool isDrifting;
//    //Are we boosting?
//    public bool isBoosting;
//    //What player controller ID is this?
//    public PlayerIndex playerIndex;

//    void OnEnable()
//    {
//        if (playerIndex == PlayerIndex.One)
//        {
//            InputController.OnTriggerHold_Controller_1 += OnTriggerHold;
//            InputController.OnLeftStickHold_Controller_1 += OnLeftStickHold;

//            InputController.OnBButtonDown_Controller_1 += OnBButtonDown;
//            InputController.OnBButtonUp_Controller_1 += OnBButtonUp;
//            InputController.OnXButtonDown_Controller_1 += OnXButtonDown;
//            InputController.OnXButtonUp_Controller_1 += OnXButtonUp;
//        }
//        else
//        {
//            InputController.OnTriggerHold_Controller_2 += OnTriggerHold;
//            InputController.OnLeftStickHold_Controller_2 += OnLeftStickHold;

//            InputController.OnBButtonDown_Controller_2 += OnBButtonDown;
//            InputController.OnBButtonUp_Controller_2 += OnBButtonUp;
//            InputController.OnXButtonDown_Controller_2 += OnXButtonDown;
//            InputController.OnXButtonUp_Controller_2 += OnXButtonUp;
//        }
//    }

//    IEnumerator OnTriggerEnter2D(Collider2D col)
//    {
//        if (col.gameObject != gameObject)
//        {
//            GamePad.SetVibration(playerIndex, 0, 1);
//            yield return new WaitForSeconds(0.1f);
//            GamePad.SetVibration(playerIndex, isBoosting ? 1 : 0, 0);

//        }
//    }

//    void OnTriggerHold(float value)
//    {
//        ship.AddForce(ship.transform.up * value * speed * Time.fixedDeltaTime);
//    }

//    void OnRightStickHold(Vector2 value)
//    {
        
//    }

//    void OnLeftStickHold(Vector2 value)
//    {
//        ship.AddTorque(value.x * rotationSpeed * Time.fixedDeltaTime);
//    }

//    void OnBButtonDown()
//    {
//        isBoosting = true;
//        speed = speedBoost;
//        GamePad.SetVibration(playerIndex, 1, 0);
//    }

//    void OnBButtonUp()
//    {
//        isBoosting = false;
//        speed = speedDefault;
//        GamePad.SetVibration(playerIndex, 0, 0);
//    }

//    void OnXButtonDown()
//    {
//        isDrifting = true;
//        rotationSpeed = rotationSpeedDrifting;
//    }

//    void OnXButtonUp()
//    {
//        isDrifting = false;
//        rotationSpeed = rotationSpeedDefault;
//    }

//}

#endif