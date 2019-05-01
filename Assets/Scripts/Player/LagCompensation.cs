using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LagCompensation : MonoBehaviourPunCallbacks, IPunObservable
{
    Vector2 networkPosition;
    float networkRotation;
    float networkAngularVelocity;

    float maxDeltaDistance;
    float maxDeltaAngle;

    public Rigidbody2D ship;

    public PhotonView PV;

    void Start()
    {
        PhotonNetwork.SerializationRate = 20;
        PhotonNetwork.SendRate = 30;

        ship = GetComponent<Rigidbody2D>();

    }

  
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ship.position);
            stream.SendNext(ship.rotation);
            stream.SendNext(ship.velocity);
            stream.SendNext(ship.angularVelocity);
        }
        else
        {
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)); 

            networkPosition = (Vector2)stream.ReceiveNext();
            networkRotation = (float)stream.ReceiveNext();


            if (Vector3.Distance(ship.position, networkPosition) > 10.0f)
            {
                ship.position = networkPosition;
            }


            ship.velocity = (Vector2)stream.ReceiveNext();
            ship.angularVelocity = (float)stream.ReceiveNext();

            networkPosition += ship.velocity * lag;
            networkRotation += ship.angularVelocity * lag;

            maxDeltaDistance = Vector2.Distance(ship.position, networkPosition);
            maxDeltaAngle = Mathf.Abs(ship.rotation - networkRotation);
        }
    }

    #endregion
    void FixedUpdate()
    {
        if (!PV.IsMine)
        {

            ship.position = Vector2.MoveTowards(ship.position, networkPosition, Time.fixedDeltaTime);
            ship.rotation = Mathf.MoveTowards(ship.rotation, networkRotation, Time.fixedDeltaTime*200);
        }
    }
}
