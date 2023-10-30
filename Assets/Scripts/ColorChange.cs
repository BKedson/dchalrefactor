using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    //stores reference to the Indicator Light
    public GameObject light;
    //stores the different materials associated with the different colors
    public Material red;
    public Material green;
    public Material defaultColor;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = light.GetComponent<Renderer>(); 
        rend.sharedMaterial = defaultColor;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method to change light
    public void changeLight(string name){
        //if name is red
        if(name == "red"){
            //set the material of this Gameobject to red
            rend.sharedMaterial = red;
        }
        //if name is green
        if(name == "green"){
            //set the material of this Gameobject to green
            rend.sharedMaterial = green;
        }
    }
}
