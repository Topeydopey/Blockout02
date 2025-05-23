using UnityEngine;
using TMPro;

public class NPCInterrogator : MonoBehaviour
{
    [Header("Ray-cast & prompt")]
    public Camera playerCamera;
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI talkPromptText;   //  “Press E to Talk”

    [Header("New Dialogue UI")]
    public DialogueUI dialogueUI;            //  Drag the DialogueCanvas here

    // ───────────────────────────────────────────────────────────────
    DesignerNPC currentTarget;

    void Update()
    {
        // If the dialogue window is already open, ignore ray-cast logic.
        if (dialogueUI != null && dialogueUI.IsOpen) return;

        // 1. Ray-cast straight ahead from the camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            DesignerNPC npc = hit.collider.GetComponent<DesignerNPC>();

            if (npc != null)
            {
                currentTarget = npc;

                // Show the on-screen prompt
                if (!talkPromptText.gameObject.activeSelf)
                    talkPromptText.gameObject.SetActive(true);

                // Player presses E → open dialogue
                if (Input.GetKeyDown(interactKey))
                {
                    talkPromptText.gameObject.SetActive(false);

                    // freeze NPC movement
                    var agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (agent) agent.enabled = false;

                    /* ── add / enable SmoothLookAt ── */
                    SmoothLookAt look = npc.GetComponent<SmoothLookAt>();
                    if (!look) look = npc.gameObject.AddComponent<SmoothLookAt>();

                    look.target = playerCamera.transform;  // face the camera
                    look.rotateSpeed = 5f;                      // or any speed you like

                    // open the Q&A UI
                    dialogueUI.Open(npc.GetComponent<NPCDialogue>());
                }

                return;          //  ✅ keep prompt while still looking at same NPC
            }
        }

        // 2. Nothing in sight → hide prompt
        currentTarget = null;
        if (talkPromptText.gameObject.activeSelf)
            talkPromptText.gameObject.SetActive(false);
    }
}
