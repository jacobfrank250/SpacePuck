using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatsWindowHandler : MonoBehaviour
{
    [System.Serializable]
    public class StatBar
    {
        public Image bar;
        public Text value;
    }

    public StatBar[] statBars;

    public void SetStatBars(float[] fillAmounts)
    {
        for (int i = 0; i < statBars.Length; i++)
        {
            statBars[i].bar.DOFillAmount(fillAmounts[i], 1);
            statBars[i].value.text = Mathf.RoundToInt((fillAmounts[i] * 100)).ToString();

        }
    }

}
