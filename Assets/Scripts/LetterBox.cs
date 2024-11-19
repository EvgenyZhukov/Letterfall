using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LetterBox : MonoBehaviour, IPointerDownHandler
{
    public AudioControllerScript audioControllerScript;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public Sprite[] spritesSelect;
    private int selectedSpriteIndex;
    public bool selected = false;
    public TMP_Text textMeshProText;

    public char letter;
    public int spawnerNumber;
    public GameObject tail;

    private GameField gameField;
    private WordLine wordLine; // Ссылка на скрипт WordLine
    public LetterAnimation letterAnimation;

    public GameObject prefabFloatingText;

    public int score;
    public int multiple;

    private char defLetter = 'О';

    public bool creatSoundLocker = false;

    public bool letterSelected = false;

    void Start()
    {
        audioControllerScript = FindObjectOfType<AudioControllerScript>();
        gameField = FindObjectOfType<GameField>();
        wordLine = FindObjectOfType<WordLine>(); // Находим объект WordLine на сцене

        SetRandomMaterial();
        SetRandomLetter();

        // Добавляем информацию о созданном объекте на игровом поле
        gameField.AddObjectToLine(spawnerNumber);
        gameField.AddLetter(letter);
    }

    void Update()
    {
        if (!creatSoundLocker)
        {
            if (gameObject.transform.position.y <= 8.45f)
            {
                audioControllerScript.soundLetterCreate.pitch = Random.Range(0.9f, 1.1f);
                audioControllerScript.soundLetterCreate.Play();

                creatSoundLocker = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioControllerScript.soundSelectLetter.pitch = Random.Range(0.95f, 1.05f);
        audioControllerScript.soundSelectLetter.Play();

        if (!wordLine)
        {
            //Debug.LogError("Не найден компонент WordLine!");
            return;
        }

        // Проверяем, что количество выбранных букв меньше максимального значения
        if (wordLine.selectedLetters.Count < wordLine.maxLetters || selected)
        {
            // Вызываем методы AddLetter и RemoveLetter в зависимости от состояния выбора буквы
            if (!wordLine.selectedLetters.Contains(this))
            {
                wordLine.AddLetter(this);
                selected = true;
            }
            else
            {
                wordLine.RemoveLetter(this);
                selected = false;
            }
            letterAnimation.isAnimating = true;
            ChangeMaterial();
        }
        else
        {
            // Если буква уже выбрана и текущее количество букв равно максимальному значению,
            // то разрешаем отменить выбор этой буквы
            if (wordLine.selectedLetters.Contains(this))
            {
                wordLine.RemoveLetter(this);
                selected = false;
                ChangeMaterial();
            }
            else
            {
                //Debug.Log("Максимальное количество букв уже выбрано!");
            }
        }

    }

    void SetRandomMaterial()
    {
        selectedSpriteIndex = Random.Range(0, sprites.Length); // Случайный индекс из массива материалов
        //Debug.Log("Selected material: " + selectedSpriteIndex);

        selected = false;
        ChangeMaterial();
    }

    public void ChangeMaterial()
    {
        if (selected)
        {
            spriteRenderer.sprite = spritesSelect[selectedSpriteIndex];
        }
        else
        {
            spriteRenderer.sprite = sprites[selectedSpriteIndex];
        }
    }

    public void Destroy()
    {
        gameField.objectsPerLine[spawnerNumber]--; // Отнимаем количество букв на линии
        gameField.RemoveLetter(letter); // Отнимаем количество букв по алфавиту

        SpawnFloatingText();
        letterAnimation.shrinkAndDestroy = true;
    }
    void SetRandomLetter()
    {
        if (!letterSelected)
        {
            // Получаем случайную букву
            letter = LetterRandomizer.GetRandomLetter();
        }

            // Получаем индекс буквы в массиве максимальных значений
            int maxIndex = (int)letter - 1040; // ASCII-код буквы "А" - 1040

            // Проверяем, не превышает ли количество этой буквы максимального значения
            if (maxIndex >= 0 && maxIndex < gameField.maxCounts.Count)
            {
                char letterKey = letter.ToString()[0];
                int maxAllowedCount = gameField.maxCounts[letterKey];
                int currentCount = gameField.GetLetterCount(letter);

                if (currentCount < maxAllowedCount)
                {
                    // Если количество букв не превышает максимальное, устанавливаем ее для объекта
                    textMeshProText.text = letter.ToString();
                }
                else
                {
                    // Если количество букв уже максимальное, выполняем метод ResetRandomLetter
                    ResetRandomLetter();
                }
            }
        
    }

    void ResetRandomLetter()
    {
        // Генерируем случайное число, определяющее направление обхода массива
        bool reverseOrder = Random.value > 0.5f;

        // Определяем начальный индекс для обхода массива
        int startIndex = reverseOrder ? gameField.maxCounts.Count - 1 : 0;
        int endIndex = reverseOrder ? -1 : gameField.maxCounts.Count;

        // Определяем шаг обхода массива
        int step = reverseOrder ? -1 : 1;

        // Перебираем буквы в указанном порядке
        for (int i = startIndex; i != endIndex; i += step)
        {
            char letterKey = (char)(i + 1040); // Получаем букву по индексу
            int maxAllowedCount = gameField.maxCounts[letterKey];
            int currentCount = gameField.GetLetterCount(letterKey);

            // Проверяем, если количество букв не превышает максимальное
            if (currentCount < maxAllowedCount)
            {
                // Назначаем букву, которая еще не максимальная
                letter = letterKey;
                textMeshProText.text = letter.ToString();
                return; // Завершаем метод после назначения буквы
            }
        }

        // Если все буквы максимальные, назначаем букву "О"
        letter = defLetter;
        textMeshProText.text = letter.ToString();
    }

    public void StartShake()
    {
        letterAnimation.TriggerShake();
    }

    public void SpawnFloatingText()
    {
        //Debug.Log($"Буква '{textMeshProText.text}' получила значение score: {score}");
        //Debug.Log($"Буква '{textMeshProText.text}' повторилась раз: {multiple}");

        GameObject floatingTextObject = Instantiate(prefabFloatingText, transform.position, transform.rotation);
        FloatingText floatingText = floatingTextObject.GetComponent<FloatingText>();
        floatingText.SetText(score * multiple);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка тега объекта, с которым произошло соприкосновение
        if (collision.gameObject.CompareTag("LetterBox") || collision.gameObject.CompareTag("Boundary"))
        {
            // Действия при соприкосновении с объектом с тегом "LetterBox"
            //Debug.Log("Collision with LetterBox detected!");
            Invoke("DestroyTrail", 0.05f);
        }
    }
    void DestroyTrail()
    {
        Destroy(tail);
    }
}