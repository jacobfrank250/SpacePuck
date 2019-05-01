using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ring : MonoBehaviour
{
    public Image ring; 
    float targetFill;
    public float ringRotationSpeed;
    public bool showRingAnimation;
    public bool showingRingAnimation;

    void RingAnimation()
    {
        if (showingRingAnimation) return;
        showingRingAnimation = true;
        ring.DOFade(0.3f, 1);
        ring.DOFillAmount(targetFill, Random.Range(1, 4f)).OnComplete(() =>
        {
            targetFill = targetFill == 1 ? 0 : 1;
            ring.fillClockwise = !ring.fillClockwise;
            ringRotationSpeed = Random.Range(0f, 300f);
            showingRingAnimation = false;
            if (showRingAnimation) RingAnimation();
            else ring.DOFade(0, 1);

        });
    }

    public void SelectShip()
    {
        showRingAnimation = false;
        ring.DOPause();
        ring.DOFillAmount(1, 1);
        ring.DOFade(1, 0.25f);
    }


}
