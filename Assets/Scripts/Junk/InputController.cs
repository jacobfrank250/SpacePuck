using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InputController : MonoBehaviour
{
    public enum PlayerTypes
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    public PlayerTypes Player;

    [Header("Joystick Dead Zone")]
    [Tooltip("This is the minimum input we must receive before we consider joystick input valid")]
    [Range(0.0f, 0.99f)] public float joystickDead;
    [Header("Left Joystick")]
    public Vector2 leftStick_Controller_1, leftStick_Controller_2;
    [Header("Triggers")]
    public float triggers_Controller_1, triggers_Controller_2;


    public static Action<float> OnTriggerHold_Controller_1;
    public static Action<Vector2> OnLeftStickHold_Controller_1;
    public static Action<float> OnTriggerHold_Controller_2;
    public static Action<Vector2> OnLeftStickHold_Controller_2;

    public static Action OnAButtonDown_Controller_1;
    public static Action OnAButtonUp_Controller_1;
    public static Action OnBButtonDown_Controller_1;
    public static Action OnBButtonHold_Controller_1;
    public static Action OnBButtonUp_Controller_1;
    public static Action OnXButtonDown_Controller_1;
    public static Action OnXButtonUp_Controller_1;
    public static Action OnYButtonPressed_Controller_1;

    public static Action OnAButtonDown_Controller_2;
    public static Action OnAButtonUp_Controller_2;
    public static Action OnBButtonDown_Controller_2;
    public static Action OnBButtonHold_Controller_2;
    public static Action OnBButtonUp_Controller_2;
    public static Action OnXButtonDown_Controller_2;
    public static Action OnXButtonUp_Controller_2;
    public static Action OnYButtonPressed_Controller_2;

    // Update is called once per frame
    void Update()
    {
        GetAxis_Controller_1();
        GetAxis_Controller_2();
        GetButtons_Controller_1();
        GetButtons_Controller_2();
    }

    /// <summary>
    /// Get Axis data of the joysick
    /// </summary>
    void GetAxis_Controller_1()
    {
        //Left Stick
        if (Input.GetAxisRaw("controller 1 X axis") > joystickDead || Input.GetAxisRaw("controller 1 X axis") < -joystickDead)
        {
            leftStick_Controller_1 = new Vector2(Input.GetAxisRaw("controller 1 X axis"), Input.GetAxisRaw("controller 1 Y axis"));
            if (OnLeftStickHold_Controller_1 != null) OnLeftStickHold_Controller_1(leftStick_Controller_1);
        }
        else
        {
            leftStick_Controller_1 = new Vector2(0, leftStick_Controller_1.y);
        }

        if (Input.GetAxisRaw("controller 1 Y axis") > joystickDead || Input.GetAxisRaw("controller 1 Y axis") < -joystickDead)
        {
            leftStick_Controller_1 = new Vector2(Input.GetAxisRaw("controller 1 X axis"), Input.GetAxisRaw("controller 1 Y axis"));
            if (OnLeftStickHold_Controller_1 != null) OnLeftStickHold_Controller_1(leftStick_Controller_1);
        }
        else
        {
            leftStick_Controller_1 = new Vector2(leftStick_Controller_1.x, 0);
        }

        //Triggers
        if (Input.GetAxisRaw("controller 1 3rd axis") > joystickDead || Input.GetAxisRaw("controller 1 3rd axis") < -joystickDead)
        {
            triggers_Controller_1 = Input.GetAxisRaw("controller 1 3rd axis");
            if (OnTriggerHold_Controller_1 != null) OnTriggerHold_Controller_1(triggers_Controller_1);
        }
        else triggers_Controller_1 = 0;
    }

    void GetAxis_Controller_2()
    {
        //Left Stick
        if (Input.GetAxisRaw("controller 2 X axis") > joystickDead || Input.GetAxisRaw("controller 2 X axis") < -joystickDead)
        {
            leftStick_Controller_2 = new Vector2(Input.GetAxisRaw("controller 2 X axis"), Input.GetAxisRaw("controller 2 Y axis"));
            if (OnLeftStickHold_Controller_2 != null) OnLeftStickHold_Controller_2(leftStick_Controller_2);
        }
        else
        {
            leftStick_Controller_2 = new Vector2(0, leftStick_Controller_2.y);
        }

        if (Input.GetAxisRaw("controller 2 Y axis") > joystickDead || Input.GetAxisRaw("controller 2 Y axis") < -joystickDead)
        {
            leftStick_Controller_2 = new Vector2(Input.GetAxisRaw("controller 2 X axis"), Input.GetAxisRaw("controller 2 Y axis"));
            if (OnLeftStickHold_Controller_2 != null) OnLeftStickHold_Controller_2(leftStick_Controller_2);
        }
        else
        {
            leftStick_Controller_2 = new Vector2(leftStick_Controller_2.x, 0);
        }

        //Triggers
        if (Input.GetAxisRaw("controller 2 3rd axis") > joystickDead || Input.GetAxisRaw("controller 2 3rd axis") < -joystickDead)
        {
            triggers_Controller_2 = Input.GetAxisRaw("controller 2 3rd axis");
            if (OnTriggerHold_Controller_2 != null) OnTriggerHold_Controller_2(triggers_Controller_2);
        }
        else triggers_Controller_2 = 0;
    }

    /// <summary>
    /// get the button data of the joystick
    /// </summary>
    void GetButtons_Controller_1()
    {
        //A Button
        if (Input.GetButtonDown("joystick 1 button 0")) if (OnAButtonDown_Controller_1 != null) OnAButtonDown_Controller_1();
        if (Input.GetButtonUp("joystick 1 button 0")) if (OnAButtonUp_Controller_1 != null) OnAButtonUp_Controller_1();
        //B Buton 
        if (Input.GetButtonDown("joystick 1 button 1")) if (OnBButtonDown_Controller_1 != null) OnBButtonDown_Controller_1();
        if (Input.GetButtonUp("joystick 1 button 1")) if (OnBButtonUp_Controller_1 != null) OnBButtonUp_Controller_1();
        //X Button
        if (Input.GetButtonDown("joystick 1 button 2")) if (OnXButtonDown_Controller_1 != null) OnXButtonDown_Controller_1();
        if (Input.GetButtonUp("joystick 1 button 2")) if (OnXButtonUp_Controller_1 != null) OnXButtonUp_Controller_1();
    }

    void GetButtons_Controller_2()
    {
        //A Button
        if (Input.GetButtonDown("joystick 2 button 0")) if (OnAButtonDown_Controller_2 != null) OnAButtonDown_Controller_2();
        if (Input.GetButtonUp("joystick 2 button 0")) if (OnAButtonUp_Controller_2 != null) OnAButtonUp_Controller_2();
        //B Buton 
        if (Input.GetButtonDown("joystick 2 button 1")) if (OnBButtonDown_Controller_2 != null) OnBButtonDown_Controller_2();
        if (Input.GetButtonUp("joystick 2 button 1")) if (OnBButtonUp_Controller_2 != null) OnBButtonUp_Controller_2();
        //X Button
        if (Input.GetButtonDown("joystick 2 button 2")) if (OnXButtonDown_Controller_2 != null) OnXButtonDown_Controller_2();
        if (Input.GetButtonUp("joystick 2 button 2")) if (OnXButtonUp_Controller_2 != null) OnXButtonUp_Controller_2(); 
    }
}
