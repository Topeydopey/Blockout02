using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHotkey : MonoBehaviour
{
    [Tooltip("Key that restarts the level")]
    public KeyCode restartKey = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            // in case the game is paused / Time.timeScale = 0
            Time.timeScale = 1f;

            // lock & hide cursor again if your game starts that way
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
