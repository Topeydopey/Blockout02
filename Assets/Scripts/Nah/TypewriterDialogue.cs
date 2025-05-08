using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float charDelay = 0.03f;

    public AudioSource audioSource;
    public AudioClip typeSound;

    private Coroutine typingCoroutine;
    private Coroutine fadeCoroutine;
    private bool isTyping = false;
    private string currentText;

    public void ShowText(string newText)
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        currentText = newText;
        typingCoroutine = StartCoroutine(TypeText(newText));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typeSound != null && audioSource != null)
            audioSource.PlayOneShot(typeSound);

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
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
            dialogueText.text = currentText;
            isTyping = false;
        }
    }

    public void HideNow()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f;
        float elapsed = 0f;

        Color originalColor = dialogueText.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dialogueText.text = "";
        dialogueText.color = originalColor;
    }
}
