using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class WordLine : MonoBehaviour
{

    public AudioControllerScript audioControllerScript;

    public ControllerUI controllerUI;
    //public WordsScore wordScore;
    public int maxLetters = 10;
    private char emptyLetterChar = '-';
    public List<LetterBox> selectedLetters = new List<LetterBox>();
    private TMP_Text textMeshProText;
    public GameField gameField;
    public ScoreLine scoreLine;
    public DifficultManager difficultManager;
    public TextLine textLine;

    public Dictionary.Trie dictionaryTrie;

    private string word;

    void Start()
    {

        textMeshProText = GetComponentInChildren<TMP_Text>();
        if (textMeshProText == null)
        {
            //Debug.LogError("�� ������ ��������� TMP_Text � �������� ��������!");
        }

        UpdateWord();

        dictionaryTrie = new Dictionary.Trie();
        foreach (string[] words in Dictionary.DictionaryWords.WordCategories.Values)
        {
            foreach (string word in words)
            {
                dictionaryTrie.Insert(word.ToLower()); // ����������� ����� � ������� �������� ����� ��������
            }
        }
    }

    public void AddLetter(LetterBox letter)
    {
        if (selectedLetters.Count >= maxLetters)
            return;

        selectedLetters.Add(letter);
        UpdateWord();
    }

    public void RemoveLetter(LetterBox letter)
    {
        selectedLetters.Remove(letter);
        UpdateWord();
    }

    private void UpdateWord()
    {
        StringBuilder wordBuilder = new StringBuilder();
        foreach (LetterBox letter in selectedLetters)
        {
            wordBuilder.Append(letter.letter);
        }

        int remainingSpaces = maxLetters - selectedLetters.Count;
        for (int i = 0; i < remainingSpaces; i++)
        {
            wordBuilder.Append(emptyLetterChar);
        }

        textMeshProText.text = wordBuilder.ToString();
    }

    public void AcceptWord()
    {
        word = GetWordWithoutUnderscores().ToLower();
        if (dictionaryTrie.Search(word))
        {
            //Debug.Log($"����� \"{word}\" �������!");
            //wordScore.AddWord(word);

            //int wordsCount = wordScore.GetUniqueWordCount();

            //int wordsCount = PlayerPrefsMethods.GetScoreWords();

            //wordsCount++;

            //PlayerPrefsMethods.SetScoreWords(wordsCount);

            //YandexGame.NewLeaderboardScores("maxWords", wordsCount);
            

            /*
            leaderBoardGet.technoName = "maxwords";
            leaderBoardGet.GetLeaderboard();
            int lastScore = leaderBoardGet.scorePlayer;
            */

            Success();
        }
        else
        {
            //Debug.Log($"����� \"{word}\" �� ������� � �������.");
            Mistake();
            if (word == "")
            {
                textLine.ShowText(12);
            }
            else
            {
                textLine.ShowText(10);
            }
        }
    }

    public void CancelWord()
    {
        List<LetterBox> lettersToRemove = new List<LetterBox>(selectedLetters);
        foreach (LetterBox letter in lettersToRemove)
        {
            letter.selected = false;
            letter.ChangeMaterial();
            RemoveLetter(letter);
        }
    }

    public void DestroyWord()
    {
        List<LetterBox> lettersToRemove = new List<LetterBox>(selectedLetters);
        foreach (LetterBox letter in lettersToRemove)
        {

            letter.Destroy();
        }
    }

    public void ShakeWord()
    {
        List<LetterBox> lettersToRemove = new List<LetterBox>(selectedLetters);
        foreach (LetterBox letter in lettersToRemove)
        {
            letter.StartShake();
        }
    }

    private string GetWordWithoutUnderscores()
    {
        StringBuilder wordBuilder = new StringBuilder();
        foreach (LetterBox letter in selectedLetters)
        {
            if (letter.letter != emptyLetterChar)
            {
                wordBuilder.Append(letter.letter);
            }
        }
        return wordBuilder.ToString();
    }

    private void Success()
    {
        audioControllerScript.soundWordAccepted.Play();

        AssignScoresToLetters();

        scoreLine.CalculateScore(word);
        //difficultManager.lettersCount = gameField.GetTotalObjectsCount();
        //scoreLine.SetNewScore();

        DestroyWord();
        CancelWord();
        gameField.CheckLines();
        difficultManager.SpawnWave(true);
    }

    private void Mistake()
    {
        audioControllerScript.soundWordCanceled.Play();

        ShakeWord();
        CancelWord();
        difficultManager.SpawnWave(false);
    }

    private void AssignScoresToLetters()
    {
        // ������� ������� ��� �������� ���������� ���������� ������ �����
        Dictionary<char, int> letterCount = new Dictionary<char, int>();

        // ������� ���������� ���������� ������ �����
        foreach (LetterBox letter in selectedLetters)
        {
            if (letterCount.ContainsKey(letter.letter))
            {
                letterCount[letter.letter]++;
            }
            else
            {
                letterCount[letter.letter] = 1;
            }
        }

        // ����������� ���� � ���������� ���������� ������ �����
        foreach (LetterBox letter in selectedLetters)
        {
            if (scoreLine.letterScores.TryGetValue(letter.letter, out int score))
            {
                letter.score = score;
            }
            else
            {
                letter.score = 0; // ��� ������ �������� �� ���������, ���� ����� �� ������� � �������
            }

            // ����������� ���������� ���������� �����
            letter.multiple = letterCount[letter.letter];
        }
    }
}