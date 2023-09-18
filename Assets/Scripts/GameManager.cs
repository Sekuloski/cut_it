using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int score = 0;
    int highScore = 0;
    string username = "";
    public bool isPlaying = false;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TMP_InputField usernameText;
    [SerializeField] GameObject bottomBorder;

    [SerializeField] GameObject postGameButtons;
    [SerializeField] GameObject menuButtons;
    [SerializeField] Button StartGameButton;
    [SerializeField] Button EndGameButton;
    [SerializeField] Button RestartGameButton;
    [SerializeField] Animator Settings;
    [SerializeField] Animator Menu;

    CanvasGroup postGameButtonsCanvas;
    CanvasGroup menuButtonsCanvas;
    bool postGameFadeIn = false;
    bool postGameFadeOut = false;
    bool menuFadeIn = false;
    bool menuFadeOut = false;

    bool leaderboardUpdated = false;

    [SerializeField] Plank plank;
    API api;
    Services services;

    private void Awake()
    {
        api = GetComponent<API>();
        services = GetComponent<Services>();
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
        LoadUsername();
        postGameButtonsCanvas = postGameButtons.GetComponent<CanvasGroup>();
        postGameButtonsCanvas.alpha = 0;
        EndGameButton.interactable = false;
        RestartGameButton.interactable = false;
        menuButtonsCanvas = menuButtons.GetComponent<CanvasGroup>();

        api.GetPlayerData();
    }

    public void ToggleSettings()
    {
        Menu.enabled = false;
        Settings.enabled = true;
        Settings.SetTrigger("Open");
    }

    public void ToggleMenu()
    {
        Settings.enabled = false;
        Menu.enabled = true;
        Menu.SetTrigger("Open");
    }

    private void Update()
    {
        if (!leaderboardUpdated && api.players.Count > 0)
        {
            FindObjectOfType<AxeSelector>().UpdateLeaderboard(api.players, services.location, username);
            leaderboardUpdated = true;
        }
        if (postGameFadeIn)
        {
            if (postGameButtonsCanvas.alpha < 1)
            {
                postGameButtonsCanvas.alpha += Time.deltaTime * 2f;
            }
            else
            {
                postGameFadeIn = false;
            }
        }
        if (postGameFadeOut)
        {
            if (postGameButtonsCanvas.alpha > 0)
            {
                postGameButtonsCanvas.alpha -= Time.deltaTime;
            }
            else
            {
                postGameFadeOut = false;
            }
        }

        if (menuFadeIn)
        {
            if (menuButtonsCanvas.alpha < 1)
            {
                menuButtonsCanvas.alpha += Time.deltaTime;
            }
            else
            {
                menuFadeIn = false;
                StartGameButton.interactable = true;
            }
        }
        if (menuFadeOut)
        {
            if (menuButtonsCanvas.alpha > 0)
            {
                menuButtonsCanvas.alpha -= Time.deltaTime;
            }
            else
            {
                menuFadeOut = false;
            }
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        menuFadeOut = true;
        StartGameButton.interactable = false;

        PlayerPrefs.SetString("Username", usernameText.text);
        PlayerPrefs.Save();
    }

    public void StopGame()
    {
        // User has lost
        isPlaying = false;
        postGameFadeIn = true;
        EndGameButton.interactable = true;
        RestartGameButton.interactable = true;
    }

    public void EndGame()
    {
        // User quits the current game
        StartCoroutine(DisableBorder());
        plank.ResetPlank();

        // Fade the menu buttons in        
        menuFadeIn = true;

        // Fade the End Game buttons out
        postGameFadeOut = true;
        EndGameButton.interactable = false;
        RestartGameButton.interactable = false;

        if (score >= highScore)
        {
            api.UpdateData(username, highScore, services.country);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(DisableBorder());

        // Fade the End Game buttons out
        postGameFadeOut = true;
        EndGameButton.interactable = false;
        RestartGameButton.interactable = false;

        isPlaying = true;
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HighScore: " + score;
            UpdateHighScore();
        }
    }

    public void UpdateHighScore()
    {
        // Update Highscore to local storage
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        // Load Highscore from local storage
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "HighScore: " + highScore;
    }

    public void LoadUsername()
    {
        // Load Username from local storage
        username = PlayerPrefs.GetString("Username", "");
        usernameText.text = username;
        if (username == "")
        {
            highScore = 0;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreText.text = "HighScore: 0";
        }
    }

    private IEnumerator DisableBorder()
    {
        bottomBorder.SetActive(false);

        yield return new WaitForSeconds(2);

        bottomBorder.SetActive(true);
    }
}
