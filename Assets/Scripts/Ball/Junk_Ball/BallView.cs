using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class BallView : MonoBehaviourPunCallbacks, IPunObservable
{
    Vector2 networkPosition;
    Vector2 networkVelocity;

    float maxDeltaDistance;

    public Rigidbody2D RB;

    public PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("info: " + info.ToString());


        if (stream.IsWriting)
        {
            stream.SendNext(RB.position);
            stream.SendNext(RB.velocity);
        }
        else
        {
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition = (Vector2)stream.ReceiveNext();

            RB.velocity = (Vector2)stream.ReceiveNext();

            networkPosition += RB.velocity * lag;

        }

    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            //RB.position = Vector2.MoveTowards(ship.position,networkPosition,maxDeltaDistance*(1/PhotonNetwork.SerializationRate));
            RB.position = Vector2.MoveTowards(RB.position, networkPosition, Time.fixedDeltaTime);
        }
    }

}