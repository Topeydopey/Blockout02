using UnityEngine;
using System.Collections;
public class Kick : MonoBehaviour
{
    [Header("Kick Settings")]
    public float kickRange = 2f;
    public float kickForce = 15f;
    public KeyCode kickKey = KeyCode.F;

    [Header("Optional Effects")]
    public AudioSource kickSound;
    public ParticleSystem kickEffect;

    void Update()
    {
        if (Input.GetKeyDown(kickKey))
        {
            AttemptKick();
        }
    }
    public void EnableRagdoll(DesignerNPC npc)
    {
        Rigidbody rb = npc.GetComponent<Rigidbody>();
        UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent != null)
            agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public IEnumerator ReenableAgentAfterDelay(UnityEngine.AI.NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (agent != null)
            agent.enabled = true;
    }

    void AttemptKick()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, kickRange))
        {
            DesignerNPC npc = hit.collider.GetComponent<DesignerNPC>();
            if (npc != null)
            {
                Rigidbody rb = npc.GetComponent<Rigidbody>();
                UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (agent != null)
                    agent.enabled = false;

                if (rb != null)
                {
                    // Enable ragdoll mode
                    rb.isKinematic = false;
                    rb.useGravity = true;

                    // Kick them
                    Vector3 direction = (hit.point - transform.position).normalized;
                    rb.AddForce(direction * kickForce, ForceMode.Impulse);
                    // Recover after 3 seconds
                    npc.RecoverAfterDelay(3f);
                }

                // OPTIONAL: enable them to knock over others on contact
                npc.gameObject.AddComponent<KickImpactTrigger>();
            }
        }
    }
}
