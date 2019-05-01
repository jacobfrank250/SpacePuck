using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarCombat : MonoBehaviour
{
    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {

            PV.RPC("RPC_Shooting", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_Shooting()
    {
        //a raycast is essentially an invisible line that has a starting point
        //you can specify what that starting point is 
        //you can also specify what direction you want this ray to travel in
        //if that ray intersects with another object that has a collider then it will return that object
        RaycastHit hit;

        //if statement casts the ray and sets the hit
        //raycast(origin of ray, The direction in which you want the ray to travel, how our function will return the object that our ray intersects with (that object will be saved within our hit variable),
        //max distance,

        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000)) //returns true if our ray hits another object 
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("did hit");

            if (hit.transform.tag == "Avatar")
            {
                hit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth -= avatarSetup.playerDamage;
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("did not hit");
        }
    }
}
