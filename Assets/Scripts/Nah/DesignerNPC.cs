using UnityEngine;
using System.Collections;
public class DesignerNPC : MonoBehaviour
{
    public bool isAlien;

    [TextArea(2, 4)]
    public string[] humanResponses;
    [TextArea(2, 4)]
    public string[] alienResponses;

    private string chosenLine;

    void Start()
    {
        string[] source = isAlien ? alienResponses : humanResponses;

        if (source.Length > 0)
            chosenLine = source[Random.Range(0, source.Length)];
        else
            chosenLine = "…";
    }
    public void RecoverAfterDelay(float delay)
    {
        StartCoroutine(RecoverCoroutine(delay));
    }

    private IEnumerator RecoverCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody rb = GetComponent<Rigidbody>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); // flatten
        }

        if (agent != null)
        {
            agent.enabled = true;
        }
    }

    public string GetResponse()
    {
        return chosenLine;
    }
}
