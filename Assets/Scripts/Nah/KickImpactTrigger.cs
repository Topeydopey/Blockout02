using UnityEngine;

public class KickImpactTrigger : MonoBehaviour
{
    [Tooltip("Drag a bowling-pins SFX clip here")]
    public AudioClip bowlingSFX;

    bool played;                    // prevent multiple sounds

    void OnCollisionEnter(Collision collision)
    {
        DesignerNPC otherNPC = collision.collider.GetComponent<DesignerNPC>();
        if (otherNPC != null && otherNPC != GetComponent<DesignerNPC>())
        {
            // ── ragdoll & shove ───────────────────────────────────────
            Rigidbody rb = otherNPC.GetComponent<Rigidbody>();
            UnityEngine.AI.NavMeshAgent agent = otherNPC.GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (agent) agent.enabled = false;

            if (rb)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                Vector3 dir = collision.impulse.normalized;
                rb.AddForce(dir * 5f, ForceMode.Impulse);
            }

            otherNPC.RecoverAfterDelay(3f);

            // ── play bowling sound once ──────────────────────────────
            if (!played && bowlingSFX)
            {
                Vector3 hitPoint = collision.contacts.Length > 0
                                 ? collision.contacts[0].point
                                 : transform.position;

                AudioSource.PlayClipAtPoint(bowlingSFX, hitPoint, 1f);
                played = true;
            }
        }

        Destroy(this);          // trigger only once
    }
}
