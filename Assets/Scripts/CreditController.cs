using UnityEngine;
using TMPro;
using System.Collections;

public class CreditController : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup panel;              // full-screen backplate (alpha 0 in Inspector)
    public TextMeshProUGUI creditText;     // your text object

    [Header("Timing")]
    public float fadeTime = 1f;         // seconds to fade in
    public float holdSeconds = 10f;        // how long to stay before quit

    void Awake()
    {
        panel.alpha = 0f;
        panel.gameObject.SetActive(false);
    }

    /* --------- called by BossManager.OnBossKilled() ---------- */
    public void ShowCredits()
    {
        panel.gameObject.SetActive(true);
        StartCoroutine(FadeAndQuit());
    }

    IEnumerator FadeAndQuit()
    {
        /* freeze gameplay + unlock cursor */
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        /* fade in */
        float t = 0f;
        while (t < fadeTime)
        {
            panel.alpha = t / fadeTime;
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        panel.alpha = 1f;

        /* hold */
        yield return new WaitForSecondsRealtime(holdSeconds);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // stop play-mode
#else
        Application.Quit();
#endif
    }
}
