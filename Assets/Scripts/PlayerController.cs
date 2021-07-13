using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = System.Numerics.Vector3;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20f;
    public float runningSpeed = 5f;
    
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private UnityEngine.Vector3 startPosition;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";
    private string STATE_MOVEMENT = "isMoving";

    private int healthPoints, manaPoints;

    [SerializeField] private PlayerInput playerInput;
    public LayerMask groundMask;
    private float inputX;
    private float inputY;

    public const int INITIAL_HEALTH = 100,
        INITIAL_MANA = 15,
        MAX_HEALTH = 200,
        MAX_MANA = 30,
        MIN_HEALTH = 10,
        MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;
    public const int SUPERRUN_COST = 5;
    public const float SUPERRUN_FORCE = 1.5f;
    float runFactor = 5f;

    private void Awake(){
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start(){
        
        animator.SetBool(STATE_MOVEMENT, false);

        startPosition = this.transform.position;
    }

    public void StartGame() {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
        
        Invoke(nameof(RestartPosition), 0.5f);
    }

    public void RestartPosition() {
        this.transform.position = startPosition;
        this.playerRigidbody.velocity = Vector2.zero;
        /*GameObject mainCamara = GameObject.Find("Main Camara");
        mainCamara.GetComponent<CamaraFollow>().ResetCamaraPosition();*/
    }

    void Update(){
        animator.enabled = true;
        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
        animator.SetBool(STATE_MOVEMENT, IsMoving());
    }

    void FixedUpdate(){
        /*if (playerRigidbody.velocity.x < runningSpeed){
            playerRigidbody.velocity = new Vector2(runningSpeed, playerRigidbody.velocity.y);
        }*/
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            playerRigidbody.velocity = new Vector2(inputX * runFactor,
                playerRigidbody.velocity.y);
        }
        else
            playerRigidbody.IsSleeping();
    }
    
    private void Animation(float inputX, float inputY, InputAction.CallbackContext context){
        if (inputY < 0)
            spriteRenderer.flipX = true;
        if (inputX > 0)
            spriteRenderer.flipX = false;
        if (inputY < 0)
            if (context.performed) {
                Fall();
            }
        if (inputY > 0)
            if (context.performed) {
                Jump(false);
            }
    }

    public void Move(InputAction.CallbackContext context){
        inputX = context.ReadValue<Vector2>().x;
        inputY = context.ReadValue<Vector2>().y;
        Animation(inputX, inputY, context);
    }
    
    public void Jump(InputAction.CallbackContext context){
        if (context.performed){
            Jump(false);
        }
    }
    
    public void SuperJump(InputAction.CallbackContext context){
        if (context.performed){
            Jump(true);
        }
    }
    
    public void SuperRun(InputAction.CallbackContext context){
        if (context.started) {
            Run(true);
        }else if (context.canceled) {
            Run(false);
        }
    }
    
    public void Run(bool superRun){
        if (superRun && manaPoints >= SUPERRUN_COST) {
            manaPoints -= SUPERRUN_COST;
            runFactor *= SUPERRUN_FORCE;
        }

        if (!superRun)
        {
            runFactor = runningSpeed;
        }

    }
    
    public void Down(InputAction.CallbackContext context){
        if (context.performed){
            Fall();
        }
    }
    void Jump(bool superJump) {
        float jumpForceFactor = jumpForce;
        if (superJump && manaPoints>= SUPERJUMP_COST) {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (IsTouchingTheGround()){
            GetComponent<AudioSource>().Play();
            playerRigidbody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
        }
    }
    
    void Fall(){
        if (!IsTouchingTheGround()) {
            playerRigidbody.AddForce(Vector2.down * (jumpForce*2), ForceMode2D.Impulse);
        }
    }

    bool IsTouchingTheGround() {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundMask)){
            //TODO: 
            //animator.enabled = true;
            return true;
        }else{
            //TODO:
            //animator.enabled = false;
            return false;
        }
    }

    bool IsMoving() {
        return playerRigidbody.velocity.x != 0;
    }

    public void Die() {
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("MaxScore", 0f);
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("MaxScore", travelledDistance);
        }
      
        this.animator.SetBool(STATE_ALIVE, false);
        this.playerRigidbody.velocity = Vector2.zero;
        GameManager.sharedInstance.Gameover();
    }

    public void CollectHealth(int points) {
        this.healthPoints += points;
        if (this.healthPoints >= MAX_HEALTH) {
            this.healthPoints = MAX_HEALTH;
        }

        if (this.healthPoints <= 0) {
            Die();
        }
    }
    
    public void CollectMana(int points) {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA) {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth() {
        return healthPoints;
    }
    
    public int GetMana() {
        return manaPoints;
    }

    public float GetTravelledDistance() {
        return this.transform.position.x - startPosition.x;
    }
}
