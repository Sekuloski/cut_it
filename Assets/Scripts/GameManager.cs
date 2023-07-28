using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int score = 0;
    int highScore = 0;
    public bool isPlaying = false;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] GameObject startGame;
    [SerializeField] GameObject selectAxe;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    public void StartGame()
    {
        isPlaying = true;
        startGame.SetActive(false);
        selectAxe.SetActive(false);
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HighScore: " + score;
            UpdateHighScore();
        }
    }

    public void UpdateHighScore()
    {
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "HighScore: " + highScore;
    }

    public void EndGame()
    {
        isPlaying = false;
        Debug.Log("Game Over!");
    }
}
