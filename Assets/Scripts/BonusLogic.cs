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


    private int maxLettersSearchWord; // ������������ ���������� ���� � �����
    public List<string>[] wordsByLength; // ������ ����, ��� ������ ������� ������� �������� ����� ������������ �����
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

        // ������� ��� ������� � ����� "LetterBox"
        GameObject[] letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox");

        // ���������, ���� ������� 14 ��� ����� ��������
        int objectsToDestroy = Mathf.Min(letterBoxes.Length, 14);

        for (int i = 0; i < objectsToDestroy; i++)
        {
            // �������� ��������� � ������� Destroy
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

        // ������� ��� ������� � ����� "LetterBox"
        GameObject[] letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox");

        // ���������� ��� ��������� �������
        foreach (GameObject letterBox in letterBoxes)
        {
            // ��������� ���������� �� ��� Y
            if (letterBox.transform.position.y >= 4)
            {
                // �������� ��������� LetterBox � ����������, ���� ��������� ������
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
        isAnimating = true; // ������������� ���� �������� � true

        float duration = 0.5f / animationSpeed; // ������������ �������� � ��������, �������� ��������

        // ������ ������
        yield return StartCoroutine(AnimateScaleAndPosition(new Vector3(bonusLighting.transform.localScale.x, 1.2f, bonusLighting.transform.localScale.z),
                                                            new Vector3(bonusLighting.transform.localPosition.x, 385f, bonusLighting.transform.localPosition.z),
                                                            new Color(1, 1, 0.1f, 1), duration));

        // ������ ������: ��������� �������� �� ��� X
        bonusLighting.transform.localScale = new Vector3(-bonusLighting.transform.localScale.x, bonusLighting.transform.localScale.y, bonusLighting.transform.localScale.z);

        // ��������� �������� ��� �������� �������� ����� ��������
        yield return new WaitForSeconds(0.1f / animationSpeed);

        // ������ ������ (������ ������)
        yield return StartCoroutine(AnimateScaleAndPosition(new Vector3(bonusLighting.transform.localScale.x, 1.2f, bonusLighting.transform.localScale.z),
                                                            new Vector3(bonusLighting.transform.localPosition.x, 385f, bonusLighting.transform.localPosition.z),
                                                            new Color(1, 1, 0.1f, 1), duration));

        isAnimating = false; // ����� ���������� �������� ������������� ���� �������� � false
    }

    private IEnumerator AnimateScaleAndPosition(Vector3 targetScale, Vector3 targetPosition, Color targetColor, float duration)
    {
        float elapsedTime = 0f;

        Vector3 initialScale = bonusLighting.transform.localScale;
        Vector3 initialPosition = bonusLighting.transform.localPosition;
        Color initialColor = new Color(1, 1, 0.1f, 0);

        // ���������� �������� � ��������� �������
        while (elapsedTime < duration)
        {
            bonusLighting.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            bonusLighting.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);

            // ������ ������ ������������, � �� ����
            float currentAlpha = Mathf.Lerp(initialColor.a, targetColor.a, elapsedTime / duration);
            lightingLine1.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine2.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine3.color = new Color(1, 1, 0.1f, currentAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������� ������������� ����� �������� � 1
        lightingLine1.color = new Color(1, 1, 0.1f, 1);
        lightingLine2.color = new Color(1, 1, 0.1f, 1);
        lightingLine3.color = new Color(1, 1, 0.1f, 1);

        elapsedTime = 0f;
        // ����������� �������� � �������
        while (elapsedTime < duration)
        {
            bonusLighting.transform.localScale = Vector3.Lerp(targetScale, initialScale, elapsedTime / duration);
            bonusLighting.transform.localPosition = Vector3.Lerp(targetPosition, initialPosition, elapsedTime / duration);

            // ����������� ������������ � ����
            float currentAlpha = Mathf.Lerp(targetColor.a, initialColor.a, elapsedTime / duration);
            lightingLine1.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine2.color = new Color(1, 1, 0.1f, currentAlpha);
            lightingLine3.color = new Color(1, 1, 0.1f, currentAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������� ������������� ����� �������� � 0
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

        letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox"); // ������� ��� ������� � ����� "LetterBox"
        List<char> letters = new List<char>();

        foreach (GameObject letterGameObject in letterBoxes)
        {
            LetterBox letterBoxComponent = letterGameObject.GetComponent<LetterBox>();
            if (letterBoxComponent != null)
            {
                letters.Add(char.ToLower(letterBoxComponent.letter));
            }
        }

        letters.Sort(); // ��������� ����� � ���������� �������

        searchWord = FindFirstWordFromLetters(letters);

        //Debug.Log("������� �����: " + searchWord);

        Invoke("InputWord", 1f);

        bonus_03--;
        PlayerPrefsMethods.SetBonusAmount(bonus_01, bonus_02, bonus_03);
    }

    IEnumerator PrepareWordLists()
    {
        //Debug.Log("�������� ��������");
        wordsByLength = new List<string>[maxLettersSearchWord + 1]; // ������� ������ ������� ����

        int wordsProcessed = 0; // ������� ������������ ����

        foreach (string[] words in Dictionary.DictionaryWords.WordCategories.Values)
        {
            foreach (string word in words)
            {
                int wordLength = word.Length;
                if (wordLength > maxLettersSearchWord || wordLength < 1)
                    continue; // ���������� �����, ����� ������� �� ������ � ���������� ��������

                if (wordsByLength[wordLength] == null)
                {
                    wordsByLength[wordLength] = new List<string>(); // �������������� ������, ���� �� ��� �� ��� ���������������
                }

                wordsByLength[wordLength].Add(word.ToLower()); // ��������� ����� � ������

                wordsProcessed++; // ����������� ������� ������������ ����

                if (wordsProcessed >= 100)
                {
                    wordsProcessed = 0; // ���������� �������
                    yield return null; // ��������� �� ���������� �����
                }
            }
        }

        //Debug.Log("�������� ���������");
        yield break;
    }

    public string FindFirstWordFromLetters(List<char> letters)
    {
        for (int i = maxLettersSearchWord; i >= 1; i--) // �������� � ����� ������� ����
        {
            if (wordsByLength[i] != null)
            {
                foreach (string word in wordsByLength[i])
                {
                    if (IsWordPossible(word, letters))
                    {
                        // ������� ������ ����� � ����������������� ������
                        int index = wordsByLength[i].IndexOf(word.ToLower());
                        return wordsByLength[i][index];
                    }
                }
            }
        }

        //Debug.Log("� ���� ��� ����.");
        textLine.ShowText(11); 
        bonus_03++;
        PlayerPrefsMethods.SetBonusAmount(bonus_01, bonus_02, bonus_03);

        return null;
    }

    private bool IsWordPossible(string word, List<char> letters)
    {
        List<char> tempLetters = new List<char>(letters); // ������� ��������� ������ ����, ����� �� �������� ������������ ������

        foreach (char c in word)
        {
            if (tempLetters.Contains(c))
            {
                tempLetters.Remove(c); // ������� �������������� �����
            }
            else
            {
                return false; // ���� ����� �� �������, ����� ���������� ���������
            }
        }
        return true;
    }

    void InputWord()
    {
        List<LetterBox> letterBoxesForSelect = CollectLetterBoxesForWord(searchWord, letterBoxes); // �������� ������ �������� LetterBox ��� ���������� �����

        // �������� ����� OnPointerDown ��� ������� ������� LetterBox, ����� ����������� �����
        foreach (LetterBox letterBox in letterBoxesForSelect)
        {
            // ������� ������ PointerEventData ��� �������� � ����� OnPointerDown
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            letterBox.OnPointerDown(pointerEventData);
        }
    }

    public List<LetterBox> CollectLetterBoxesForWord(string word, GameObject[] letterBoxes)
    {
        List<LetterBox> letterBoxesForWord = new List<LetterBox>();

        // �������� �� ������ ����� � �����
        foreach (char c in word)
        {
            // ������� ������ ������ LetterBox � ������ ������
            foreach (GameObject letterGameObject in letterBoxes)
            {
                LetterBox letterBoxComponent = letterGameObject.GetComponent<LetterBox>();
                if (letterBoxComponent != null && char.ToLower(letterBoxComponent.letter) == char.ToLower(c))
                {
                    // ��������� ������ LetterBox � ������, ���� ��� ����� ������������� ����� � �����
                    letterBoxesForWord.Add(letterBoxComponent);

                    // ������� ���� ������ �� �������, ����� �� ������������ ��� �����
                    letterBoxes = letterBoxes.Where(box => box != letterGameObject).ToArray();
                    break; // ��������� � ��������� ����� � �����
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
