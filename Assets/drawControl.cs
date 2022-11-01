using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawControl : MonoBehaviour
{
    public GameObject LinePrefab;
    bool isMouseAlreadyDown = false;
    Vector3 lastMousePos;
    LineRenderer lr;
    GameObject gameObj;

    // Line parameters
    public float lineWidth = 0.75f;
    public float minLineWidth = 0.3f;
    public float maxLineWidth = 1.2f;
    public Color lineColor = Color.red;

    Color[] colors = { Color.red, Color.green, Color.blue, Color.black, Color.white };
    // Functions to change colors
    public void changeColor(int colorIndex)
    {
        lineColor = colors[colorIndex];
    }

    // Handling audio
    public AudioSource audi;

    public void playAudio()
    {
        audi.Play();
    }


    // Capturing Screenshot
    public void captureScreenshot()
    {
        ScreenCapture.CaptureScreenshot("screenshot.png");
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameObject gameObj = Instantiate(LinePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //LineRenderer lr = gameObj.GetComponent<LineRenderer>();
        //lr.positionCount = 2;
        //lr.SetPosition(0, new Vector3(0, 6, 0));
        //lr.SetPosition(1, new Vector3(1, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // Use mouse wheel to control brush size
        if (Input.mouseScrollDelta.y != 0) { 
                lineWidth += Input.mouseScrollDelta.y;
            if (lineWidth > maxLineWidth) lineWidth = maxLineWidth;
            if (lineWidth < minLineWidth) lineWidth = minLineWidth;
        }

        if (Input.GetMouseButton(0))
        {
            // Mouse not held down
            if (!isMouseAlreadyDown ) {
                isMouseAlreadyDown = true;
                // Instantiating a new LinePrefab everytime mouse is clicked after being released
                gameObj = Instantiate(LinePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                // Getting LineRenderer component of gameObj
                lr = gameObj.GetComponent<LineRenderer>();
                lastMousePos = Input.mousePosition;
                // We have 2 points making up a line to account for single click event
                lr.positionCount = 2;
                // Setting line width
                lr.startWidth = lineWidth;
                lr.endWidth = lineWidth;

                // Setting line color
                lr.startColor = lineColor;
                lr.endColor = lineColor;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                lr.SetPosition(0, mousePos);
                lr.SetPosition(1, mousePos);
            }

            // Mouse held down
            if (isMouseAlreadyDown)
            {
                // When mouse is being held down, we first check if it's a new coordinate. If so, we add a new point to the line renderer.
                //if(lastMousePos != Input.mousePosition)
                //{
                lastMousePos = Input.mousePosition;
                lr.positionCount++;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                lr.SetPosition(lr.positionCount - 1, mousePos);
                //}

            }
        }
        else if (Input.GetMouseButtonUp(0) && isMouseAlreadyDown) {
            isMouseAlreadyDown = false;
        }
    }

   
}
