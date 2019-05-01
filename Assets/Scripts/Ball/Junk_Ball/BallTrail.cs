using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if !UNITY_ANDROID
//using XInputDotNetPure; 
#endif


public class BallTrail : MonoBehaviour
{
    public Sprite particle;
    public Vector3 offset;
    SpriteRenderer sR;
    List<SpriteRenderer> trailParticles = new List<SpriteRenderer>();

    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        StartCoroutine(SpawnTrail());
    }

    IEnumerator SpawnTrail()
    {
        yield return new WaitForSeconds(0.05f);
        GameObject go = null;
        if (trailParticles.Count > 0) go = trailParticles[trailParticles.Count - 1].color.a <= 0 ? trailParticles[trailParticles.Count - 1].gameObject : new GameObject();
        else go = new GameObject();

        SpriteRenderer newSR = null;
        if (go.GetComponent<SpriteRenderer>() != null) newSR = go.GetComponent<SpriteRenderer>();
        else newSR = go.AddComponent<SpriteRenderer>();
        newSR.sprite = particle;
        go.transform.localScale = sR.transform.localScale;
        go.transform.position = sR.transform.position + offset;
        newSR.color = sR.color - new Color(0, 0, 0, 0.5f);
        newSR.transform.DOScale(0, 1);
        newSR.DOFade(0, 1).OnComplete(() =>
            {
                trailParticles.Add(newSR);
            });
        StartCoroutine(SpawnTrail());
    }


}
