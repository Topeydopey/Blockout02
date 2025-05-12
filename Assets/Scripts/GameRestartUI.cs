using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestartUI : MonoBehaviour
{
    public void RestartGame()
    {
        // Unlock time
        Time.timeScale = 1f;

        // Lock and hide cursor again for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
