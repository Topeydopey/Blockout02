using UnityEngine;
using TMPro;

public class StaticInteraction : MonoBehaviour
{
    public string line = "…muffled voice behind the wall…";
    public TextMeshProUGUI promptText;      // “Press E”
    public TextMeshProUGUI dialogueText;    // anchor somewhere on HUD
    public float fadeAfter = 3f;

    bool playerInside;

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
        {
            promptText.gameObject.SetActive(false);
            StartCoroutine(ShowLine());
        }
    }

    System.Collections.IEnumerator ShowLine()
    {
        dialogueText.text = line;
        dialogueText.gameObject.SetActive(true);
        yield return new WaitForSeconds(fadeAfter);
        dialogueText.gameObject.SetActive(false);
    }
}
