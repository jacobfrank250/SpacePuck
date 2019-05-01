using UnityEngine;
using UnityEngine.UI;

public class ParallaxingManager : MonoBehaviour
{
    //The layers to parallax
    public Image[] layers;
    //The speed difference multiplyer between all of the layers
    public float divisor = 4;


    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].material.mainTextureOffset += Vector2.right * (i) / divisor * Time.deltaTime;
        }
    }
}
