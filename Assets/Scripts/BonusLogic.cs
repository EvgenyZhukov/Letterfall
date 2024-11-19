using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BonusLogic : MonoBehaviour
{
    public AudioControllerScript audioControllerScript;
    public GameField gameField;
    public WordLine wordLine;
    public DifficultManager difficultManager;
    public TextLine textLine;
    public ControllerUI controllerUI;

    public GameObject bonusLighting;
    public Image lightingLine1;
    public Image lightingLine2;
    public Image lightingLine3;
    public Image lightingFlash;
    private float animationSpeed = 4.0f;
    private bool isAnimating = false;


    private int maxLettersSearchWord; // Максимальное количество букв в слове
    public List<string>[] wordsByLength; // Список слов, где каждый элемент массива содержит слова определенной длины
    private string searchWord;
    private GameObject[] letterBoxes;

    public int bonus_01;
    public int bonus_02;
    public int bonus_03;

    public bool readyBonus_01;
    public bool readyBonus_02;
    public bool readyBonus_03;

    private void Start()
    {

        maxLettersSearchWord = wordLine.maxLetters;
        StartCoroutine(PrepareWordLists());

        PlayerPrefsMethods.GetBonusAmount(out bonus_01, out bonus_02, out bonus_03);
    }
    private void Update()
    {
        LightingFlash();
        BonusCountAvailable();
    }
    #region Bonus_01 Region
    public void Bonus_01()
    {
        audioControllerScript.soundBonusActivated.Play();

        wordLine.CancelWord();

        textLine.ShowText(1);

        // Находим все объекты с тэгом "LetterBox"
        GameObject[] letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox");

        // Проверяем, если найдено 14 или более объектов
        int objectsToDestroy = Mathf.Min(letterBoxes.Length, 14);

        for (int i = 0; i < objectsToDestroy; i++)
        {
            // Получаем компонент с методом Destroy
            LetterBox letterBoxComponent = letterBoxes[i].GetComponent<LetterBox>();
            if (letterBoxComponent != null)
            {
                letterBoxComponent.Destroy();
            }
        }
        gameField.CheckLines();
        difficultManager.SpawnWave(objectsToDestroy);

        bonus_01--;
        PlayerPrefsMethods.SetBonusAmount(bonus_01, bonus_02, bonus_03);
    }
    #endregion

    #region Bonus_02 Region
    public void Bonus_02()
    {
        audioControllerScript.soundLightingHit.Play();

        wordLine.CancelWord();

        textLine.ShowText(2);

        if (!isAnimating) StartCoroutine(AnimateLightning());

        // Находим все объекты с тэгом "LetterBox"
        GameObject[] letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox");

        // Перебираем все найденные объекты
        foreach (GameObject letterBox in letterBoxes)
        {
            // Проверяем координату по оси Y
            if (letterBox.transform.position.y >= 4)
            {
                // Получаем компонент LetterBox и уничтожаем, если компонент найден
                LetterBox letterBoxComponent = letterBox.GetComponent<LetterBox>();
                if (letterBoxComponent != null)
                {
                    letterBoxComponent.Destroy();
                }
            }
        }
        gameField.CheckLines();

        bonus_02--;
        PlayerPrefsMethods.SetBonusAmount(bonus_01, bonus_02, bonus_03);

        //Invoke("SaveGameAfterBonus_03", 2f);
    }

    private IEnumerator AnimateLightning()
    {
        isAnimating = true; // Устанавливаем флаг анимации в true

        float duration = 0.5f / animationSpeed; // Длительность анимации в секундах, учитывая скорость

        // Первая стадия
        yield return StartCoroutine(AnimateScaleAndPosition(new Vector3(bonusLighting.transform.localScale.x, 1.2f, bonusLighting.transform.localScale.z),
                                                            new Vector3(bonusLighting.transform.localPosition.x, 385f, bonusLighting.transform.localPosition.z),
                                                            new Color(1, 1, 0.1f, 1), duration));

        // Вторая стадия: изменение масштаба по оси X
        bonusLighting.transform.localScale = new Vector3(-bonusLighting.transform.localScale.x, bonusLighting.transform.localScale.y, bonusLighting.transform.localScale.z);

        // Небольшая задержка для плавного перехода между стадиями
        yield return new WaitForSeconds(0.1f / animationSpeed);

        // Третья стадия (повтор первой)
        yield return StartCoroutine(AnimateScaleAndPosition(new Vector3(bonusLighting.transform.localScale.x, 1.2f, bonusLighting.transform.localScale.z),
                                                            new Vector3(bonusLighting.transform.localPosition.x, 385f, bonusLighting.transform.localPosition.z),
                                                            new Color(1, 1, 0.1f, 1), duration));

        isAnimating = false; // После завершения анимации устанавливаем флаг анимации в false
    }

    private IEnumerator AnimateScaleAndPosition(Vector3 targetScale, Vector3 targetPosition, Color targetColor, float duration)
    {
        float elapsedTime = 0f;

        Vector3 initialScale = bonusLighting.transform.localScale;
        Vector3 initialPosition = bonusLighting.transform.localPosition;
        Color initialColor = new Color(1, 1, 0.1f, 0);

        // Увеличение масштаба и изменение позиции
        while (elapsedTime < duration)
        {
            bonusLighting.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            bonusLighting.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);

            // Меняем только прозрачность, а не цвет
            float currentAlpha = Mathf.Lerp(initialColor.a, targetColor.a, elapsedTime / duration);
            lightingLine1.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine2.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine3.color = new Color(1, 1, 0.1f, currentAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Вручную устанавливаем альфа значение в 1
        lightingLine1.color = new Color(1, 1, 0.1f, 1);
        lightingLine2.color = new Color(1, 1, 0.1f, 1);
        lightingLine3.color = new Color(1, 1, 0.1f, 1);

        elapsedTime = 0f;
        // Возвращение масштаба и позиции
        while (elapsedTime < duration)
        {
            bonusLighting.transform.localScale = Vector3.Lerp(targetScale, initialScale, elapsedTime / duration);
            bonusLighting.transform.localPosition = Vector3.Lerp(targetPosition, initialPosition, elapsedTime / duration);

            // Возвращение прозрачности к нулю
            float currentAlpha = Mathf.Lerp(targetColor.a, initialColor.a, elapsedTime / duration);
            lightingLine1.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine2.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine3.color = new Color(1, 1, 0.1f, currentAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Вручную устанавливаем альфа значение в 0
        lightingLine1.color = new Color(1, 1, 0.1f, 0);
        lightingLine2.color = new Color(1, 1, 0.1f, 0);
        lightingLine3.color = new Color(1, 1, 0.1f, 0);
    }

    void LightingFlash()
    {
        if (isAnimating)
        {
            if (lightingLine1.color.a >= 0.33f)
            {
                lightingFlash.color = new Color(0.9f, 1, 1, 0.01f);
                //lightingFlash.SetActive(true);
            }
            else if (lightingLine1.color.a < 0.33f)
            {
                lightingFlash.color = new Color(0.9f, 1, 1, 0f);
                //lightingFlash.SetActive(false);
            }
        }
        else
        {
            lightingFlash.color = new Color(0.9f, 1, 1, 0f);
            //lightingFlash.SetActive(false);
        }
    }
    #endregion

    #region Bonus_03 Region
    public void Bonus_03()
    {
        wordLine.CancelWord();

        audioControllerScript.soundBonusActivated.Play();

        controllerUI.ButtonCancel();

        textLine.ShowText(3);

        letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox"); // Находим все объекты с тегом "LetterBox"
        List<char> letters = new List<char>();

        foreach (GameObject letterGameObject in letterBoxes)
        {
            LetterBox letterBoxComponent = letterGameObject.GetComponent<LetterBox>();
            if (letterBoxComponent != null)
            {
                letters.Add(char.ToLower(letterBoxComponent.letter));
            }
        }

        letters.Sort(); // Сортируем буквы в алфавитном порядке

        searchWord = FindFirstWordFromLetters(letters);

        //Debug.Log("Искомое слово: " + searchWord);

        Invoke("InputWord", 1f);

        bonus_03--;
        PlayerPrefsMethods.SetBonusAmount(bonus_01, bonus_02, bonus_03);
    }

    IEnumerator PrepareWordLists()
    {
        //Debug.Log("Корутина запущена");
        wordsByLength = new List<string>[maxLettersSearchWord + 1]; // Создаем массив списков слов

        int wordsProcessed = 0; // Счетчик обработанных слов

        foreach (string[] words in Dictionary.DictionaryWords.WordCategories.Values)
        {
            foreach (string word in words)
            {
                int wordLength = word.Length;
                if (wordLength > maxLettersSearchWord || wordLength < 1)
                    continue; // Пропускаем слова, длина которых не входит в допустимый диапазон

                if (wordsByLength[wordLength] == null)
                {
                    wordsByLength[wordLength] = new List<string>(); // Инициализируем список, если он еще не был инициализирован
                }

                wordsByLength[wordLength].Add(word.ToLower()); // Добавляем слово в список

                wordsProcessed++; // Увеличиваем счетчик обработанных слов

                if (wordsProcessed >= 100)
                {
                    wordsProcessed = 0; // Сбрасываем счетчик
                    yield return null; // Подождать до следующего кадра
                }
            }
        }

        //Debug.Log("Корутина завершена");
        yield break;
    }

    public string FindFirstWordFromLetters(List<char> letters)
    {
        for (int i = maxLettersSearchWord; i >= 1; i--) // Начинаем с самых длинных слов
        {
            if (wordsByLength[i] != null)
            {
                foreach (string word in wordsByLength[i])
                {
                    if (IsWordPossible(word, letters))
                    {
                        // Находим индекс слова в неотсортированном списке
                        int index = wordsByLength[i].IndexOf(word.ToLower());
                        return wordsByLength[i][index];
                    }
                }
            }
        }

        //Debug.Log("У меня нет слов.");
        textLine.ShowText(11); 
        bonus_03++;
        PlayerPrefsMethods.SetBonusAmount(bonus_01, bonus_02, bonus_03);

        return null;
    }

    private bool IsWordPossible(string word, List<char> letters)
    {
        List<char> tempLetters = new List<char>(letters); // Создаем временный список букв, чтобы не изменять оригинальный список

        foreach (char c in word)
        {
            if (tempLetters.Contains(c))
            {
                tempLetters.Remove(c); // Убираем использованную букву
            }
            else
            {
                return false; // Если буква не найдена, слово невозможно составить
            }
        }
        return true;
    }

    void InputWord()
    {
        List<LetterBox> letterBoxesForSelect = CollectLetterBoxesForWord(searchWord, letterBoxes); // Получаем список объектов LetterBox для найденного слова

        // Вызываем метод OnPointerDown для каждого объекта LetterBox, чтобы имитировать выбор
        foreach (LetterBox letterBox in letterBoxesForSelect)
        {
            // Создаем объект PointerEventData для передачи в метод OnPointerDown
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            letterBox.OnPointerDown(pointerEventData);
        }
    }

    public List<LetterBox> CollectLetterBoxesForWord(string word, GameObject[] letterBoxes)
    {
        List<LetterBox> letterBoxesForWord = new List<LetterBox>();

        // Проходим по каждой букве в слове
        foreach (char c in word)
        {
            // Находим первый объект LetterBox с нужной буквой
            foreach (GameObject letterGameObject in letterBoxes)
            {
                LetterBox letterBoxComponent = letterGameObject.GetComponent<LetterBox>();
                if (letterBoxComponent != null && char.ToLower(letterBoxComponent.letter) == char.ToLower(c))
                {
                    // Добавляем объект LetterBox в список, если его буква соответствует букве в слове
                    letterBoxesForWord.Add(letterBoxComponent);

                    // Удаляем этот объект из массива, чтобы не использовать его снова
                    letterBoxes = letterBoxes.Where(box => box != letterGameObject).ToArray();
                    break; // Переходим к следующей букве в слове
                }
            }
        }

        return letterBoxesForWord;
    }


    #endregion

    void BonusCountAvailable()
    {
        int bonusAvailable = 0;

        if (readyBonus_01)
        {
            bonusAvailable++;
        }
        if (readyBonus_02)
        {
            bonusAvailable++;
        }
        if (readyBonus_03)
        {
            bonusAvailable++;
        }
        controllerUI.bonusAvailable = bonusAvailable;
    }

    /*
    void SaveGameAfterBonus_03()
    {

    }
    */
}
