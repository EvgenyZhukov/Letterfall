using UnityEngine;
using System.Collections;

public class TextAnimator : MonoBehaviour
{
    public AudioControllerScript audioControllerScript;

    public GameObject wordLine;
    public GameObject messageLine;
    public bool isAnimating = false;
    private bool secondPhase = false;
    private float scaleChangeSpeed = 12f; // �������� ������������ ��������
    private float textDelayTime = 1.2f; // ����� �������� �������


    void Update()
    {
        // ���������, ����� �� ������ ��������
        if (isAnimating)
        {
            if (!secondPhase)
            {
                StartCoroutine(AnimateTextLine());
            }
            else
            {
                StartCoroutine(ReverseTextLine());
            }

            audioControllerScript.soundTextChange.Play();

            isAnimating = false;
        }
    }

    IEnumerator AnimateTextLine()
    {
        // �������� ��� ������� ������� (���������� ��������)
        for (float scale = 1f; scale >= 0f; scale -= scaleChangeSpeed * Time.deltaTime)
        {
            wordLine.transform.localScale = new Vector3(1f, scale, 1f);
            yield return null;
        }
        wordLine.transform.localScale = new Vector3(1f, 0, 1f);

        // ��������� ������ ������ � �������� ������
        //wordLine.SetActive(false);
        //messageLine.SetActive(true);

        // �������� ��� ������� ������� (���������� ��������)
        for (float scale = 0f; scale <= 1f; scale += scaleChangeSpeed * Time.deltaTime)
        {
            messageLine.transform.localScale = new Vector3(1f, scale, 1f);
            yield return null;
        }
        messageLine.transform.localScale = new Vector3(1f, 1, 1f);

        secondPhase = true;
        Invoke("ReverseActivator", textDelayTime);
    }

    IEnumerator ReverseTextLine()
    {
        // �������� ��� ������� ������� (���������� ��������)
        for (float scale = 1f; scale >= 0f; scale -= scaleChangeSpeed * Time.deltaTime)
        {
            messageLine.transform.localScale = new Vector3(1f, scale, 1f);
            yield return null;
        }
        messageLine.transform.localScale = new Vector3(1f, 0, 1f);

        // ��������� ������ ������ � �������� ������
        //messageLine.SetActive(false);
        //wordLine.SetActive(true);

        // �������� ��� ������� ������� (���������� ��������)
        for (float scale = 0f; scale <= 1f; scale += scaleChangeSpeed * Time.deltaTime)
        {
            wordLine.transform.localScale = new Vector3(1f, scale, 1f);
            yield return null;
        }
        wordLine.transform.localScale = new Vector3(1f, 1, 1f);

        secondPhase = false;
    }

    void ReverseActivator()
    {
        isAnimating = true;
    }
}