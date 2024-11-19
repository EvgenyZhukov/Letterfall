using UnityEngine;

public class HeartbeatScale : MonoBehaviour
{
    // ��������� ��� �������� ������ ������
    public float maxScale = 1.2f;  // ������������ ������� �� ��� X
    public float minScale = 1.0f;  // ����������� ������� �� ��� X
    public float beatSpeed = 6.0f; // �������� ������
    public float beatPause = 0.5f; // ������������ ����� ����� ������� ������
    public int beatsPerSeries = 2; // ���������� ������ � ����� �����

    // ���������� ����������
    private Vector3 originalScale;
    private float beatTime;
    private int beatCount;
    private bool isPausing;

    void Start()
    {
        // ��������� ������������ ������� �������
        originalScale = transform.localScale;
        beatTime = 0f;
        beatCount = 0;
        isPausing = false;
    }

    void Update()
    {
        if (isPausing)
        {
            // ������������ ������� � ��������� �����
            transform.localScale = new Vector3(minScale, originalScale.y, originalScale.z);
            beatTime += Time.deltaTime;

            // ���� ������ ���������� ������� ��� �����, ����� �������� ������
            if (beatTime >= beatPause)
            {
                isPausing = false;
                beatTime = 0f;
            }
        }
        else
        {
            // ������������ ������� ����� ��� �������� ������ ������
            beatTime += Time.deltaTime * beatSpeed;

            // ���� ������ ������, ������������ ����� �������� �������� �� ��� X
            float scaleX = Mathf.Lerp(minScale, maxScale, Mathf.Sin(beatTime) * 0.5f + 0.5f);

            // ��������� ������� �������
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);

            // ���� ������ ���������� ������� ��� ������ �����, ����������� ������� ������
            if (beatTime >= Mathf.PI)
            {
                beatCount++;
                beatTime = 0f;
            }

            // ���� ���������� ������ �������� ������� ��������, �������� �����
            if (beatCount >= beatsPerSeries)
            {
                isPausing = true;
                beatTime = 0f;
                beatCount = 0;
            }
        }
    }
}