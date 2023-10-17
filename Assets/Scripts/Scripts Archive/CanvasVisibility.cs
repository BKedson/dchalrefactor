using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The UI elements in the second canvas must have their alpha controlled by the first canvas

public class CanvasVisibility : MonoBehaviour
{
    //reference to main canvas
    public GameObject mainCanvas;
    //reference to the GameObject containing the canvas group of fade
    public GameObject canvasImage;
    //stores the alpha value of canvasImage
    public float alphaValue;

    // Update is called once per frame
    void Update()
    {
        //set the update method to get the current alpha of the canvas 
        alphaValue = canvasImage.GetComponent<CanvasGroup>().alpha;
        //mainCanvas updater
        mainCanvas.GetComponent<CanvasGroup>().alpha = 1-alphaValue;
    }
}
