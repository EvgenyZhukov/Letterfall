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
    public int[] objectsPerLine; // Массив для учета количества объектов на каждой линии
    public int[] letterCounts; // Массив для учета количества каждой буквы

    public bool defeat = false;

    // Определяем максимальное количество букв для каждого типа
    public Dictionary<char, int> maxCounts = new Dictionary<char, int>()
    {
        {'А', 3}, {'Б', 2}, {'В', 2}, {'Г', 2}, {'Д', 2}, {'Е', 3}, {'Ж', 2}, {'З', 2},
        {'И', 3}, {'Й', 1}, {'К', 2}, {'Л', 2}, {'М', 2}, {'Н', 3}, {'О', 3}, {'П', 2},
        {'Р', 2}, {'С', 2}, {'Т', 2}, {'У', 2}, {'Ф', 2}, {'Х', 2}, {'Ц', 1}, {'Ч', 2},
        {'Ш', 2}, {'Щ', 1}, {'Ь', 1}, {'Ы', 2}, {'Ъ', 1}, {'Э', 1}, {'Ю', 1}, {'Я', 2}
    };

    void Start()
    {

        // Инициализация массивов
        objectsPerLine = new int[7]; // Предположим, что у вас 7 линий
        letterCounts = new int[32]; // Для 32 букв русского алфавита (32 а не 33, так как вместо буквы "Ё" используется буква "Е")

        if (!PlayerPrefsMethods.GetGameStarted())
        {
            textLine.ShowText(0);
        }

        //InvokeRepeating("CheckLines", 2f, 1f);
    }
    // Метод для добавления информации о созданном объекте на указанной линии
    public void AddObjectToLine(int lineIndex)
    {
        if (lineIndex >= 0 && lineIndex < objectsPerLine.Length)
        {
            objectsPerLine[lineIndex]++;
        }
    }
    // Метод для получения количества объектов на указанной линии
    public int GetObjectsCountOnLine(int lineIndex)
    {
        if (lineIndex >= 0 && lineIndex < objectsPerLine.Length)
        {
            return objectsPerLine[lineIndex];
        }
        return 0;
    }
    // Метод для добавления информации о букве в массив
    public void AddLetter(char letter)
    {
        int index = (int)letter - 1040; // ASCII-код буквы "А" - 1040
        if (index >= 0 && index < letterCounts.Length)
        {
            letterCounts[index]++;
        }
    }
    // Метод для удаления информации о букве из массива
    public void RemoveLetter(char letter)
    {
        int index = (int)letter - 1040; // ASCII-код буквы "А" - 1040
        if (index >= 0 && index < letterCounts.Length && letterCounts[index] > 0)
        {
            letterCounts[index]--;
        }
    }
    // Метод для получения количества буквы по ее символу
    public int GetLetterCount(char letter)
    {
        int index = (int)letter - 1040; // ASCII-код буквы "А" - 1040
        if (index >= 0 && index < letterCounts.Length)
        {
            return letterCounts[index];
        }
        return 0;
    }
    // Метод для получения общего количества объектов на всем игровом поле
    public int GetTotalObjectsCount()
    {
        int totalObjectsCount = 0;
        foreach (int count in objectsPerLine)
        {
            totalObjectsCount += count;
        }
        return totalObjectsCount;
    }
    // Метод проверки наполнения линии и активации предупреждений о опасности
    public void CheckLines()
    {
        if (!defeat)
        {
        for (int i = 0; i < objectsPerLine.Length; i++)
        {
            int objectsCount = objectsPerLine[i];

            // Проверяем количество объектов на линии и активируем соответствующие предупреждения о опасности
            if (objectsCount > 8)
            {
                // Проигрышь
                //Debug.Log("Превышено максимальное количество объектов на линии " + (i + 1) + ". Игра окончена.");
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
                // Уровень опасности 3
                //Debug.Log("Превышено максимальное количество объектов на линии " + (i + 1) + ". Уровень опасности 3.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.Fast);
                // Дополнительные действия, связанные с уровнем опасности 3
            }
            else if (objectsCount > 6)
            {
                // Уровень опасности 2
                //Debug.Log("Превышено максимальное количество объектов на линии " + (i + 1) + ". Уровень опасности 2.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.Medium);
                // Дополнительные действия, связанные с уровнем опасности 2
            }
            else if (objectsCount > 5)
            {
                // Уровень опасности 1
                //Debug.Log("Превышено максимальное количество объектов на линии " + (i + 1) + ". Уровень опасности 1.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.Slow);
                // Дополнительные действия, связанные с уровнем опасности 1
            }
            else if (objectsCount <= 5)
            {
                // Уровень опасности 0
                //Debug.Log("Не превышено максимальное количество объектов на линии " + (i + 1) + ". Уровень опасности 0.");
                blinkingController.SetBlinkMode(i, BlinkingController.BlinkMode.None);
                // Дополнительные действия, связанные с уровнем опасности 0
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
            Destroy(letterBoxes[i]);    // Уничтожаем объект стандартным методом Unity
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