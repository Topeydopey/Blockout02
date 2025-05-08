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
    private bool isTyping = false;
    private string currentText;

    public void ShowText(string newText)
    {
        if (isTyping)
        {
            // Skip to full reveal
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentText;
            isTyping = false;
            return;
        }

        currentText = newText;
        typingCoroutine = StartCoroutine(TypeText(newText));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        // 🔊 Play type sound once at the start
        if (typeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(typeSound);
        }

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
}
