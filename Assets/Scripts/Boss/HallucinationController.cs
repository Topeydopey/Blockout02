using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class HallucinationController : MonoBehaviour
{
    public RawImage overlay;          // full-screen RawImage
    public VideoClip clip;            // 3-sec meme video
    public float fadeTime = 0.4f;

    VideoPlayer vp;
    CanvasGroup cg;

    void Awake()
    {
        cg = overlay.gameObject.AddComponent<CanvasGroup>();
        vp = overlay.gameObject.AddComponent<VideoPlayer>();
        vp.playOnAwake = false;
        vp.isLooping = false;
        vp.clip = clip;
        vp.renderMode = VideoRenderMode.APIOnly;
        vp.targetTexture = new RenderTexture(1280, 720, 0);
        overlay.texture = vp.targetTexture;
        cg.alpha = 0f;
    }

    public void PlayHallucination()
    {
        StartCoroutine(Hallucinate());
    }

    IEnumerator Hallucinate()
    {
        vp.Stop();
        vp.Play();

        float t = 0;
        while (t < fadeTime)
        {
            cg.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1;

        yield return new WaitForSeconds((float)vp.clip.length - fadeTime * 2);

        t = 0;
        while (t < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0;
    }
}
