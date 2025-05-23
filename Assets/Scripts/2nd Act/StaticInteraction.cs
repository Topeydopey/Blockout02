using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class StaticInteraction : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [TextArea] public string[] lines;

    public TextMeshProUGUI promptText;
    public TextMeshProUGUI dialogueText;
    public float fadeAfter = 3f;

    [Header("Events")]
    public UnityEvent onFirstInteract;        //  ← NEW

    bool playerInside;
    int currentIndex;
    bool firstDone;
    Coroutine fadeCR;

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

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
            ShowNextLine();
    }

    void ShowNextLine()
    {
        if (lines == null || lines.Length == 0) return;

        if (!firstDone)
        {
            firstDone = true;
            onFirstInteract?.Invoke();        //  ← fire once
        }

        if (fadeCR != null) StopCoroutine(fadeCR);

        dialogueText.text = lines[currentIndex];
        dialogueText.gameObject.SetActive(true);

        currentIndex = (currentIndex + 1) % lines.Length;

        fadeCR = StartCoroutine(FadeAfterDelay());
    }

    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(fadeAfter);
        dialogueText.gameObject.SetActive(false);
    }
}
