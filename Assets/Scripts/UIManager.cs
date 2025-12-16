using UnityEngine;
using TMPro; // Required for TextMeshPro elements

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;   
    public GameObject gameplayPanel;   
    public GameObject gameOverPanel;   

    [Header("Texts")]
    public TextMeshProUGUI scoreText;      
    public TextMeshProUGUI shotsText;      
    public TextMeshProUGUI finalScoreText; 


    void Start()
    {
        
        ShowMainMenu();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.uiManager = this;
        }
        else
        {
            Debug.LogWarning("UIManager could not find GameManager! Are both scenes loaded?");
        }
    }


    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowGameplay()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int finalScore)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
        }
    }

    public void OnStartGameClicked()
    {
        ShowGameplay();

        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure GameScene is loaded.");
        }
    }


    public void OnRestartClicked()
    {
        ShowMainMenu();
        
    }

   

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
        }
    }

    public void UpdateShotsUI(int shotsLeft)
    {
        if (shotsText != null)
        {
            shotsText.text = $"Shots: {shotsLeft}";
        }
    }
}