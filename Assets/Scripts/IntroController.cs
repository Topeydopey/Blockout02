using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("UI refs")]
    public Canvas canvas;          // the DialogueCanvas (or IntroCanvas)
    public Image black;           // full-screen black image (alpha 1 in Inspector)
    public TextMeshProUGUI introText;

    [Header("Sequence")]
    [TextArea] public string[] lines;
    public float lineFadeTime = 0.5f;
    public float lineHoldTime = 2.0f;
    public float finalFadeTime = 1.0f;

    [Header("Control scripts to disable")]
    public MonoBehaviour[] controlScripts;   // drag movement / look scripts here
    public bool lockCursorDuringIntro = true;

    void Start() => StartCoroutine(RunIntro());

    IEnumerator RunIntro()
    {
        /* ── 1. Disable player input, keep camera alive ─────────────── */
        foreach (var s in controlScripts) if (s) s.enabled = false;
        if (lockCursorDuringIntro)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        canvas.enabled = true;            // show intro panel

        /* ── 2. Run line-by-line text fades ─────────────────────────── */
        foreach (string line in lines)
        {
            introText.text = line;
            yield return FadeText(0f, 1f, lineFadeTime);   // fade in
            yield return new WaitForSeconds(lineHoldTime); // hold
            yield return FadeText(1f, 0f, lineFadeTime);   // fade out
        }

        /* ── 3. Fade black out to reveal gameplay ───────────────────── */
        float t = 0f, a;
        Color bc = black.color;
        while (t < finalFadeTime)
        {
            a = Mathf.Lerp(1f, 0f, t / finalFadeTime);
            black.color = new Color(bc.r, bc.g, bc.b, a);
            t += Time.deltaTime;
            yield return null;
        }

        /* ── 4. Hide intro panel & re-enable controls ───────────────── */
        canvas.enabled = false;

        foreach (var s in controlScripts) if (s) s.enabled = true;
        if (lockCursorDuringIntro)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;  // or None if you prefer
        }
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
        }
        introText.color = new Color(c.r, c.g, c.b, to);
    }
}
