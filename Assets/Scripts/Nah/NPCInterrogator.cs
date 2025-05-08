using UnityEngine;
using TMPro;

public class NPCInterrogator : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI talkPromptText;

    [Header("Dialogue")]
    public TypewriterDialogue typewriter;

    private DesignerNPC currentTarget;

    void Update()
    {
        // Raycast forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            DesignerNPC npc = hit.collider.GetComponent<DesignerNPC>();
            if (npc != null)
            {
                currentTarget = npc;

                if (!talkPromptText.gameObject.activeSelf && !typewriter.IsTyping())
                    talkPromptText.gameObject.SetActive(true);

                if (Input.GetKeyDown(interactKey))
                {
                    if (typewriter.IsTyping())
                    {
                        typewriter.SkipToEnd(); // Skip to full line
                    }
                    else
                    {
                        string response = npc.GetNextResponse();
                        typewriter.ShowText(response);
                        talkPromptText.gameObject.SetActive(false); // Hide prompt during typing
                    }
                }

                return;
            }
        }

        // Nothing hit
        currentTarget = null;

        if (!typewriter.IsTyping() && talkPromptText.gameObject.activeSelf)
            talkPromptText.gameObject.SetActive(false);
    }
}
