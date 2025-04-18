using UnityEngine;

public class IntelDetector : MonoBehaviour
{
    public Transform detector;        // Your detector gadget (visual model)
    public AudioSource beepSound;     // Beeping sound source
    public float detectionRange = 10f;
    public LayerMask intelLayer;      // Set your intel object layer here

    void Update()
    {
        Collider[] intelNearby = Physics.OverlapSphere(transform.position, detectionRange, intelLayer);

        if (intelNearby.Length > 0)
        {
            // Get closest intel
            float closestDistance = detectionRange;
            foreach (Collider intel in intelNearby)
            {
                float distance = Vector3.Distance(transform.position, intel.transform.position);
                if (distance < closestDistance)
                    closestDistance = distance;
            }

            // Adjust beep frequency
            beepSound.pitch = Mathf.Lerp(2f, 0.5f, closestDistance / detectionRange);
            if (!beepSound.isPlaying)
                beepSound.Play();
        }
        else
        {
            beepSound.Stop();
        }
    }
}
