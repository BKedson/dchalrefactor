using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameManager is a singleton and marked as DoNotDestroy, so it's difficult to reference the GameManager object in the Editor
// This class serves as an intermediary between things that need a GameManager reference object and the GameManager
public class GameManagerProxy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInvincibility(bool inv) {
        if (GameManager.manager) {
            GameManager.manager.SetInvincibility(inv);
        }
    }

    public bool IsInvincible(){
        if(GameManager.manager){
            return GameManager.manager.IsInvincible();
        }
        return false;
    }

    public void SetCursorSize(int size){
        if (GameManager.manager) {
            GameManager.manager.SetCursorSize(size);
        }
    }

    public int GetCursorSize(){
        if(GameManager.manager){
            return GameManager.manager.GetCursorSize();
        }
        return 10;
    }

    public void SetPlayerCursor(CursorBehavior cursor){
        if (GameManager.manager) {
            GameManager.manager.SetPlayerCursor(cursor);
        }
    }

    public void ChangeSkin() {
        if (GameManager.manager) {
            GameManager.manager.ChangeSkin();
        }
    }

}
