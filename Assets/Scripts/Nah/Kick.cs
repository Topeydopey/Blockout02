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
                // ─── Ragdoll & force ─────────────────────────────────────────────
                Rigidbody rb = npc.GetComponent<Rigidbody>();
                var agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (agent) agent.enabled = false;

                if (rb)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;

                    Vector3 dir = (hit.point - transform.position).normalized;
                    rb.AddForce(dir * kickForce, ForceMode.Impulse);

                    npc.RecoverAfterDelay(3f);
                }

                // make knocked NPC knock down others
                npc.gameObject.AddComponent<KickImpactTrigger>();

                // ───---  EFFECTS  ---─────────────────────────────────────────────
                // 1. sound
                if (kickSound) kickSound.Play();

                // 2. particle at impact point
                if (kickEffect)
                {
                    Vector3 spawnPos = hit.point + Vector3.up * 0.20f;   // lift a bit off floor
                    Instantiate(kickEffect, spawnPos, Quaternion.identity)
                        .Play();                                         // make sure it starts
                }
            }
        }
    }
}
