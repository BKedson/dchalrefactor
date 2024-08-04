using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorBehavior : MonoBehaviour
{
    //[SerializeField] private GameManagerProxy gameManager;
    
    void Start()
    {
      //  int initialSize = gameManager.GetCursorSize();
        //if(initialSize < 2){
          //  initialSize = 10;
        //}
        //gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(initialSize, initialSize);
        //gameManager.SetPlayerCursor(this);

    }

    public void UpdateCursor(int size){
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }
}
