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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        totalAliens = FindObjectsOfType<DesignerNPC>().Length; // Count at start
        foreach (var npc in FindObjectsOfType<DesignerNPC>())
        {
            if (!npc.isAlien) totalAliens--;
        }

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

        Time.timeScale = 0f; // Freeze game
    }
}
