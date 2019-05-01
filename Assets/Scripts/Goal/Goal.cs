using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

//using DG.Tweening;

public class Goal : MonoBehaviour
{
    public static Action<Transform> OnBallEnteredGoal;
    public static Action<Enums.Teams> OnGoalScored;

    public Enums.Teams Team;

    public GameObject goalExplosion;

    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ball")
        {
            if (OnBallEnteredGoal != null) OnBallEnteredGoal(this.transform);


            Vector3 pos = col.transform.position;

            PV.RPC("RPC_PositionBallMaster", RpcTarget.MasterClient, pos.x, pos.y, col.GetComponent<PhotonView>().ViewID);

            PV.RPC("HideBall", RpcTarget.AllBuffered, false, col.GetComponent<PhotonView>().ViewID);


            PV.RPC("createExplosion", RpcTarget.AllBuffered, pos);
            GetComponent<goalSoundManager>().playBallExplosion();
             
            yield return new WaitForSeconds(1.0f);

            GetComponent<goalSoundManager>().playShowBoat();

            yield return new WaitForSeconds(1.0f);


            if (PhotonNetwork.IsMasterClient)
            {
                if (OnGoalScored != null) OnGoalScored(Team);
            }

            PV.RPC("HideBall", RpcTarget.AllBuffered, true, col.GetComponent<PhotonView>().ViewID);
        }
        else
        {
            yield return null;
        }
    }

    [PunRPC]
    void createExplosion(Vector3 pos)
    {
        GameObject clone = (GameObject)Instantiate(goalExplosion, pos, this.transform.rotation);
        Destroy(clone,2.0f);
    }


    [PunRPC]
    void HideBall(bool doIt, int ball)
    {
        GameObject Ball = PhotonView.Find(ball).gameObject;
        Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Ball.GetComponent<SpriteRenderer>().enabled = doIt;
    }

    [PunRPC]
    void  RPC_PositionBallMaster(float ballPosX,float ballposy, int ballID)
    {
        GameObject Ball = PhotonView.Find(ballID).gameObject;
        Ball.GetComponent<Rigidbody2D>().position = new Vector2(ballPosX, ballposy);

    }

}
