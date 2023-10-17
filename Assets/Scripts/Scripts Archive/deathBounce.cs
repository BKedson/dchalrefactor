using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathBounce : MonoBehaviour
{

    private float horizspeed = 90f;
    private float vertspeed = 60f;
    
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject dText;
    // Update is called once per frame
    void Update()
    {
        var transform = this.GetComponent<RectTransform>();
        var canvas = canvasUI.GetComponent<RectTransform>();

        var height = canvas.rect.height;
        var width = canvas.rect.width;

        if(this.transform.position.x > height){
            horizspeed *= -1;
        }
        if(this.transform.position.y > width){
            vertspeed *= -1;
        }
        transform.Translate(horizspeed * Time.deltaTime, vertspeed * Time.deltaTime,0);

    }
}
