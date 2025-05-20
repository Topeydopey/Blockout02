using UnityEngine;
using System.Collections;

public class DesignerNPC : MonoBehaviour
{
    public bool isAlien;          //  still referenced by Shotgun / ScoreManager

    /* ─────────────────────────  Ragdoll recovery  ───────────────────────── */

    public void RecoverAfterDelay(float delay)
    {
        StartCoroutine(Recover(delay));
    }

    IEnumerator Recover(float delay)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody rb = GetComponent<Rigidbody>();
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }

        if (agent) agent.enabled = true;
    }
}
