using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 10f;
    public LayerMask intelLayer;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.blue);

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name} on layer '{LayerMask.LayerToName(hit.collider.gameObject.layer)}'");

                if (((1 << hit.collider.gameObject.layer) & intelLayer) != 0)
                {
                    IntelProp intel = hit.collider.GetComponent<IntelProp>();
                    if (intel != null)
                    {
                        intel.Interact();
                        Debug.Log("Intel Interaction Triggered Successfully!");
                    }
                    else
                    {
                        Debug.Log("No IntelProp component found!");
                    }
                }
                else
                {
                    Debug.Log($"Object '{hit.collider.gameObject.name}' is not on the Intel layer!");
                }
            }
            else
            {
                Debug.Log("Raycast hit nothing at all.");
            }
        }
    }
}
