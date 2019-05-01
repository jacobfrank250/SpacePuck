using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudManager : MonoBehaviour
{

    //The layers to parallax
    public SpriteRenderer[] layers;
    //The speed difference multiplyer between all of the layers
    public float divisor = 4;


    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].sharedMaterial.mainTextureOffset += Vector2.right * (i) / divisor * Time.deltaTime;
        }
    }
}
