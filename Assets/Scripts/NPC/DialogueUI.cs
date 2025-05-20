using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Link UI Elements")]
    public TextMeshProUGUI q1; public TextMeshProUGUI q2; public TextMeshProUGUI q3;
    public TextMeshProUGUI answerBox;
    public Canvas canvas;

    NPCDialogue current;
    bool inDialogue;
    public bool IsOpen => inDialogue;   //  place inside DialogueUI class

    void Update()
    {
        if (!inDialogue) return;

        // Number-row keys (Alpha1..3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) Ask(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Ask(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Ask(2);

        // Quit with E or Esc
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open(NPCDialogue npc)
    {
        current = npc;
        inDialogue = true;
        canvas.enabled = true;
        answerBox.text = "";

        // Populate question texts (hide empty ones)
        SetQuestion(q1, npc.dialogue, 0);
        SetQuestion(q2, npc.dialogue, 1);
        SetQuestion(q3, npc.dialogue, 2);
    }

    void SetQuestion(TextMeshProUGUI txt, QA[] list, int i)
    {
        if (i < list.Length && !string.IsNullOrWhiteSpace(list[i].question))
        {
            txt.transform.parent.gameObject.SetActive(true);
            txt.text = (i + 1) + ".  " + list[i].question;
        }
        else
            txt.transform.parent.gameObject.SetActive(false);
    }

    void Ask(int index)
    {
        current.Talk(index, (ans) =>
        {
            answerBox.text = ans;
        });
    }

    public void Close()
    {
        if (current != null)
        {
            var agent = current.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent) agent.enabled = true;
        }
        inDialogue = false;
        canvas.enabled = false;
        current = null;
    }
}
