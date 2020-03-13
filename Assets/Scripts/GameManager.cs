using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public GameObject shootButton;
    public GameObject displayScore;
    public AudioSource dieAudio;
    public Text scoreText;
    public bool started = false;
    public Button ShootButton;
    public Button JumpButton;
    public PlayerController player;

    enum PageState
    {
        None,
        Start,
        GameOver,
        CountDown
    }

    public int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }
    public int Score { get { return score; } }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        SetPageState(PageState.CountDown);
    }

    private void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        PlayerController.OnPlayerDied += OnPlayerDied;
        PlayerController.OnPlayerScored += OnPlayerScored;
    }

    private void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        PlayerController.OnPlayerDied -= OnPlayerDied;
        PlayerController.OnPlayerScored -= OnPlayerScored;
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if(score > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.jumping)
            JumpButton.interactable = false;
        else
            JumpButton.interactable = true;
        if (player.shooting)
            ShootButton.interactable = false;
        else
            ShootButton.interactable = true;
    }

    void SetPageState(PageState state)
    {
        switch(state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                shootButton.SetActive(true);
                displayScore.SetActive(true);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                shootButton.SetActive(false);
                displayScore.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                shootButton.SetActive(false);
                displayScore.SetActive(false);
                dieAudio.Play();
                break;
            case PageState.CountDown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                shootButton.SetActive(true);
                displayScore.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
    {
        //activated when replay button is hit
        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        //activated when play button is hit
        SetPageState(PageState.CountDown);
    }

    public void ReturnToHomeScreen()
    {
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }
}
