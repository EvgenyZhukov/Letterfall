using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using YG;

public class ScoreLine : MonoBehaviour
{

    public ControllerUI controllerUI;
    public TextLine textLine;
    public int score;  // ���������� ��� �������� ���������� �����
    private int wordScore;
    public TMP_Text scoreText; // ������ �� ��������� TMP_Text

    public Dictionary<char, int> letterScores = new Dictionary<char, int>()
    {
        {'�', 1}, {'�', 2}, {'�', 2}, {'�', 3}, {'�', 3}, {'�', 3}, {'�', 4}, {'�', 4},
        {'�', 4}, {'�', 4}, {'�', 5}, {'�', 5}, {'�', 5}, {'�', 5}, {'�', 5}, {'�', 6},
        {'�', 10}, {'�', 6}, {'�', 6}, {'�', 6}, {'�', 6}, {'�', 6}, {'�', 8}, {'�', 8},
        {'�', 8}, {'�', 8}, {'�', 10}, {'�', 10}, {'�', 10}, {'�', 10}, {'�', 10}, {'�', 20}
    };
    void Awake()
    {
        //ScoreInitial();
    }

    void Start()
    {
        // ��������� ����� � ���������� TMP_Text ��� ������� �����
        UpdateScoreText(score);
    }

    public void CalculateScore(string word)
    {
        word = word.ToUpper();

        //Debug.Log("Calculating score for word: " + word + " (Length: " + word.Length + ")");

        wordScore = 0;
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();

        foreach (char letter in word)
        {
            if (letterCounts.ContainsKey(letter))
            {
                letterCounts[letter]++;
            }
            else
            {
                letterCounts[letter] = 1;
            }
        }

        //Debug.Log("Letter scores and counts:");
        foreach (var pair in letterCounts)
        {
            char letter = pair.Key;
            int count = pair.Value;
            int letterScore = letterScores.ContainsKey(letter) ? letterScores[letter] : 0;
            //Debug.Log("Letter: " + letter + ", Score: " + letterScore + ", Count: " + count);
        }

        int wordLength = word.Length;
        float multiplier = 1f;

        // ��������� ��������� � ����������� �� ���������� ���� � �����

        if (wordLength < 6)
        {
            textLine.ShowText(12);
        }
        else if(wordLength == 6)
        {
            multiplier = 1.1f;
            textLine.ShowText(4);
        }
        else if (wordLength == 7)
        {
            multiplier = 1.25f;
            textLine.ShowText(5);
        }
        else if (wordLength == 8)
        {
            multiplier = 1.5f;
            textLine.ShowText(6);
        }
        else if (wordLength == 9)
        {
            multiplier = 2f;
            textLine.ShowText(7);
        }
        else if (wordLength == 10)
        {
            multiplier = 3f;
            textLine.ShowText(8);
        }

        //Debug.Log("Multiplier for word length: " + multiplier);

        foreach (char letter in word)
        {
            if (letterScores.ContainsKey(letter))
            {
                int letterScore = letterScores[letter];

                // �������� ���� �� ����� �� ���������� � ��������� � �����
                wordScore += letterScore * letterCounts[letter];
            }
        }

        // �������� ����� ���������� ����� �� ��������� � ��������� �� ������ ��������
        wordScore = Mathf.RoundToInt(wordScore * multiplier);

        //Debug.Log("����� �� �����: " + wordScore);

        SetNewScore();
    }

    public void SetNewScore()
    {
        int startScore = score;
        int targetScore = score + wordScore;

        // ��������� �������� ��� �������� ��������� �������� �����
        StartCoroutine(UpdateScoreOverTime(startScore, targetScore));

        score = targetScore;

        PlayerPrefsMethods.SetScore(score); // ������� �������� �����

        // ���������� � ������������ ���� �� ��������, ������� ������: ������� ���� ��� �� ��� ���� � ������������

        if (!YandexGame.auth)
        {
            if (score > PlayerPrefsMethods.GetMaxScoreAnon())
            {
                PlayerPrefsMethods.SetMaxScoreAnon(score);

                //Debug.Log("Score anon: " + PlayerPrefsMethods.GetMaxScoreAnon());

                //controllerUI.leaderboardYG_scoreAnon.NewScore(score);
            }
        }
        else
        {
            if (score > PlayerPrefsMethods.GetMaxScore())
            {
                PlayerPrefsMethods.SetMaxScore(score);
                YandexGame.NewLeaderboardScores("maxScore", score);

                //controllerUI.leaderboardYG_scoreAnon.NewScore(score);
            }
        }
    }

    // �������� ��� �������� ��������� �������� �����
    private IEnumerator UpdateScoreOverTime(int startScore, int targetScore)
    {
        while (startScore < targetScore)
        {
            startScore++;
            UpdateScoreText(startScore);
            //Debug.Log("Score: " + score + " / Target Score: " + targetScore);
            yield return null; // ���� ���� ����
        }
    }

    // ����� ��� ���������� ������ �����
    public void UpdateScoreText(int score)
    {
            // ����������� �������� ���������� score � ������ � ������������� ��� � ����� ����������
            scoreText.text = score.ToString();
    }
    /*
    void ScoreInitial()
    {
        // ������� ������ SafeInt
        score = new SafeInt(0);

        // �������������� ���� ����� �������� �������
        score.InitializeSalt();
    }
    */

}