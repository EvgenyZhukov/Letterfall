using System.Collections;
using UnityEngine;

public class ForEditor : MonoBehaviour
{
    // Публичное поле для настройки времени задержки из редактора
    public float delay = 1.0f;

    void Start()
    {
        // Запуск корутины, которая остановит время
        StartCoroutine(StopTimeAfterDelay());
    }

    private IEnumerator StopTimeAfterDelay()
    {
        // Ожидание заданного времени
        yield return new WaitForSeconds(delay);

        // Остановка времени
        Time.timeScale = 0;
    }
}