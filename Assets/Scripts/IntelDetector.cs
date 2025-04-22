using UnityEngine;

public class IntelDetector : MonoBehaviour
{
    public Transform detector;            // Optional visual model
    public AudioSource beepSound;         // Looping beep sound
    public float detectionRange = 10f;
    public LayerMask intelLayer;

    [HideInInspector] public float proximityStrength = 0f; // Optional UI use

    void Update()
    {
        Collider[] intelNearby = Physics.OverlapSphere(transform.position, detectionRange, intelLayer);

        float closestDistance = detectionRange;
        bool found = false;

        foreach (Collider intel in intelNearby)
        {
            IntelProp prop = intel.GetComponent<IntelProp>();
            if (prop != null && prop.intelType == IntelProp.IntelType.Paper)
            {
                float distance = Vector3.Distance(transform.position, intel.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    found = true;
                }
            }
        }

        if (found)
        {
            proximityStrength = 1f - Mathf.Clamp01(closestDistance / detectionRange);
            beepSound.pitch = Mathf.Lerp(0.4f, 1f, proximityStrength);
            beepSound.volume = Mathf.Lerp(0.2f, 1f, proximityStrength);

            if (!beepSound.isPlaying)
                beepSound.Play();
        }
        else
        {
            proximityStrength = 0f;
            beepSound.Stop();
        }
    }
}
