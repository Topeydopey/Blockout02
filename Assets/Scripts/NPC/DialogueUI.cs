using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Link UI Elements")]
    public GameObject dialoguePanel;          // root Panel
    public CanvasGroup canvasGroup;           // CanvasGroup on Panel
    public TextMeshProUGUI q1, q2, q3;        // question texts
    public TextMeshProUGUI answerBox;
    public TextMeshProUGUI closePrompt;       // "Press E to stop talking"

    [Header("Feedback")]
    public AudioClip[] selectSFX;             // drag 1-7 blip clips here

    NPCDialogue current;
    bool inDialogue;
    bool justOpened;                          // debounce for initial E
    bool answered;                            // set after first question
    AudioSource sfxSource;                    // plays the blips

    public bool IsOpen => inDialogue;

    /* ───────────────────────── initialisation ───────────────────── */

    void Awake()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;          // 2-D
        sfxSource.volume = 0.8f;
    }

    /* ───────────────────────── per-frame logic ──────────────────── */

    void Update()
    {
        if (!inDialogue) return;

        // Ignore input for the frame the panel opens
        if (justOpened)
        {
            if (!Input.GetKey(KeyCode.E)) justOpened = false;
            return;
        }

        // Allow number keys until player has asked one question
        if (!answered)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) Ask(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Ask(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Ask(2);
        }

        // Close on E / Esc after answer is displayed
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    /* ───────────────────────── open panel ───────────────────────── */

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

    /* ───────────────── ask once ──────────────────────────── */

    void Ask(int idx)
    {
        answered = true;

        // Keep only the chosen question visible
        q1.gameObject.SetActive(idx == 0);
        q2.gameObject.SetActive(idx == 1);
        q3.gameObject.SetActive(idx == 2);

        // Play a random blip
        if (selectSFX != null && selectSFX.Length > 0)
        {
            AudioClip clip = selectSFX[Random.Range(0, selectSFX.Length)];
            sfxSource.pitch = Random.Range(0.95f, 1.05f);   // slight variation
            sfxSource.PlayOneShot(clip);
        }

        // Show answer immediately
        current.Talk(idx, ans => answerBox.text = ans);

        if (closePrompt) closePrompt.gameObject.SetActive(true);
    }

    /* ─────────────── close & fade (0.3 s default) ─────────────── */

    public void Close() => StartCoroutine(FadeOut(0.3f));
    public void CloseWithFade(float t) => StartCoroutine(FadeOut(t));  // alias for Shotgun

    System.Collections.IEnumerator FadeOut(float dur)
    {
        // Re-enable wander & remove look script
        if (current)
        {
            var ag = current.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (ag) ag.enabled = true;

            var look = current.GetComponent<SmoothLookAt>();
            if (look) Destroy(look);
        }

        float startA = canvasGroup.alpha, tElapsed = 0f;
        while (tElapsed < dur)
        {
            canvasGroup.alpha = Mathf.Lerp(startA, 0f, tElapsed / dur);
            tElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        dialoguePanel.SetActive(false);
        if (closePrompt) closePrompt.gameObject.SetActive(false);
        inDialogue = false;
        current = null;
    }
}
