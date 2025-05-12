using UnityEngine;

public class ClockSecondHand : MonoBehaviour
{
    void Update()
    {
        float seconds = Time.time;
        float angle = (seconds % 60f) / 60f * 360f;

        // Keep Y = 90, X = 0, and only rotate Z
        transform.localRotation = Quaternion.Euler(0f, 90f, +angle);
    }
}
