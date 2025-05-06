using UnityEngine;
using UnityEngine.UI;

public class IntelDetector : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask intelLayer;
    public Text intelCounterText;

    private int intelCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, intelLayer))
            {
                IntelProp intel = hit.collider.GetComponent<IntelProp>();
                if (intel != null)
                {
                    intelCount++;
                    UpdateUI();
                    Destroy(hit.collider.gameObject);
                    // Optional: Play sound or feedback
                }
            }
        }
    }

    void UpdateUI()
    {
        intelCounterText.text = "Intel Collected: " + intelCount;
    }
}
