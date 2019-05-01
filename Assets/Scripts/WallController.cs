using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallController : MonoBehaviour
{
    public enum WallType {left, right, top, bottom};

    public WallType wallType;

    float fieldWidth = 40.0f;
    float fieldLength = 62.0f;


    Vector2 playerEnterPosition;
    Vector2 playerExitPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            //Debug.Log("On trigger enter ball");
            ballColision(other.attachedRigidbody, wallType);

        }
        else if (other.gameObject.tag == "Player")
        {
            playerEnterPosition = other.attachedRigidbody.position;
            //Debug.Log("Player entered trigger at time " + Time.realtimeSinceStartup + " and at y position " + other.attachedRigidbody.position.y, other.gameObject);
            //Debug.DrawLine(other.attachedRigidbody.position, new Vector2(other.attachedRigidbody.position.x + 10, other.attachedRigidbody.position.y),Color.blue,20.0f,false);
           

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerExitPosition = other.attachedRigidbody.position;
            //Debug.Log("on trigger exit player at time " + Time.realtimeSinceStartup + " and at y position " + other.attachedRigidbody.position.y, other.gameObject);
            //Debug.DrawLine(other.attachedRigidbody.position, new Vector2(other.attachedRigidbody.position.x + 10, other.attachedRigidbody.position.y), Color.red,20.0f, false );

            TeleportPlayer(wallType, other.attachedRigidbody);
        }
    }


    public void TeleportPlayer(WallType wall, Rigidbody2D player)
    {
        if(wall == WallType.left)
        {
            //this function is called on triggerExit. We want to see that the player has exited on the outer side of the wall. 
            //For the left wall that is when their x position is less than (to the left of) the walls position
            //(if it was greater then it would mean they entered the wall trigger and then turned back into the playing field)
            if (playerExitPosition.x < playerEnterPosition.x)
            {
                player.position = new Vector2 (player.position.x+fieldWidth,player.position.y);
            }
        }
        else if(wall == WallType.right)
        {
            //if (player.position.x > this.transform.position.x)
            if (playerExitPosition.x > playerEnterPosition.x)
            {
                player.position = new Vector2(player.position.x - fieldWidth, player.position.y);
            }
        }
        else if(wall == WallType.top)
        {
            if(playerExitPosition.y>playerEnterPosition.y)
            {
                //player.rotation = 180-player.rotation;
                player.rotation = player.rotation + 180;
                Debug.Log("player rotation: " + player.rotation);
            }
            else
            {
                Debug.Log("TOP BARRIER: players enter position less than the exit position",this.gameObject);

            }
        }
        else if(wall == WallType.bottom)
        {
            if (playerExitPosition.y < playerEnterPosition.y)
            {
                player.rotation = player.rotation + 180;
              


            }
            else
            {
                Debug.Log("BOTTOM BARRIER: players enter position greater than the exit position", this.gameObject);

            }
        }

        else 
        {
            Debug.Log("error! no wall type");
        }
    }

   

    private void ballColision(Rigidbody2D ball, WallType wall)
    {
        Vector2 inDirection = ball.velocity;
        Vector2 inNormal = getWallNormal(wall);
        Vector2 newVelocity = Vector2.Reflect(inDirection, inNormal);
        ball.velocity = newVelocity;
    }

    private Vector2 getWallNormal(WallType wall)
    {
        if (wall == WallType.left)
        {
            return Vector2.right;
        }
        else if (wall == WallType.right)
        {
            return Vector2.left;
        }
        else if (wall == WallType.top)
        {
            return Vector2.down;
        }
        else if (wall == WallType.bottom)
        {
            return Vector2.up;
        }

        else
        {
            Debug.Log("error! no wall type...wall type normal zero");
            return Vector2.zero;
        }
    }

   
}
