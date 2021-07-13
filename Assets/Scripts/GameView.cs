using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Text scoreText, maxScoreText, coinsText;

    private PlayerController _controller;
    void Start()
    {
        _controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            int coins = GameManager.sharedInstance.collectedObject;
            float score = _controller.GetTravelledDistance();
            float maxScore = PlayerPrefs.GetFloat("MaxScore", 0);

            coinsText.text = coins.ToString();
            scoreText.text = "Score: " + score.ToString("f1");
            maxScoreText.text = "Max Score: " + maxScore.ToString("f1");
        }
    }
}
