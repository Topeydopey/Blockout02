using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 5f;
    public float yOffset = 90f;              // ← new

    void Update()
    {
        if (!target) return;

        Vector3 flatDir = target.position - transform.position;
        flatDir.y = 0f;

        if (flatDir.sqrMagnitude > 0.0001f)
        {
            Quaternion tgt = Quaternion.LookRotation(flatDir) * Quaternion.Euler(0, yOffset, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, tgt, rotateSpeed * Time.deltaTime);
        }
    }
}
