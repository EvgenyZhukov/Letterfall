using System.Collections;
using UnityEngine;

public class ForEditor : MonoBehaviour
{
    // ��������� ���� ��� ��������� ������� �������� �� ���������
    public float delay = 1.0f;

    void Start()
    {
        // ������ ��������, ������� ��������� �����
        StartCoroutine(StopTimeAfterDelay());
    }

    private IEnumerator StopTimeAfterDelay()
    {
        // �������� ��������� �������
        yield return new WaitForSeconds(delay);

        // ��������� �������
        Time.timeScale = 0;
    }
}