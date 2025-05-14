using UnityEngine;

public class KickImpactTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        DesignerNPC otherNPC = collision.collider.GetComponent<DesignerNPC>();
        if (otherNPC != null && otherNPC != this.GetComponent<DesignerNPC>())
        {
            Rigidbody rb = otherNPC.GetComponent<Rigidbody>();
            UnityEngine.AI.NavMeshAgent agent = otherNPC.GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (agent != null)
                agent.enabled = false;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                Vector3 forceDir = collision.impulse.normalized;
                rb.AddForce(forceDir * 5f, ForceMode.Impulse);
            }

            // Start recovery
            otherNPC.RecoverAfterDelay(3f);
        }

        // Only trigger once
        Destroy(this);
    }
}
