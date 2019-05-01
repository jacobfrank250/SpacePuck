using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsController : MonoBehaviour
{
    static float offset = 5.0f;
    float leftBound = -20.0f - offset;
    float rightBound = 20f + offset;
    float topBound = 31.0f + offset;
    float bottomBound = -31.0f -offset;

    public Rigidbody2D ship;
                         
    // Start is called before the first frame update
    void Start()
    {

        ship = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        checkInBounds(ship);

    }

    void checkInBounds(Rigidbody2D RB)
    {
        if(RB.position.x > rightBound)
        {
            Debug.Log("player is out of bounds to the right");
            if (RB.position.y > topBound)
            {
                Debug.Log("AND out of bounds on the bottom");

                RB.position = new Vector2(rightBound-offset,topBound-offset);
            }
            else if (RB.position.y < bottomBound)
            {
                Debug.Log("AND out of bounds on the top");
                RB.position = new Vector2(rightBound - offset, bottomBound + offset);

            }
            else
            {
                Debug.Log("and no where else");
                RB.position = new Vector2(rightBound - offset, RB.position.y);
            }
        }
        else if (RB.position.x< leftBound)
        {
            Debug.Log("player is out of bounds to the left");
            if (RB.position.y > topBound)
            {
                Debug.Log("AND out of bounds on the bottom");
                RB.position = new Vector2(leftBound + offset, topBound - offset);
            }
            else if (RB.position.y < bottomBound)
            {
                Debug.Log("AND out of bounds on the top");
                RB.position = new Vector2(leftBound + offset, bottomBound + offset);

            }
            else
            {
                Debug.Log("and no where else");
                RB.position = new Vector2(leftBound + offset, RB.position.y);

            }
        }
        else if (RB.position.y>topBound)
        {
            Debug.Log("player is out of bounds on the bottom");
            RB.position = new Vector2(RB.position.x, topBound-offset);

        }
        else if (RB.position.y < bottomBound)
        {
            RB.position = new Vector2(RB.position.x, bottomBound + offset);

        }
        else
        {
            //Debug.Log("player is in bounds");
        }

    }
}
