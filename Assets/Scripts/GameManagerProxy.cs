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

    public void SetMusicVolume(float vol){
        if (GameManager.manager) {
            GameManager.manager.SetMusicVolume(vol);
        }
    }

    public void SetSFXVolume(float vol){
        if (GameManager.manager) {
            GameManager.manager.SetSFXVolume(vol);
        }
    }

    public float GetMusicVolume(){
        if (GameManager.manager) {
            return GameManager.manager.GetMusicVolume();
        }
        return 1;
    }

    public float GetSFXVolume(){
        if (GameManager.manager) {
            return GameManager.manager.GetSFXVolume();
        }
        return 1;
    }

    public void SetSubjectSetting(int subject) {
        if (GameManager.manager) {
            GameManager.manager.SetSubjectSetting(subject);
        }
    }

    public int GetSubjectSetting() {
        if (GameManager.manager) {
            return GameManager.manager.GetSubjectSetting();
        }
        return 0;
    }

    public void ChangeSkin() {
        if (GameManager.manager) {
            GameManager.manager.ChangeSkin();
        }
    }

}
