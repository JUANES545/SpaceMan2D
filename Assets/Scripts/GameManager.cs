using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour{
    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance;
    private PlayerController Controller;

    public int collectedObject = 0;

    void Awake(){
        if (sharedInstance == null){
            sharedInstance = this;
        }
        
    }
    void Start() {
        Controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
    void Update(){
        /*if (Input.GetKeyDown(KeyCode.Space)){
            StartGame();
        }*/
    }

    public void Submit(InputAction.CallbackContext context) {
        switch (currentGameState) {
            case GameState.menu: {
                if (context.performed) {
                    GetComponent<AudioSource>().Play();
                    StartGame();
                }
                break;
            }
            case GameState.inGame: {
                if (context.performed) {
                    BackToMenu();
                }
                break;
            }
            case GameState.gameOver: {
                if (context.performed) {
                    GetComponent<AudioSource>().Play();
                    StartGame();
                }
                break;
            }
        }
    }
    
    public void StartGame(){
        SetGameState(GameState.inGame);
        gameObject.GetComponent<PlayerInput>().actions.FindActionMap("InGame").Enable();
        gameObject.GetComponent<PlayerInput>().actions.FindActionMap("UI").Enable();
    }
    
    public void Gameover(){
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu(){
        SetGameState(GameState.menu);
        gameObject.GetComponent<PlayerInput>().actions.FindActionMap("InGame").Disable();
        gameObject.GetComponent<PlayerInput>().actions.FindActionMap("UI").Enable();
    }

    private void SetGameState(GameState newgameState){
        if (newgameState == GameState.menu){
            //TODO: colocar la lógica del menú
            MenuManager.sharedInstance.ShowMainMenu();
            MenuManager.sharedInstance.ShowInGame();
        }else if (newgameState == GameState.inGame){
            //TODO: hay que prparar la escena para jugar
            LevelManager.sharedInstance.RemoveAllLevelBlock();
            LevelManager.sharedInstance.generateInitialBlocks();
            Controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowInGame();
            MenuManager.sharedInstance.HideGameOverMenu();
        }else if (newgameState == GameState.gameOver){
            //TODO: prepara el juego para el Game over
            MenuManager.sharedInstance.ShowGameOverMenu();
            MenuManager.sharedInstance.HideInGame();
        }

        this.currentGameState = newgameState;
    }

    public void CollectObject(Collectable collectable) {
        collectedObject += collectable.value;
    }
    
}
