using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimation : MonoBehaviour
{
        
    public RectTransform[] ObjectsToRotate;
    public float rotationSpeed;

    void Update()
    {
        for (int i = 0; i < ObjectsToRotate.Length; i++) ObjectsToRotate[i].Rotate(Vector3.forward * Time.deltaTime * rotationSpeed * (i + 1));
    }

}
