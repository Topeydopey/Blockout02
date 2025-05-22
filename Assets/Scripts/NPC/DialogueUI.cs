using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("Link UI Elements")]
    public GameObject dialoguePanel;          // ← drag the Panel (root) here
    public TextMeshProUGUI q1, q2, q3;
    public TextMeshProUGUI answerBox;

    NPCDialogue current;
    bool inDialogue;
    bool justOpened;                          // key-release guard
    public bool IsOpen => inDialogue;
    public TextMeshProUGUI closePrompt;

    void Update()
    {
        if (!inDialogue) return;

        /* ── Ignore input on the exact frame we opened ─────────────────── */
        if (justOpened)
        {
            if (!Input.GetKey(KeyCode.E)) justOpened = false;  // wait for release
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) Ask(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Ask(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Ask(2);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open(NPCDialogue npc)
    {
        current = npc;
        inDialogue = true;
        justOpened = true;

        dialoguePanel.SetActive(true);
        if (closePrompt) closePrompt.gameObject.SetActive(true);   // ← show prompt
        answerBox.text = "";

        SetQuestion(q1, npc.dialogue, 0);
        SetQuestion(q2, npc.dialogue, 1);
        SetQuestion(q3, npc.dialogue, 2);
    }

    void SetQuestion(TextMeshProUGUI t, QA[] list, int i)
    {
        if (i < list.Length && !string.IsNullOrWhiteSpace(list[i].question))
        {
            t.transform.parent.gameObject.SetActive(true);
            t.text = (i + 1) + ". " + list[i].question;
        }
        else
            t.transform.parent.gameObject.SetActive(false);
    }

    void Ask(int idx) => current.Talk(idx, ans => answerBox.text = ans);

    public void Close()
    {
        if (current)
        {
            var ag = current.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (ag) ag.enabled = true;

            /* ── clean up the look script ── */
            var look = current.GetComponent<SmoothLookAt>();
            if (look) Destroy(look);
        }

        dialoguePanel.SetActive(false);
        if (closePrompt) closePrompt.gameObject.SetActive(false);  // ← hide prompt
        inDialogue = false;
        current = null;
    }
}
