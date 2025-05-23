using UnityEngine;
using TMPro;
using System.Collections;

public class StaticInteraction : MonoBehaviour
{
    [Header("Dialogue Lines (one per press)")]
    [TextArea] public string[] lines;      // fill in Inspector

    public TextMeshProUGUI promptText;     // “Press E”
    public TextMeshProUGUI dialogueText;   // centre-HUD text
    public float fadeAfter = 3f;           // seconds each line stays

    bool playerInside;
    int currentIndex;                     // next line to show
    Coroutine fadeCR;                      // so lines don’t overlap

    /* ───────────── trigger sensing ───────────── */

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = true;
            promptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = false;
            promptText.gameObject.SetActive(false);
        }
    }

    /* ───────────── per-frame input ───────────── */

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
            ShowNextLine();
    }

    /* ───────────── dialogue logic ───────────── */

    void ShowNextLine()
    {
        if (lines == null || lines.Length == 0) return;

        // stop any running fade so text updates instantly
        if (fadeCR != null) StopCoroutine(fadeCR);

        dialogueText.text = lines[currentIndex];
        dialogueText.gameObject.SetActive(true);

        currentIndex = (currentIndex + 1) % lines.Length;   // loop

        fadeCR = StartCoroutine(FadeAfterDelay());
    }

    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(fadeAfter);
        dialogueText.gameObject.SetActive(false);
    }
}
