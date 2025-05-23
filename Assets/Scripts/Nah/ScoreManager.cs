using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Limits")]
    public int maxMistakes = 3;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Events")]
    public UnityEvent onAllAliensKilled;          // fired when last alien dies

    /* --------------- internal state --------------- */
    int aliensKilled;
    int humansKilled;
    int totalAliens;
    bool act2Started;                             // NEW: prevents double-fire

    /* --------------- unity lifecycle -------------- */
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() => UpdateScoreUI();

    /* --------------- external API ----------------- */
    public void SetTotalAliens(int count)
    {
        totalAliens = count;
        UpdateScoreUI();
    }

    public void RegisterKill(bool wasAlien)
    {
        if (wasAlien) aliensKilled++;
        else humansKilled++;

        UpdateScoreUI();

        /* ---------- player loses --------- */
        if (humansKilled >= maxMistakes)
        {
            ShowLose();
            return;
        }

        /* ---------- all aliens dead ----- */
        if (!act2Started && aliensKilled >= totalAliens)
        {
            act2Started = true;
            onAllAliensKilled?.Invoke();          // trigger basement sequence
            return;                              // <-- skip win/pause for Act-2
        }
    }

    /* --------------- UI helpers ------------------- */
    void UpdateScoreUI()
    {
        if (scoreText)
            scoreText.text = $"Aliens: {aliensKilled}/{totalAliens}    Mistakes: {humansKilled}/{maxMistakes}";
    }

    void ShowLose()
    {
        if (loseScreen) loseScreen.SetActive(true);
        PauseGame();
    }

    /* Optional: call this later if you still want a win panel after Act-2 */
    public void ShowWinPanelManually()
    {
        if (winScreen) winScreen.SetActive(true);
        PauseGame();
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
