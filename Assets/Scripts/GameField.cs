using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameField : MonoBehaviour
{
    public AudioControllerScript audioControllerScript;
    public LetterBoxSpawner letterBoxSpawner;

    public TextLine textLine;
    public ControllerUI controllerUI;
    public BlinkingController blinkingController;
    public int[] objectsPerLine; // ������ ��� ����� ���������� �������� �� ������ �����
    public int[] letterCounts; // ������ ��� ����� ���������� ������ �����

    public bool defeat = false;

    // ���������� ������������ ���������� ���� ��� ������� ����
    public Dictionary<char, int> maxCounts = new Dictionary<char, int>()
    {
        {'�', 3}, {'�', 2}, {'�', 2}, {'�', 2}, {'�', 2}, {'�', 3}, {'�', 2}, {'�', 2},
        {'�', 3}, {'�', 1}, {'�', 2}, {'�', 2}, {'�', 2}, {'�', 3}, {'�', 3}, {'�', 2},
        {'�', 2}, {'�', 2}, {'�', 2}, {'�', 2}, {'�', 2}, {'�', 2}, {'�', 1}, {'�', 2},
        {'�', 2}, {'�', 1}, {'�', 1}, {'�', 2}, {'�', 1}, {'�', 1}, {'�', 1}, {'�', 2}
    };

    void Start()
    {

        // ������������� ��������
        objectsPerLine = new int[7]; // �����������, ��� � ��� 7 �����
        letterCounts = new int[32]; // ��� 32 ���� �������� �������� (32 � �� 33, ��� ��� ������ ����� "�" ������������ ����� "�")

        if (!PlayerPrefsMethods.GetGameStarted())
        {
            textLine.ShowText(0);
        }

        //InvokeRepeating("CheckLines", 2f, 1f);
    }
    // ����� ��� ���������� ���������� � ��������� ������� �� ��������� �����
    public void AddObjectToLine(int lineIndex)
    {
        if (lineIndex >= 0 && lineIndex < objectsPerLine.Length)
        {
            objectsPerLine[lineIndex]++;
        }
    }
    // ����� ��� ��������� ���������� �������� �� ��������� �����
    public int GetObjectsCountOnLine(int lineIndex)
    {
        if (lineIndex >= 0 && lineIndex < objectsPerLine.Length)
        {
            return objectsPerLine[lineIndex];
        }
        return 0;
    }
    // ����� ��� ���������� ���������� � ����� � ������
    public void AddLetter(char letter)
    {
        int index = (int)letter - 1040; // ASCII-��� ����� "�" - 1040
        if (index >= 0 && index < letterCounts.Length)
        {
            letterCounts[index]++;
        }
    }
    // ����� ��� �������� ���������� � ����� �� �������
    public void RemoveLetter(char letter)
    {
        int index = (int)letter - 1040; // ASCII-��� ����� "�" - 1040
        if (index >= 0 && index < letterCounts.Length && letterCounts[index] > 0)
        {
            letterCounts[index]--;
        }
    }
    // ����� ��� ��������� ���������� ����� �� �� �������
    public int GetLetterCount(char letter)
    {
        int index = (int)letter - 1040; // ASCII-��� ����� "�" - 1040
        if (index >= 0 && index < letterCounts.Length)
        {
            return letterCounts[index];
        }
        return 0;
    }
    // ����� ��� ��������� ������ ���������� �������� �� ���� ������� ����
    public int GetTotalObjectsCount()
    {
        int totalObjectsCount = 0;
        foreach (int count in objectsPerLine)
        {
            totalObjectsCount += count;
        }
        return totalObjectsCount;
    }
    // ����� �������� ���������� ����� � ��������� �������������� � ���������
    public void CheckLines()
    {
        if (!defeat)
        {
        for (int i = 0; i < objectsPerLine.Length; i++)
        {
            int objectsCount = objectsPerLine[i];

            // ��������� ���������� �������� �� ����� � ���������� ��������������� �������������� � ���������
            if (objectsCount > 8)
            {
                // ���������
                //Debug.Log("��������� ������������ ���������� �������� �� ����� " + (i + 1) + ". ���� ��������.");
                //controllerUI.panelLockUI.SetActive(false);

                //letterBoxSpawner.spawning = false;

                textLine.ShowText(9);

                defeat = true;
                controllerUI.greenButton.interactable = false;

                Invoke("Defeat", 1.5f);

                //blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.None);
            }
            else if (objectsCount > 7)
            {
                // ������� ��������� 3
                //Debug.Log("��������� ������������ ���������� �������� �� ����� " + (i + 1) + ". ������� ��������� 3.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.Fast);
                // �������������� ��������, ��������� � ������� ��������� 3
            }
            else if (objectsCount > 6)
            {
                // ������� ��������� 2
                //Debug.Log("��������� ������������ ���������� �������� �� ����� " + (i + 1) + ". ������� ��������� 2.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.Medium);
                // �������������� ��������, ��������� � ������� ��������� 2
            }
            else if (objectsCount > 5)
            {
                // ������� ��������� 1
                //Debug.Log("��������� ������������ ���������� �������� �� ����� " + (i + 1) + ". ������� ��������� 1.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.Slow);
                // �������������� ��������, ��������� � ������� ��������� 1
            }
            else if (objectsCount <= 5)
            {
                // ������� ��������� 0
                //Debug.Log("�� ��������� ������������ ���������� �������� �� ����� " + (i + 1) + ". ������� ��������� 0.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.None);
                // �������������� ��������, ��������� � ������� ��������� 0
            }
            }
        }
    }
    void Defeat()
    {

        controllerUI.panelLockUI.SetActive(true);

        audioControllerScript.soundDefeat.Play();

        PlayerPrefsMethods.SetGameStarted(false);

        for (int i = 0; i < objectsPerLine.Length; i++)
        {
            blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.None);
        }

        if (PlayerPrefsMethods.GetSecondChance())
        {
            controllerUI.defeatScreen.SetActive(true);
            controllerUI.buttonSecondChance.SetActive(false);
            //PlayerPrefsMethods.SetSecondChance(false);
        }
        else
        {
            controllerUI.defeatScreen.SetActive(true);
            controllerUI.buttonSecondChance.SetActive(true);
        }

        //PlayerPrefsMethods.SaveGame();
    }
    public void ClearField()
    {
        GameObject[] letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox");

        for (int i = 0; i < letterBoxes.Length; i++)
        {
            Destroy(letterBoxes[i]);    // ���������� ������ ����������� ������� Unity
        }

        for (int i = 0; i < objectsPerLine.Length; i++)
        {
            objectsPerLine[i] = 0;
            blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.None);
        }

        for (int i = 0; i < letterCounts.Length; i++)
        {
            letterCounts[i] = 0;
        }
    }
}