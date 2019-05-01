using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public PhotonView PV;
    Rigidbody2D RB;

    Vector2 ballVelocity;


    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = new Vector2(0.01f, 0.03f);
    }

    //This method calculates the balls final velocity using an equation for two-dimensional elastic collisions with two moving objects
    void calculateBallVelocity(Rigidbody2D player)
    {
        Vector2 v1 = RB.velocity;
        Vector2 v2 = player.velocity;
        float m1 = RB.mass;
        float m2 = player.mass;
        Vector2 x1 = RB.position;
        Vector2 x2 = player.position;

        Vector2 lhs = v1 - v2;
        Vector2 rhs = x1 - x2;
        float dot = Vector2.Dot(lhs, rhs);
        float squaredValue = Vector2.Distance(x1, x2);

        Vector2 v1f = v1 - (2.0f * m2 / (m1 + m2)) * (dot / squaredValue) * (x1 - x2);

        PV.RPC("RPC_VelocityAfterColision", RpcTarget.All, v1f);

    }

    [PunRPC]
    void RPC_VelocityAfterColision(Vector2 velocityAfterCollision)
    {

        RB.velocity = new Vector2(velocityAfterCollision.x / 1.5f, velocityAfterCollision.y / 1.5f);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BallSoundManager>().playPlayerHitBall(RB, other.attachedRigidbody);

            if (other.gameObject.GetComponentInParent<PhotonView>().IsMine)
            {
                calculateBallVelocity(other.attachedRigidbody);
            }

        }
        else if (other.tag == "Wall")
        {
            GetComponent<BallSoundManager>().playBallHitWall(RB);

        }
        else
        {
            GetComponent<BallSoundManager>().playBallHitWall(RB);

        }
    }


    #region Old Implementation: non-trigger, angle-dependent elastic collision calculation

    Vector2 getVelocityAfterCollision(float v1, float v2, float m1, float m2, float theta1, float theta2, float collisionAngle)
    {
        float leftSide = LeftSideOfEquation(v1, v2, m1, m2, theta1, theta2, collisionAngle);

        float v1fx = leftSide * (Mathf.Cos(collisionAngle) + v1 * Mathf.Sin(theta1 - collisionAngle) * Mathf.Sin(collisionAngle));
        float v1fy = leftSide * (Mathf.Sin(collisionAngle) + v1 * Mathf.Sin(theta1 - collisionAngle) * Mathf.Cos(collisionAngle));

        return new Vector2(v1fx, v1fy);
    }

    float LeftSideOfEquation(float v1, float v2, float m1, float m2, float theta1, float theta2, float collisionAngle) //same long calculation is used to solve v1f and v2f 
    {
        return (v1 * Mathf.Cos(theta1 - collisionAngle) * (m1 - m2) + 2 * m2 * v2 * Mathf.Cos(theta2 - collisionAngle)) / (m1 + m2);
    }



    float getMovementAngle(float vx, float vy, float v, Rigidbody2D obj2)
    {

        if (v == 0.0f) return Mathf.Atan(obj2.velocity.y / obj2.velocity.x);
        return Mathf.Acos(vx / v);
    }



    float getContactAngle(Collision2D col)
    {
        Vector2 orthogonalVector = col.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
        float collisionAngle = Vector2.Angle(orthogonalVector, RB.velocity);
        return collisionAngle;
    } //void calculatePlayerVelocity(Collision2D colPlayer)
    //{
    //    Rigidbody2D player = colPlayer.rigidbody;
    //    Vector2 v1 = RB.velocity;
    //    Vector2 v2 = player.velocity;
    //    float m1 = RB.mass;
    //    float m2 = player.mass;
    //    Vector2 x1 = RB.position;
    //    Vector2 x2 = player.position;

    //    Vector2 lhs = v2 - v1;
    //    Vector2 rhs = x2 - x1;
    //    float dot = Vector2.Dot(lhs, rhs);
    //    float squaredValue = Vector2.Distance(x2, x1);

    //    Vector2 v2f = v2 - (2.0f * m1 / (m1 + m2)) * (dot / squaredValue) * (x2 - x1);

    //    int playerViewID = colPlayer.gameObject.GetComponent<PhotonView>().ViewID;
    //    PV.RPC("RPC_VelocityAfterColision2", RpcTarget.All, v2f,playerViewID);


    //    //Vector2 v1f = v1 - (2*m2/(m1+m2))*((Vector2.Dot())/())

    //}

    //[PunRPC]
    //void RPC_VelocityAfterColision2(Vector2 velocityAfterCollision, int viewID)
    //{
    //    PhotonView.Find(viewID).gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityAfterCollision.x / 2, velocityAfterCollision.y / 2);
    //}

    void playerCollision(Collision2D collision)
    {
        //FIND NEW VELOCITY AFTER COLLISION
        //variables to pass into getVelocityAfterCollision method.
        float ballSpeed; //v1
        float playerSpeed; //v2
        float ballMass; //m1
        float playerMass; //m2
        float ballMovementAngle; //theta1
        float playerMovementAngle; //theta2;
        float contactAngle; //collision angle

        ballSpeed = RB.velocity.magnitude;
        //ballSpeed = velocityBeforePhysicsUpdate.magnitude;

        playerSpeed = collision.rigidbody.velocity.magnitude;

        ballMass = RB.mass;
        playerMass = collision.rigidbody.mass;

        ballMovementAngle = getMovementAngle(RB.velocity.x, RB.velocity.y, ballSpeed, collision.rigidbody);
        playerMovementAngle = getMovementAngle(collision.rigidbody.velocity.x, collision.rigidbody.velocity.y, playerSpeed, RB);


        contactAngle = getContactAngle(collision);

        Vector2 velocityAfterColision = getVelocityAfterCollision(ballSpeed, playerSpeed, ballMass, playerMass, ballMovementAngle, playerMovementAngle, contactAngle);


        PV.RPC("RPC_VelocityAfterColision", RpcTarget.All, velocityAfterColision);
    }

    //[PunRPC]
    //void RPC_SendVelocityAfterCollision(Vector2 velocity)
    //{
    //    Debug.Log("RPC_SendVelocityAfterCollision");
    //    RB.velocity = velocity;
    //}
    #endregion

}
