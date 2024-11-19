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
    private WordLine wordLine; // ������ �� ������ WordLine
    public LetterAnimation letterAnimation;

    public GameObject prefabFloatingText;

    public int score;
    public int multiple;

    private char defLetter = '�';

    public bool creatSoundLocker = false;

    public bool letterSelected = false;

    void Start()
    {
        audioControllerScript = FindObjectOfType<AudioControllerScript>();
        gameField = FindObjectOfType<GameField>();
        wordLine = FindObjectOfType<WordLine>(); // ������� ������ WordLine �� �����

        SetRandomMaterial();
        SetRandomLetter();

        // ��������� ���������� � ��������� ������� �� ������� ����
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
            //Debug.LogError("�� ������ ��������� WordLine!");
            return;
        }

        // ���������, ��� ���������� ��������� ���� ������ ������������� ��������
        if (wordLine.selectedLetters.Count < wordLine.maxLetters || selected)
        {
            // �������� ������ AddLetter � RemoveLetter � ����������� �� ��������� ������ �����
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
            // ���� ����� ��� ������� � ������� ���������� ���� ����� ������������� ��������,
            // �� ��������� �������� ����� ���� �����
            if (wordLine.selectedLetters.Contains(this))
            {
                wordLine.RemoveLetter(this);
                selected = false;
                ChangeMaterial();
            }
            else
            {
                //Debug.Log("������������ ���������� ���� ��� �������!");
            }
        }

    }

    void SetRandomMaterial()
    {
        selectedSpriteIndex = Random.Range(0, sprites.Length); // ��������� ������ �� ������� ����������
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
        gameField.objectsPerLine[spawnerNumber]--; // �������� ���������� ���� �� �����
        gameField.RemoveLetter(letter); // �������� ���������� ���� �� ��������

        SpawnFloatingText();
        letterAnimation.shrinkAndDestroy = true;
    }
    void SetRandomLetter()
    {
        if (!letterSelected)
        {
            // �������� ��������� �����
            letter = LetterRandomizer.GetRandomLetter();
        }

            // �������� ������ ����� � ������� ������������ ��������
            int maxIndex = (int)letter - 1040; // ASCII-��� ����� "�" - 1040

            // ���������, �� ��������� �� ���������� ���� ����� ������������� ��������
            if (maxIndex >= 0 && maxIndex < gameField.maxCounts.Count)
            {
                char letterKey = letter.ToString()[0];
                int maxAllowedCount = gameField.maxCounts[letterKey];
                int currentCount = gameField.GetLetterCount(letter);

                if (currentCount < maxAllowedCount)
                {
                    // ���� ���������� ���� �� ��������� ������������, ������������� �� ��� �������
                    textMeshProText.text = letter.ToString();
                }
                else
                {
                    // ���� ���������� ���� ��� ������������, ��������� ����� ResetRandomLetter
                    ResetRandomLetter();
                }
            }
        
    }

    void ResetRandomLetter()
    {
        // ���������� ��������� �����, ������������ ����������� ������ �������
        bool reverseOrder = Random.value > 0.5f;

        // ���������� ��������� ������ ��� ������ �������
        int startIndex = reverseOrder ? gameField.maxCounts.Count - 1 : 0;
        int endIndex = reverseOrder ? -1 : gameField.maxCounts.Count;

        // ���������� ��� ������ �������
        int step = reverseOrder ? -1 : 1;

        // ���������� ����� � ��������� �������
        for (int i = startIndex; i != endIndex; i += step)
        {
            char letterKey = (char)(i + 1040); // �������� ����� �� �������
            int maxAllowedCount = gameField.maxCounts[letterKey];
            int currentCount = gameField.GetLetterCount(letterKey);

            // ���������, ���� ���������� ���� �� ��������� ������������
            if (currentCount < maxAllowedCount)
            {
                // ��������� �����, ������� ��� �� ������������
                letter = letterKey;
                textMeshProText.text = letter.ToString();
                return; // ��������� ����� ����� ���������� �����
            }
        }

        // ���� ��� ����� ������������, ��������� ����� "�"
        letter = defLetter;
        textMeshProText.text = letter.ToString();
    }

    public void StartShake()
    {
        letterAnimation.TriggerShake();
    }

    public void SpawnFloatingText()
    {
        //Debug.Log($"����� '{textMeshProText.text}' �������� �������� score: {score}");
        //Debug.Log($"����� '{textMeshProText.text}' ����������� ���: {multiple}");

        GameObject floatingTextObject = Instantiate(prefabFloatingText, transform.position, transform.rotation);
        FloatingText floatingText = floatingTextObject.GetComponent<FloatingText>();
        floatingText.SetText(score * multiple);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �������� ���� �������, � ������� ��������� ���������������
        if (collision.gameObject.CompareTag("LetterBox") || collision.gameObject.CompareTag("Boundary"))
        {
            // �������� ��� ��������������� � �������� � ����� "LetterBox"
            //Debug.Log("Collision with LetterBox detected!");
            Invoke("DestroyTrail", 0.05f);
        }
    }
    void DestroyTrail()
    {
        Destroy(tail);
    }
}