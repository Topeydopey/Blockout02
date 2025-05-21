using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class AmbienceZone : MonoBehaviour
{
    public AudioSource seaSource;        // drag SeaAmbience
    public AudioSource interiorSource;   // drag InteriorAmbience
    public float fadeTime = 1.5f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopAllCoroutines();
        StartCoroutine(Fade(seaSource, 0.3f));      // lower sea to 30 %
        StartCoroutine(Fade(interiorSource, 0.9f)); // raise music to 90 %
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopAllCoroutines();
        StartCoroutine(Fade(seaSource, 0.7f));       // sea back up
        StartCoroutine(Fade(interiorSource, 0.0f));  // music out
    }

    IEnumerator Fade(AudioSource src, float targetVol)
    {
        float start = src.volume;
        float t = 0f;
        while (t < fadeTime)
        {
            src.volume = Mathf.Lerp(start, targetVol, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        src.volume = targetVol;
    }
}
