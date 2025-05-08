using UnityEngine;
using TMPro;

public class NPCInterrogator : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI talkPromptText;
    public TextMeshProUGUI dialogueText;

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

                if (!talkPromptText.gameObject.activeSelf)
                    talkPromptText.gameObject.SetActive(true);

                if (Input.GetKeyDown(interactKey))
                {
                    string response = npc.GetRandomResponse();
                    dialogueText.text = response;
                    talkPromptText.gameObject.SetActive(false); // Hide after talking
                }

                return;
            }
        }

        // Nothing hit
        currentTarget = null;
        if (talkPromptText.gameObject.activeSelf)
            talkPromptText.gameObject.SetActive(false);
    }
}
