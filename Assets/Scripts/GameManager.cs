using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int MaxShots = 10; 
    public int CurrentShots { get; private set; }
    public int Score = 0;

    [Header("References")]
    public UIManager uiManager;
    public LevelGenerator levelGenerator;

    public bool IsGamePlaying { get; private set; } = false;

    void Awake() { Instance = this; }

    void Start()
    {
        SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        IsGamePlaying = false;
    }

    public void StartGame()
    {
        Score = 0;
        CurrentShots = MaxShots;

        if (levelGenerator != null)
        {
            levelGenerator.RestartLevel();
        }

        IsGamePlaying = true;
        if (uiManager != null)
        {
            uiManager.UpdateScoreUI(Score);
            uiManager.UpdateShotsUI(CurrentShots);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if (uiManager != null) uiManager.UpdateScoreUI(Score);
    }

    public bool TryShoot()
    {
        if (CurrentShots > 0)
        {
            CurrentShots--;
            if (uiManager != null) uiManager.UpdateShotsUI(CurrentShots);
            return true; 
        }
        else
        {
            Debug.Log("Out of Ammo!");
            return false; 
        }
    }

    public void OnBallDestroyed()
    {
        if (CurrentShots <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        IsGamePlaying = false; 
        Debug.Log("Game Over!");

        if (uiManager != null)
        {
            uiManager.ShowGameOver(Score);
        }
    }


}
