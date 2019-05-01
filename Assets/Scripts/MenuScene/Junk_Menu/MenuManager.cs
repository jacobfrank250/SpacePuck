using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class MenuManager : MonoBehaviour
{
    public Animator anim;
    public Image SelectedShip;
    public Transform SelectedShipTransform;
   
    bool selectingShip;
    bool doneScaling;
    bool doneRotating;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onShipSelect(Sprite selectedSprite)
    {
        Debug.Log("ship selected");
        if (!selectingShip) StartCoroutine(NewShip(selectedSprite));
        else Debug.Log("selecting ship action pause");
    }

    IEnumerator rotateShip(float duration)
    {
        bool keepRotating = true;
        float startTime = Time.realtimeSinceStartup;
        float timer = 0;
        while(keepRotating)
        {
            if (timer > 1)
            { Debug.Log("timer>1"); }
            if (timer > 5)
            { Debug.Log("timer>5"); }
            //Quaternion newRotation = Quaternion.Euler(0, 0, SelectedShip.transform.rotation.z + 1);
            //SelectedShip.transform.rotation = newRotation;
            SelectedShip.transform.rotation = new Quaternion(0, 0, 100 * timer,1);

            timer += Time.deltaTime;
            if (timer >= duration) keepRotating = false;

            yield return null;
        }
        Debug.Log("done");
        //doneRotating = true;
    }

    IEnumerator Rotate(float duration)
    {
        doneRotating = false;

        //float startRotation = transform.eulerAngles.z;
        float startRotation = SelectedShipTransform.eulerAngles.z;

        //float endRotation = startRotation + 360.0f;
        float endRotation = 360.0f;

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            SelectedShipTransform.eulerAngles = new Vector3(SelectedShipTransform.eulerAngles.x, SelectedShip.transform.eulerAngles.y, zRotation);
            yield return null;
        }
        Debug.Log("DOne Rotating");
        doneRotating = true;

    }

    IEnumerator Scale(float duration, float endSizeScale)
    {
        doneScaling = false;

        //float startRotation = transform.eulerAngles.z;
        float startSizeScale = SelectedShipTransform.localScale.x;

        //Vector3 endSize = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float newSize = Mathf.Lerp(startSizeScale, endSizeScale, t / duration) % 360.0f;

            SelectedShipTransform.localScale = new Vector3(newSize, newSize, newSize);
            yield return null;
        }
        Debug.Log("DOne Scaling");
        doneScaling = true;
    }


  
    IEnumerator NewShip(Sprite newShipSprite)
    {
        selectingShip = true;
        Debug.Log("new ship");
        //Scale(10.0f, 0.1f);
        StartCoroutine(Scale(0.5f, 0.01f));
        StartCoroutine(Rotate(0.5f));
        while(!doneScaling && !doneRotating)
        {
            yield return null;
        }
        Debug.Log("done scaling and rotating");

        SelectedShip.sprite = newShipSprite;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Scale(0.5f, 1.0f));
        StartCoroutine(Rotate(0.5f));
        while (!doneScaling && !doneRotating)
        {
            yield return null;
        }
        selectingShip = false;
    }

}
