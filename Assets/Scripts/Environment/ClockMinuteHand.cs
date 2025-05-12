using UnityEngine;

public class ClockMinuteHand : MonoBehaviour
{
    public bool useRealTime = false; // Toggle between real-time and in-game time

    void Update()
    {
        float minutes;

        if (useRealTime)
        {
            System.DateTime now = System.DateTime.Now;
            minutes = now.Minute + (now.Second / 60f);
        }
        else
        {
            minutes = (Time.time / 60f) % 60f; // In-game time passed
        }

        float angle = minutes / 60f * 360f;

        // Apply rotation: only rotate Z, preserve any base orientation (e.g., Y = 90)
        transform.localRotation = Quaternion.Euler(0f, 90f, +angle);
    }
}
