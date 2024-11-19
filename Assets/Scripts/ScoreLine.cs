using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using YG;

public class ScoreLine : MonoBehaviour
{

    public ControllerUI controllerUI;
    public TextLine textLine;
    public int score;  // Переменная для хранения количества очков
    private int wordScore;
    public TMP_Text scoreText; // Ссылка на компонент TMP_Text

    public Dictionary<char, int> letterScores = new Dictionary<char, int>()
    {
        {'О', 1}, {'Е', 2}, {'А', 2}, {'Н', 3}, {'И', 3}, {'Т', 3}, {'С', 4}, {'Л', 4},
        {'В', 4}, {'Р', 4}, {'К', 5}, {'Д', 5}, {'М', 5}, {'У', 5}, {'П', 5}, {'Я', 6},
        {'Ь', 10}, {'Ы', 6}, {'Г', 6}, {'Б', 6}, {'Ч', 6}, {'З', 6}, {'Ж', 8}, {'Й', 8},
        {'Ш', 8}, {'Х', 8}, {'Ю', 10}, {'Э', 10}, {'Ц', 10}, {'Щ', 10}, {'Ф', 10}, {'Ъ', 20}
    };
    void Awake()
    {
        //ScoreInitial();
    }

    void Start()
    {
        // Обновляем текст в компоненте TMP_Text при запуске сцены
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

        // Изменение множителя в зависимости от количества букв в слове

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

                // Умножаем очки за букву на количество её вхождений в слове
                wordScore += letterScore * letterCounts[letter];
            }
        }

        // Умножаем общее количество очков на множитель и округляем до целого значения
        wordScore = Mathf.RoundToInt(wordScore * multiplier);

        //Debug.Log("Очков за слово: " + wordScore);

        SetNewScore();
    }

    public void SetNewScore()
    {
        int startScore = score;
        int targetScore = score + wordScore;

        // Запускаем корутину для плавного изменения значения очков
        StartCoroutine(UpdateScoreOverTime(startScore, targetScore));

        score = targetScore;

        PlayerPrefsMethods.SetScore(score); // текущее значение очков

        // записываем в максимальные очки то значение, которое больше: текущие очки или то что было в максимальных

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

    // Корутину для плавного изменения значения очков
    private IEnumerator UpdateScoreOverTime(int startScore, int targetScore)
    {
        while (startScore < targetScore)
        {
            startScore++;
            UpdateScoreText(startScore);
            //Debug.Log("Score: " + score + " / Target Score: " + targetScore);
            yield return null; // Ждем один кадр
        }
    }

    // Метод для обновления текста счета
    public void UpdateScoreText(int score)
    {
            // Преобразуем значение переменной score в строку и устанавливаем его в текст компонента
            scoreText.text = score.ToString();
    }
    /*
    void ScoreInitial()
    {
        // Создаем объект SafeInt
        score = new SafeInt(0);

        // Инициализируем соль после создания объекта
        score.InitializeSalt();
    }
    */

}