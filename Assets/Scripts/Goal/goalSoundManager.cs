using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalSoundManager : MonoBehaviour
{
    public AudioClip ballExplosion;
    public AudioClip showBoat;

    public AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

    }

    public void playBallExplosion()
    {
        source.PlayOneShot(ballExplosion);

    }

    public void playShowBoat()
    {
        source.PlayOneShot(showBoat);

    }
}
