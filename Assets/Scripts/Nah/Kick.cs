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
    public AudioClip bowlingSFX;     // drag your “pins” clip here

    void Update()
    {
        if (Input.GetKeyDown(kickKey))
            AttemptKick();
    }

    /* ─────────────────────────── helpers (unchanged) ───────────────────────── */

    public void EnableRagdoll(DesignerNPC npc)
    {
        var rb = npc.GetComponent<Rigidbody>();
        var agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent) agent.enabled = false;
        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public IEnumerator ReenableAgentAfterDelay(UnityEngine.AI.NavMeshAgent ag, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (ag) ag.enabled = true;
    }

    /* ─────────────────────────────── main kick ─────────────────────────────── */

    void AttemptKick()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, kickRange))
        {
            DesignerNPC npc = hit.collider.GetComponent<DesignerNPC>();
            if (npc == null) return;

            // ragdoll + shove
            var rb = npc.GetComponent<Rigidbody>();
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

            /* ── add trigger & hand it the SFX clip ─────────────────── */
            var trig = npc.gameObject.AddComponent<KickImpactTrigger>();
            trig.bowlingSFX = bowlingSFX;          // << pass the clip

            /* ── local effects ──────────────────────────────────────── */
            if (kickSound) kickSound.Play();

            if (kickEffect)
            {
                Vector3 spawnPos = hit.point + Vector3.up * 0.20f;
                Instantiate(kickEffect, spawnPos, Quaternion.identity).Play();
            }
        }
    }
}
