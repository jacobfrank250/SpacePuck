using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;


public class GoalManager : MonoBehaviour
{

    public static Action<bool> OnGameStateChanged;

    public bool canMove;
    public int blueScore, orangeScore;
    public int blueScorePrev, orangeScorePrev;

    //public Transform bluePlayer, orangePlayer
    public Transform ball;

    public Vector3 bluePlayerStartPos, orangePlayerStartPos, ballStartPos;
    public Quaternion bluePlayerStartRot, orangePlayerStartRot, ballStartRot;

    public Text blueScoreText, orangeScoreText;

    public Text countDownText;

    PhotonView PV;


    void OnEnable()
    {
        Goal.OnGoalScored += OnGoalScored;

    }

    void Start()
    {
        PV = this.GetComponent<PhotonView>();

        StartCoroutine(RetartGame());
    }


    public IEnumerator RetartGame()
    {
        if (OnGameStateChanged != null) OnGameStateChanged(false);
        canMove = false;
        yield return new WaitForSeconds(3);
        if (OnGameStateChanged != null) OnGameStateChanged(true);
        canMove = true;
    }

    public IEnumerator CountDown()
    {
        int timer = 3;
        if (OnGameStateChanged != null) OnGameStateChanged(false);
        while (timer>0)
        {
            Debug.Log(timer);
            countDownText.text = "" + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }
        countDownText.text = "GO";
        if (OnGameStateChanged != null) OnGameStateChanged(true);
        yield return new WaitForSeconds(1);
        countDownText.text = "";
    }

    [PunRPC]
    void RPC_CountDown()
    {
        StartCoroutine(CountDown());
    }

    public IEnumerator animateCountDownText(Text myText,float duration)
    {
        float startSizeScale = 1/2;
        float endSizeScale = 1;

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float newSize = Mathf.Lerp(startSizeScale, endSizeScale, t / duration);

            yield return null;
        }
    }
    void OnGoalScored(Enums.Teams Team)
    {

        PV.RPC("updateScore", RpcTarget.AllBuffered, Team);

        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.position = ballStartPos;
        ball.rotation = ballStartRot;

        PV.RPC("respawnBall", RpcTarget.AllBuffered);

        PV.RPC("RPC_CountDown", RpcTarget.All);

    }



    [PunRPC]
    void respawnBall()
    {
        ball.position = ballStartPos;
        ball.rotation = ballStartRot;
    }



    [PunRPC]
    public void updateScore(Enums.Teams Team)
    {
        if (Team == Enums.Teams.Blue) blueScore++;
        else orangeScore++;

        blueScoreText.text = blueScore.ToString();
        orangeScoreText.text = orangeScore.ToString();

    }


}
