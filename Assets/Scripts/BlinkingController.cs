using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingController : MonoBehaviour
{
    public Image[] images; // Массив объектов с компонентом Image
    public float slowBlinkInterval = 1.0f;
    public float mediumBlinkInterval = 0.5f;
    public float fastBlinkInterval = 0.25f;

    private Coroutine[] blinkingCoroutines;

    void Start()
    {
        blinkingCoroutines = new Coroutine[images.Length];
        // Установим начальную прозрачность всех изображений на 0
        foreach (Image img in images)
        {
            SetImageAlpha(img, 0.0f);
        }
    }

    public void SetBlinkMode(int index, BlinkMode mode)
    {
        if (index < 0 || index >= images.Length) return;

        if (blinkingCoroutines[index] != null)
        {
            StopCoroutine(blinkingCoroutines[index]);
        }

        switch (mode)
        {
            case BlinkMode.Slow:
                blinkingCoroutines[index] = StartCoroutine(BlinkImage(index, slowBlinkInterval));
                break;
            case BlinkMode.Medium:
                blinkingCoroutines[index] = StartCoroutine(BlinkImage(index, mediumBlinkInterval));
                break;
            case BlinkMode.Fast:
                blinkingCoroutines[index] = StartCoroutine(BlinkImage(index, fastBlinkInterval));
                break;
            case BlinkMode.None:
                blinkingCoroutines[index] = StartCoroutine(FadeToAlpha(index, 0.0f, 0.5f)); // Плавное исчезновение
                break;
        }
    }

    private IEnumerator BlinkImage(int index, float interval)
    {
        Image img = images[index];
        while (true)
        {
            yield return StartCoroutine(FadeToAlpha(index, 1.0f, interval / 2));
            yield return StartCoroutine(FadeToAlpha(index, 0.0f, interval / 2));
        }
    }

    private IEnumerator FadeToAlpha(int index, float targetAlpha, float duration)
    {
        Image img = images[index];
        float startAlpha = img.color.a;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            SetImageAlpha(img, alpha);
            yield return null;
        }
        SetImageAlpha(img, targetAlpha); // Убедитесь, что значение установлено точно
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

    public enum BlinkMode
    {
        None,
        Slow,
        Medium,
        Fast
    }
}