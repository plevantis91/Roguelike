using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    
    [Header("Game UI Elements")]
    public Text treasureText;
    public Text livesText;
    public Text scoreText;
    public Slider healthBar;
    
    [Header("Buttons")]
    public Button startButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;
    
    [Header("Game Over UI")]
    public Text finalScoreText;
    public Text gameOverMessage;
    
    private bool isPaused = false;
    private int currentScore = 0;
    
    void Start()
    {
        SetupButtons();
        ShowMainMenu();
    }
    
    void SetupButtons()
    {
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }
    
    public void ShowMainMenu()
    {
        SetPanelActive(mainMenuPanel, true);
        SetPanelActive(gamePanel, false);
        SetPanelActive(pausePanel, false);
        SetPanelActive(gameOverPanel, false);
    }
    
    public void StartGame()
    {
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(gamePanel, true);
        SetPanelActive(pausePanel, false);
        SetPanelActive(gameOverPanel, false);
        
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        SetPanelActive(pausePanel, true);
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SetPanelActive(pausePanel, false);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void ShowGameOver(bool won)
    {
        Time.timeScale = 0f;
        SetPanelActive(gamePanel, false);
        SetPanelActive(gameOverPanel, true);
        
        if (gameOverMessage != null)
        {
            gameOverMessage.text = won ? "Level Complete!" : "Game Over!";
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {currentScore}";
        }
    }
    
    public void UpdateTreasureCount(int collected, int total)
    {
        if (treasureText != null)
        {
            treasureText.text = $"Treasures: {collected}/{total}";
        }
    }
    
    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {lives}";
        }
    }
    
    public void UpdateScore(int score)
    {
        currentScore = score;
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
    
    public void UpdateHealthBar(float healthPercentage)
    {
        if (healthBar != null)
        {
            healthBar.value = healthPercentage;
        }
    }
    
    void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
    
    void Update()
    {
        // Handle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
