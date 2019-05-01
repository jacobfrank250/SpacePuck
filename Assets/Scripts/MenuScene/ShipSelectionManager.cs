using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShipSelectionManager : MonoBehaviour
{
    //The scrollRect
    public ScrollRect scroll;
    //The content to scroll
    public RectTransform gridLayout;
    //The title tile that slides in from the left
    public RectTransform titleTile;
    //The ships within the gridLayout
    public DisplayShip[] ships;
    //The ship we're currently focused on
    public int focusedShip;
    //The ship we've selected for use
    public int selectedShip;
    //Should we be snapping horizontally?
    public bool LerpH;
    //Are we manual snapping?
    public bool isManualSnapping;
    //The target x position
    public float targetH;
    //The speed we snap to our current ship at
    public float snapSpeed;
    //The time after which we will consider the user 'not interacting'
    public float timeBeforeSnap;
    //The distance between each array element
    float stepSize;
    //The points to snap to based on the number of array elements we have
    float[] targetPoints;
    //The time since our last interaction with the scroll rect
    float lastInteraction;

    public Image SelectedShip;
    public Transform SelectedShipTransform;

    bool selectingShip;
    bool doneScaling;
    bool doneRotating;

    private void Awake()
    {
        //Screen.orientation = ScreenOrientation.Landscape;

    }

    // Use this for initialization
    IEnumerator Start()
    {
        //Find the step size by adding the padding between the ships plus the actual width of each cell
        stepSize = gridLayout.GetComponent<GridLayoutGroup>().spacing.x + gridLayout.GetComponent<GridLayoutGroup>().cellSize.x;
        //stepSize = gridLayout.GetComponent<HorizontalLayoutGroup>().spacing + 400;


        //Set the target points
        //float multiplyer = 4.5f;
        //float multiplyer = (ships.Length ) / 2; 
        float multiplyer = 3.15f;

        targetPoints = new float[ships.Length];
        for (int i = 0; i < ships.Length; i++)
        {
            targetPoints[i] = stepSize * multiplyer;
            multiplyer -= 1f;
        }

        //Wait for a frame, and then initialize the position of the gridLayout content so that
        //we start at the first element
        yield return new WaitForEndOfFrame();
        gridLayout.anchoredPosition += Vector2.right * gridLayout.anchoredPosition.x * 2;


        //Move in the title slide
        titleTile.DOAnchorPosX(-titleTile.anchoredPosition.x, 0);
        yield return new WaitForSeconds(1);
        titleTile.DOAnchorPosX(-titleTile.anchoredPosition.x, 0.5f);
    }

    void LateUpdate()
    {
        GetFocusedShip();
        DetectInteractions();
        DetectDragEnd();
        SnapToNearest();
        HandleMainShipDisplay();
    }

    void GetFocusedShip()
    {
        if (isManualSnapping) return;
        //Get our currently focused ship and scale it
        for (int i = 0; i < ships.Length; i++)
        {
            float viewportPosX = Camera.main.ScreenToViewportPoint(ships[i].transform.position).x;
            if (viewportPosX > 0.3f && viewportPosX < 0.7f && focusedShip != i)
            {
                focusedShip = i;
                lastInteraction = Time.time;
                for (int j = 0; j < ships.Length; j++) ships[j].SetShipActive(j == i);
            }
        }
        //Set our target horizontal position based on the focused ship
        targetH = targetPoints[focusedShip];


    }

    void DetectDragEnd()
    {
        if (!Input.GetMouseButton(0)) LerpH = true;
    }

    void DetectInteractions()
    {
        if (Input.GetMouseButton(0)) lastInteraction = Time.time;
    }

    void SnapToNearest()
    {
        if (LerpH && !Input.GetMouseButton(0) && Time.time - lastInteraction >= timeBeforeSnap)
        {
            gridLayout.anchoredPosition = Vector2.Lerp(gridLayout.anchoredPosition, new Vector2(targetH, gridLayout.anchoredPosition.y), Time.deltaTime * snapSpeed);

        }
       
        if (Mathf.Abs(gridLayout.anchoredPosition.x - targetH) < stepSize / 4)
        {
            LerpH = false;
            isManualSnapping = false;
        }
    }

    public void OnClick(int id)
    {
        #region Handle Scroll Bar
        Debug.Log("click");
        isManualSnapping = true;
        focusedShip = id;
        //Set our target horizontal position based on the focused ship
        targetH = targetPoints[focusedShip];
        lastInteraction -= 1;
        for (int j = 0; j < ships.Length; j++) ships[j].SetShipActive(j == id);
        ships[id].SelectShip();
        selectedShip = id;
        //PlayerPrefs.SetInt("SelectedShip", id);
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySlectedCharacter = id;
            PlayerPrefs.SetInt("MyCharacter", id);
        }
        #endregion
    }

    public void onClickCharacterPick(int whichCharacter)
    {
        Debug.Log("is this called?");
        isManualSnapping = true;
        focusedShip = whichCharacter;
        targetH = targetPoints[focusedShip];
        lastInteraction -= 1;
        for (int j = 0; j < ships.Length; j++) ships[j].SetShipActive(j == whichCharacter);
        ships[whichCharacter].SelectShip();



        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySlectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }

    public void LoadNextScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    IEnumerator rotateShip(float duration)
    {
        bool keepRotating = true;
        float startTime = Time.realtimeSinceStartup;
        float timer = 0;
        while (keepRotating)
        {
            SelectedShip.transform.rotation = new Quaternion(0, 0, 100 * timer, 1);
            timer += Time.deltaTime;
            if (timer >= duration) keepRotating = false;
            yield return null;
        }
        //doneRotating = true;
    }

    IEnumerator Rotate(float duration)
    {
        doneRotating = false;

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
        while (!doneScaling && !doneRotating)
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

    bool detectedReady;
    void HandleMainShipDisplay()
    {
        if(isManualSnapping == false && LerpH == false)
        {
            if(!detectedReady)
            {
                detectedReady = true;
                Sprite selectedSprite = ships[focusedShip].shipImages[0].sprite;
                StartCoroutine(NewShip(selectedSprite));
            }
        }
        else
        {
            detectedReady = false;
        }
    }

}
