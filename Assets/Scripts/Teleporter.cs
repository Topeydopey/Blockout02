using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Where the player will be moved to")]
    public Transform destination;

    void OnTriggerEnter(Collider other)
    {
        // Only teleport the player
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                // Move the CharacterController
                cc.enabled = false;                        // disable so we can teleport cleanly
                other.transform.position = destination.position;
                cc.enabled = true;                         // re-enable so physics continue
            }
            else
            {
                // Fallback: just teleport the transform
                other.transform.position = destination.position;
            }
        }
    }
}
