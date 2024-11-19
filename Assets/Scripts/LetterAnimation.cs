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

    public bool shrinkAndDestroy = false; // ����������, ������������ ������� ���������� � ����������� �������
    private float shrinkSpeed = 2f; // �������� ���������� �������

    // ����������������� ������
    private float shakeDuration = 0.3f;
    // ���� ������
    private float shakeAngle = 10.0f;
    // ��������� �������� �������
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
            // ��������� ������ �������
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

            // ���������, ���� �� ������ ����� ���
            if (transform.localScale.x <= destroySize || transform.localScale.y <= destroySize || transform.localScale.z <= destroySize)
            {
                // ���������� ������
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
            // ���������� ��������� �������� ����
            float angle = Mathf.Sin(Time.time * 20) * shakeAngle; // ��������� ��� �������� �������

            // ��������� �������� � �������� �������
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, angle);

            // ����������� �����
            elapsed += Time.deltaTime;

            // ������� ���������� ��������� � ������� ��������
            //Debug.Log("Elapsed time: " + elapsed);

            // ���� ��������� ����
            yield return null;
        }

        // ��������������� ��������� ��������
        transform.localRotation = initialRotation;
        //Debug.Log("Shake ended");

        // ������������� ��������
        yield break;
    }
}