using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorExampleBehavior : MonoBehaviour
{
    
    [SerializeField] private Slider slider;
    [SerializeField] private GameManagerProxy gameManager;
    // Start is called before the first frame update
    void Start()
    {
        int initialSize = gameManager.GetCursorSize();
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(initialSize, initialSize);
        slider.value = initialSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCursorSize(){
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(slider.value, slider.value);
        gameManager.SetCursorSize((int) slider.value);
    }
}
