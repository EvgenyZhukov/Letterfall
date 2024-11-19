using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private float moveSpeed = 1.5f;
    private float fadeDuration = 0.5f;

    public TMP_Text text;
    private Color originalColor;

    void Start()
    {
        originalColor = text.color;
        Destroy(gameObject, fadeDuration); // Уничтожаем объект после завершения анимации
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        float alpha = Mathf.Lerp(originalColor.a, 0, Time.deltaTime / fadeDuration);
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    public void SetText(int score)
    {
        text.text = score.ToString();
    }
}