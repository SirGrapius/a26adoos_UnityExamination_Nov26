using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Fade(Color startColor, Color targetColor, float duration)
    {
        renderer.color = Color.Lerp(startColor, targetColor, duration);
        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeIn(float duration)
    {
        Color startColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
        Color targetColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        StartCoroutine(Fade(startColor, targetColor, duration));
        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeOut(float duration)
    {
        Color startColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        Color targetColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
        StartCoroutine(Fade(startColor, targetColor, duration));
        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeInAndOut(float duration)
    {
        StartCoroutine(FadeOut(duration/2));
        yield return new WaitForSeconds(duration/2);
        StartCoroutine(FadeIn(duration/2));
        yield return new WaitForSeconds(duration/2);
    }
}
