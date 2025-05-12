using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int maxMistakes = 3;

    private int aliensKilled = 0;
    private int humansKilled = 0;
    private int totalAliens;

    public TextMeshProUGUI scoreText;
    public GameObject winScreen;
    public GameObject loseScreen;

    public void SetTotalAliens(int count)
    {
        totalAliens = count;
        UpdateScoreUI();
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Do not calculate totalAliens here anymore
        UpdateScoreUI();
    }

    public void RegisterKill(bool wasAlien)
    {
        if (wasAlien)
            aliensKilled++;
        else
            humansKilled++;

        UpdateScoreUI();

        if (humansKilled >= maxMistakes)
        {
            GameOver(false);
        }
        else if (aliensKilled >= totalAliens)
        {
            GameOver(true);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Aliens: {aliensKilled} / {totalAliens}    Mistakes: {humansKilled} / {maxMistakes}";
        }
    }

    void GameOver(bool win)
    {
        if (win && winScreen != null) winScreen.SetActive(true);
        if (!win && loseScreen != null) loseScreen.SetActive(true);

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // Freeze game
    }
}
