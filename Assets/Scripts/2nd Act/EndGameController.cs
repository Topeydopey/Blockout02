using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public GameObject shipLightsParent;     // drag “ShipLights”
    public CanvasGroup basementObjective;   // UI text/arrow
    public float fadeTime = 1f;

    /* called from ScoreManager event */
    public void BeginAct2()
    {
        basementObjective.alpha = 0;
        basementObjective.gameObject.SetActive(true);
        StartCoroutine(FadeInObjective());

        // black-out
        if (shipLightsParent) shipLightsParent.SetActive(false);
    }

    System.Collections.IEnumerator FadeInObjective()
    {
        float t = 0;
        while (t < fadeTime)
        {
            basementObjective.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        basementObjective.alpha = 1;
    }
}
