using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float charDelay = 0.03f;
    public float fadeDelay = 3f;
    public float fadeDuration = 0.5f;

    public AudioSource audioSource;
    public AudioClip typeSound;

    private Coroutine typingCoroutine;
    private Coroutine fadeCoroutine;
    private bool isTyping = false;
    private string currentText;

    public void ShowText(string newText)
    {
        // Cancel any ongoing effects
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // Reset full opacity
        Color color = dialogueText.color;
        color.a = 1f;
        dialogueText.color = color;

        currentText = newText;
        dialogueText.text = "";

        typingCoroutine = StartCoroutine(TypeText(newText));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;

        if (typeSound != null && audioSource != null)
            audioSource.PlayOneShot(typeSound);

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
        typingCoroutine = null;

        // Wait before fading
        yield return new WaitForSeconds(fadeDelay);

        // Only fade if no new typing has started
        if (!isTyping && fadeCoroutine == null)
            fadeCoroutine = StartCoroutine(FadeOut());
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    public void SkipToEnd()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            dialogueText.text = currentText;
            isTyping = false;

            // Reset alpha just in case
            Color color = dialogueText.color;
            color.a = 1f;
            dialogueText.color = color;
        }
    }

    public void HideNow()
    {
        // Cancel typing and any ongoing fade
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startColor = dialogueText.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            dialogueText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dialogueText.text = "";
        dialogueText.color = new Color(startColor.r, startColor.g, startColor.b, 1f); // reset for next time
        fadeCoroutine = null;
    }
}
