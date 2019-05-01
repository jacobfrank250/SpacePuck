using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisplayShip : MonoBehaviour
{
    public Image backgroundCircle;
    public Image[] shipImages;
    public Shadow shipShadow;
    public bool isUnlocked;

    public Image ring;
    bool isMovingForward;
    float targetFill;
    public float ringRotationSpeed;
    public bool showRingAnimation;
    public bool showingRingAnimation;
    public int shipID;
    [Range(0.0f, 1.0f)] public float speed, weight, handling,acceleration;

    private void Start()
    {
        speed = PlayerInfo.PI.ships[shipID].Speed / 1000;
        weight = PlayerInfo.PI.ships[shipID].Mass / 10;
        handling = PlayerInfo.PI.ships[shipID].Handling / 100;
        acceleration = PlayerInfo.PI.ships[shipID].Acceleration / 1000;
    }

    void Update()
    {
        if (!showRingAnimation) return;
        ring.transform.Rotate(Vector3.forward * Time.deltaTime * ringRotationSpeed);
    }


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


    public void SetShipActive(bool state)
    {
        ring.DOPlay();

        if (state)
        {
            FindObjectOfType<StatsWindowHandler>().SetStatBars(new float[] { speed, acceleration, handling , weight});

            RingAnimation();
            showRingAnimation = true;
            transform.DOScale(Vector3.one * 1.2f, 0.25f);
            for (int i = 0; i < shipImages.Length; i++)
            {
                backgroundCircle.DOFade(0.2f, 0.05f).OnComplete(() =>
                    {
                        backgroundCircle.DOFade(0.12f, 0.25f);
                    });
                Color toColor = isUnlocked ? Color.white : new Color(0, 0, 0, 0.25f);
                Vector2 toEffectDistance = isUnlocked ? Vector2.up * -7 : Vector2.zero;
                shipImages[i].DOColor(toColor, 0.25f);
                DOTween.To(() => shipShadow.effectDistance, x => shipShadow.effectDistance = x, toEffectDistance, 0.25f);
            }

        }
        else
        {
            showRingAnimation = false;
            transform.DOScale(Vector3.one * 0.8f, 0.25f);
            for (int i = 0; i < shipImages.Length; i++)
            {
                backgroundCircle.DOFade(0.08f, 0.25f);
                Color toColor = isUnlocked ? new Color(1, 1, 1, 0.5f) : new Color(0, 0, 0, 0.25f);
                shipImages[i].DOColor(toColor, 0.25f);
                for (int j = 0; j < shipImages.Length; j++) shipImages[j].rectTransform.DOScale(Vector3.one, 0.1f);
                DOTween.To(() => shipShadow.effectDistance, x => shipShadow.effectDistance = x, Vector2.zero, 0.25f);
            }
        }

    }

}
