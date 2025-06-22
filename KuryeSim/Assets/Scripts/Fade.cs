using UnityEngine;
using System.Collections;
public class Fade : MonoBehaviour
{
    public float fadeDuration = 2f;  // yok olma süresi
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    public void StartFading()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / fadeDuration);
            Color newColor = originalColor;
            newColor.a = alpha;
            rend.material.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}
