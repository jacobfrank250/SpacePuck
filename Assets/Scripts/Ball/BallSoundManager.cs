using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSoundManager : MonoBehaviour
{
    //public static AudioManager AM;
    public  AudioClip playerHitBall;
    public AudioClip BallHitWall;

    //AudioClip ballHitWall;
    //AudioClip goalScoredTaunt;
    //AudioClip ballExplosion;
    //AudioClip shipAcceleration;
    //AudioClip teleport;

    public AudioSource source;
    private  float lowPitchRange = 0.75f;
    private  float highPitchRange = 1.5f;
    private  float velToVol = 0.15f;
    private float velocityCLipSplit = 10f;
    // Start is called before the first frame update
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    /*
    In each function: 
    -we choose a random pitch for each collision
    -the volume we play the sound at is a function of the relative speed of the collision.
    */
    public void playPlayerHitBall(Rigidbody2D ballRB, Rigidbody2D playerRB)
    {
        source.pitch = Random.Range(lowPitchRange, highPitchRange); 
        float hitVol = (ballRB.velocity - playerRB.velocity).magnitude * velToVol; 
        source.PlayOneShot(playerHitBall, hitVol);
    }
    public void playBallHitWall(Rigidbody2D ballRB)
    {
        source.pitch = Random.Range(lowPitchRange, highPitchRange);
        float hitVol = (ballRB.velocity).magnitude * velToVol;
        source.PlayOneShot(BallHitWall, hitVol);
    }
}
