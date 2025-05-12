using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float waitTime = 2f;
    public float moveCooldown = 5f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = moveCooldown;
        SetNewDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (timer >= moveCooldown)
            {
                SetNewDestination();
                timer = 0f;
            }
        }
    }

    void SetNewDestination()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
        agent.SetDestination(newPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;
        randDirection += origin;

        if (NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, NavMesh.AllAreas))
        {
            return navHit.position;
        }

        return origin;
    }
}
