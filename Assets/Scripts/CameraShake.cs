using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Default Shake Settings")]
    public float defaultDuration = 0.2f;
    public float defaultMagnitude = 0.1f;

    private float shakeTimer = 0f;
    private float currentMagnitude = 0f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * currentMagnitude;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        // Use default values if none provided
        if (duration <= 0f) duration = defaultDuration;
        if (magnitude <= 0f) magnitude = defaultMagnitude;

        shakeTimer = duration;
        currentMagnitude = magnitude;
    }
}
