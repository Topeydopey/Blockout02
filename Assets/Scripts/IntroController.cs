using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("UI refs")]
    public Canvas canvas;                // Intro / Dialogue canvas
    public Image black;                 // Full-screen black (alpha 1 in Inspector)
    public TextMeshProUGUI introText;

    [Header("Sequence")]
    [TextArea] public string[] lines;
    public float lineFadeTime = 0.5f;
    public float lineHoldTime = 2.0f;
    public float finalFadeTime = 1.0f;   // normal fade
    public float skipFadeTime = 0.25f;  // fast fade when skipped
    public KeyCode skipKey = KeyCode.Escape;

    [Header("Control scripts to disable")]
    public MonoBehaviour[] controlScripts;   // movement, look, etc.
    public bool lockCursorDuringIntro = true;

    /* ──────────────────────────────────────────────────────────────── */
    bool skipRequested;

    void Update()
    {
        if (!skipRequested && Input.GetKeyDown(skipKey))
            skipRequested = true;              // user pressed Esc
    }

    void Start() => StartCoroutine(RunIntro());

    IEnumerator RunIntro()
    {
        /* 1 ─ disable controls, keep camera alive */
        foreach (var s in controlScripts) if (s) s.enabled = false;
        if (lockCursorDuringIntro)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        canvas.enabled = true;                 // show intro panel

        /* 2 ─ line-by-line text, abort when skipRequested is true */
        foreach (string line in lines)
        {
            if (skipRequested) break;

            introText.text = line;
            yield return FadeText(0f, 1f, lineFadeTime);         // fade in
            if (skipRequested) break;

            yield return new WaitForSeconds(lineHoldTime);       // hold
            if (skipRequested) break;

            yield return FadeText(1f, 0f, lineFadeTime);         // fade out
        }

        /* 3 ─ fade black out (shorter if skipped) */
        float fadeTime = skipRequested ? skipFadeTime : finalFadeTime;
        float t = 0f;
        Color bc = black.color;

        while (t < fadeTime)
        {
            float a = Mathf.Lerp(1f, 0f, t / fadeTime);
            black.color = new Color(bc.r, bc.g, bc.b, a);
            t += Time.deltaTime;
            yield return null;
        }
        black.color = new Color(bc.r, bc.g, bc.b, 0f);           // ensure zero alpha

        /* 4 ─ hide intro & re-enable controls */
        canvas.enabled = false;

        foreach (var s in controlScripts) if (s) s.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator FadeText(float from, float to, float dur)
    {
        float t = 0f;
        Color c = introText.color;

        while (t < dur)
        {
            float a = Mathf.Lerp(from, to, t / dur);
            introText.color = new Color(c.r, c.g, c.b, a);
            t += Time.deltaTime;
            yield return null;

            if (skipRequested) break;          // abort mid-fade if user skips
        }
        introText.color = new Color(c.r, c.g, c.b, to);
    }
}
