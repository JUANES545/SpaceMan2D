using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 3;
    private Rigidbody2D _rigidbody;
    public bool facingRight = false;
    private Vector3 StartPosition;

    public int enemyDamage = 50;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //StartPosition = this.transform.position;
    }

    void Start() {
        //this.transform.position = StartPosition;
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;
        if (facingRight) {
            currentRunningSpeed = runningSpeed;
            //GetComponent<SpriteRenderer>().flipX = true;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else {
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            _rigidbody.velocity = new Vector2(currentRunningSpeed, _rigidbody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin")) {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }
        //si llegamos aquí, no ha chocado con nada de lo anterior, por lo tanto debe girar el enemigo.
        facingRight = !facingRight;
    }
}
