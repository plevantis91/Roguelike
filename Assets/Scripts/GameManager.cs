using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int totalTreasures = 5;
    public int playerLives = 3;
    
    [Header("UI References")]
    public Text treasureText;
    public Text livesText;
    public Text gameOverText;
    public Button restartButton;
    
    [Header("Audio")]
    public AudioClip levelCompleteSound;
    public AudioClip gameOverSound;
    
    private int collectedTreasures = 0;
    private int currentLives;
    private AudioSource audioSource;
    
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        currentLives = playerLives;
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);
    }
    
    public void CollectTreasure()
    {
        collectedTreasures++;
        UpdateUI();
        
        if (collectedTreasures >= totalTreasures)
        {
            LevelComplete();
        }
    }
    
    public void TakeDamage()
    {
        currentLives--;
        UpdateUI();
        
        if (currentLives <= 0)
        {
            GameOver();
        }
    }
    
    public void LevelComplete()
    {
        if (audioSource != null && levelCompleteSound != null)
            audioSource.PlayOneShot(levelCompleteSound);
            
        if (gameOverText != null)
        {
            gameOverText.text = "Level Complete!\nAll treasures collected!";
            gameOverText.gameObject.SetActive(true);
        }
        
        if (restartButton != null)
            restartButton.gameObject.SetActive(true);
    }
    
    void GameOver()
    {
        if (audioSource != null && gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);
            
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over!\nTry Again!";
            gameOverText.gameObject.SetActive(true);
        }
        
        if (restartButton != null)
            restartButton.gameObject.SetActive(true);
    }
    
    void UpdateUI()
    {
        if (treasureText != null)
            treasureText.text = $"Treasures: {collectedTreasures}/{totalTreasures}";
            
        if (livesText != null)
            livesText.text = $"Lives: {currentLives}";
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        // For future levels
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
