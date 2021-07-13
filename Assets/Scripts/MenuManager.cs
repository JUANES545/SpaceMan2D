using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas GameOverCanvas;
    public Canvas InGameCanvas;
    
    private void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu() {
        menuCanvas.enabled = true;
    }
    
    public void HideMainMenu() {
        menuCanvas.enabled = false;
    }
    
    public void ShowGameOverMenu() {
        GameOverCanvas.enabled = true;
    }
    
    public void HideGameOverMenu() {
        GameOverCanvas.enabled = false;
    }
    
    public void ShowInGame() {
        InGameCanvas.enabled = true;
    }
    
    public void HideInGame() {
        InGameCanvas.enabled = false;
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Start(){
        
    }
    
    void Update(){
        
    }
}
