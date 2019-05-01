using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cam;
    public Transform target;
    public float yLimit;

    
    void OnEnable()
    {
        Goal.OnBallEnteredGoal += OnBallEnteredGoal;
    }

    void LateUpdate()
    {
        if (target.position.y >= yLimit || target.position.y <= -yLimit)
        {
            cam.position = Vector3.Lerp(cam.position, new Vector3(cam.position.x, target.position.y >= yLimit ? yLimit : -yLimit, cam.position.z), Time.deltaTime * 5);
            return;
        }
        else
        {
            cam.position = Vector3.Lerp(cam.position, new Vector3(cam.position.x, target.position.y, cam.position.z), Time.deltaTime * 5);
        }
    }

    void OnBallEnteredGoal(Transform goalEntered)
    {
        StartCoroutine(DoOnBallEnteredGoal(goalEntered));
    }

    IEnumerator DoOnBallEnteredGoal(Transform goal)
    {

        //do stuff
        //cam.position = goal.position;

        //move camera to goal position and spawn electric explosion particle system
        yield break;
    }
}
