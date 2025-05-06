using UnityEngine;
using TMPro;

public class IntelDetector : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask intelLayer;
    public Camera playerCamera;

    public TextMeshProUGUI intelCounterText;
    public TextMeshProUGUI interactPromptText;

    private int intelCount = 0;
    private IntelProp currentIntel = null;
    public AudioSource audioSource;
    public AudioClip pickupClip;


    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, intelLayer))
        {
            IntelProp intel = hit.collider.GetComponent<IntelProp>();
            if (intel != null)
            {
                currentIntel = intel;
                if (interactPromptText != null && !interactPromptText.gameObject.activeSelf)
                    interactPromptText.gameObject.SetActive(true);

                if (Input.GetKeyDown(interactKey))
                {
                    intelCount++;
                    UpdateUI();
                    Destroy(hit.collider.gameObject);
                    currentIntel = null;
                    interactPromptText.gameObject.SetActive(false);

                    if (pickupClip != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(pickupClip);
                    }
                }
            }
        }
        else
        {
            if (currentIntel != null)
            {
                currentIntel = null;
            }

            if (interactPromptText != null && interactPromptText.gameObject.activeSelf)
                interactPromptText.gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (intelCounterText != null)
            intelCounterText.text = "Intel Collected: " + intelCount;
    }
}
