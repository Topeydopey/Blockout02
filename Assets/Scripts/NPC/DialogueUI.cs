using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Link UI Elements")]
    public GameObject dialoguePanel;                // root Panel
    public CanvasGroup canvasGroup;                 // CanvasGroup on Panel
    public TextMeshProUGUI q1, q2, q3;              // question texts
    public TextMeshProUGUI answerBox;
    public TextMeshProUGUI closePrompt;             // “Press E to stop talking”

    NPCDialogue current;
    bool inDialogue;
    bool justOpened;    // debounce for initial E press
    bool answered;      // true after player selects a question

    public bool IsOpen => inDialogue;

    /* ─────────────────────────────────────────────────────────────── */

    void Update()
    {
        if (!inDialogue) return;

        // wait until E key is released right after opening
        if (justOpened)
        {
            if (!Input.GetKey(KeyCode.E)) justOpened = false;
            return;
        }

        // allow number keys only until first question picked
        if (!answered)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) Ask(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Ask(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Ask(2);
        }

        // close on E or Esc after answer is displayed
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open(NPCDialogue npc)
    {
        current = npc;
        inDialogue = true;
        justOpened = true;
        answered = false;

        dialoguePanel.SetActive(true);
        canvasGroup.alpha = 1f;

        if (closePrompt) closePrompt.gameObject.SetActive(false);
        answerBox.text = "";

        SetQuestion(q1, npc.dialogue, 0);
        SetQuestion(q2, npc.dialogue, 1);
        SetQuestion(q3, npc.dialogue, 2);
    }

    /* show / hide individual question texts */
    void SetQuestion(TextMeshProUGUI t, QA[] list, int i)
    {
        if (i < list.Length && !string.IsNullOrWhiteSpace(list[i].question))
        {
            t.gameObject.SetActive(true);
            t.text = $"{i + 1}. {list[i].question}";
        }
        else
            t.gameObject.SetActive(false);
    }

    /* ─────────────── ask once ─────────────── */

    void Ask(int idx)
    {
        answered = true;

        // keep only the chosen question visible
        q1.gameObject.SetActive(idx == 0);
        q2.gameObject.SetActive(idx == 1);
        q3.gameObject.SetActive(idx == 2);

        // show answer immediately
        current.Talk(idx, ans => answerBox.text = ans);

        if (closePrompt) closePrompt.gameObject.SetActive(true);
    }

    /* ─────────────── close & fade (0.3 s) ─────────────── */

    public void Close() => StartCoroutine(FadeOut(0.3f));
    public void CloseWithFade(float t) => StartCoroutine(FadeOut(t));  // alias for Shotgun

    System.Collections.IEnumerator FadeOut(float dur)
    {
        // re-enable wander & remove look script
        if (current)
        {
            var ag = current.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (ag) ag.enabled = true;
            var look = current.GetComponent<SmoothLookAt>();
            if (look) Destroy(look);
        }

        float startA = canvasGroup.alpha, t = 0f;
        while (t < dur)
        {
            canvasGroup.alpha = Mathf.Lerp(startA, 0f, t / dur);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        dialoguePanel.SetActive(false);
        if (closePrompt) closePrompt.gameObject.SetActive(false);
        inDialogue = false;
        current = null;
    }
}
