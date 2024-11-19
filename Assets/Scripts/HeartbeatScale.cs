using UnityEngine;

public class HeartbeatScale : MonoBehaviour
{
    // Настройки для анимации биения сердца
    public float maxScale = 1.2f;  // Максимальный масштаб по оси X
    public float minScale = 1.0f;  // Минимальный масштаб по оси X
    public float beatSpeed = 6.0f; // Скорость биения
    public float beatPause = 0.5f; // Длительность паузы между сериями ударов
    public int beatsPerSeries = 2; // Количество ударов в одной серии

    // Внутренние переменные
    private Vector3 originalScale;
    private float beatTime;
    private int beatCount;
    private bool isPausing;

    void Start()
    {
        // Сохраняем оригинальный масштаб объекта
        originalScale = transform.localScale;
        beatTime = 0f;
        beatCount = 0;
        isPausing = false;
    }

    void Update()
    {
        if (isPausing)
        {
            // Поддерживаем масштаб в состоянии покоя
            transform.localScale = new Vector3(minScale, originalScale.y, originalScale.z);
            beatTime += Time.deltaTime;

            // Если прошло достаточно времени для паузы, снова включаем биение
            if (beatTime >= beatPause)
            {
                isPausing = false;
                beatTime = 0f;
            }
        }
        else
        {
            // Рассчитываем текущее время для анимации биения сердца
            beatTime += Time.deltaTime * beatSpeed;

            // Если объект бьется, рассчитываем новую величину масштаба по оси X
            float scaleX = Mathf.Lerp(minScale, maxScale, Mathf.Sin(beatTime) * 0.5f + 0.5f);

            // Обновляем масштаб объекта
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);

            // Если прошло достаточно времени для одного удара, увеличиваем счетчик ударов
            if (beatTime >= Mathf.PI)
            {
                beatCount++;
                beatTime = 0f;
            }

            // Если количество ударов достигло нужного значения, включаем паузу
            if (beatCount >= beatsPerSeries)
            {
                isPausing = true;
                beatTime = 0f;
                beatCount = 0;
            }
        }
    }
}