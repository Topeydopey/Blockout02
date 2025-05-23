using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public GameObject shipLightsParent;
    public CanvasGroup basementObjective;

    [Header("HUD")]
    public GameObject scoreHUD;          // drag the Score Text or its parent

    public float fadeTime = 1f;

    public void BeginAct2()
    {
        if (scoreHUD) scoreHUD.SetActive(false);  // ← hide the counter

        if (shipLightsParent) shipLightsParent.SetActive(false);

        basementObjective.alpha = 0;
        basementObjective.gameObject.SetActive(true);
        StartCoroutine(FadeInObjective());
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
