using System.Collections;
using UnityEngine;

public class LetterAnimation : MonoBehaviour
{
    public GameObject objectToAnimate;
    private float minScale = 0.75f;
    private float maxScale = 1.0f;
    private float animationDuration = 0.2f;
    private float destroySize = 0.1f;

    public bool isAnimating = false;
    public bool isShaking = false;
    private float timer = 0.0f;
    private Vector3 initialScale;

    public bool shrinkAndDestroy = false; // Переменная, активирующая процесс уменьшения и уничтожения объекта
    private float shrinkSpeed = 2f; // Скорость уменьшения объекта

    // Продолжительность тряски
    private float shakeDuration = 0.3f;
    // Угол тряски
    private float shakeAngle = 10.0f;
    // Начальное вращение объекта
    private Quaternion initialRotation;

    void Start()
    {
        initialScale = objectToAnimate.transform.localScale;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        SelectAnimation();
        ShrinkAndDestroy();
    }

    void SelectAnimation()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            objectToAnimate.transform.localScale = initialScale * scale;

            if (timer >= animationDuration)
            {
                isAnimating = false;
                timer = 0.0f;
            }
        }
    }

    void ShrinkAndDestroy()
    {
        if (shrinkAndDestroy)
        {
            // Уменьшаем размер объекта
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

            // Проверяем, стал ли объект очень мал
            if (transform.localScale.x <= destroySize || transform.localScale.y <= destroySize || transform.localScale.z <= destroySize)
            {
                // Уничтожаем объект
                shrinkAndDestroy = false;
                Destroy(gameObject);
            }
        }
    }

    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;
        //Debug.Log("Shake started");

        while (elapsed < shakeDuration)
        {
            // Генерируем случайное смещение угла
            float angle = Mathf.Sin(Time.time * 20) * shakeAngle; // Синусоида для плавного эффекта

            // Применяем смещение к вращению объекта
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, angle);

            // Увеличиваем время
            elapsed += Time.deltaTime;

            // Выводим отладочное сообщение с текущим временем
            //Debug.Log("Elapsed time: " + elapsed);

            // Ждем следующий кадр
            yield return null;
        }

        // Восстанавливаем начальное вращение
        transform.localRotation = initialRotation;
        //Debug.Log("Shake ended");

        // Останавливаем корутину
        yield break;
    }
}