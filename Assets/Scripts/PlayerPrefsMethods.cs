using System;
using UnityEngine;
using YG;

public static class PlayerPrefsMethods
{
    /// <summary>
     /// Сохранить текущие очки
    /// </summary>
    /// <param name="score"></param>
    public static void SetMaxScoreAnon(int score)
    {
        PlayerPrefs.SetInt("maxscoreAnon", score);
        //PlayerPrefs.Save();

        //SaveGame();
    }
    /// <summary>
    /// Загрузить текущие очки
    /// </summary>
    /// <returns></returns>
    public static int GetMaxScoreAnon()
    {
        //LoadGame();

        return PlayerPrefs.HasKey("maxscoreAnon") ? PlayerPrefs.GetInt("maxscoreAnon") : 0;
    }

    /// <summary>
    /// Сохранить текущие очки
    /// </summary>
    /// <param name="score"></param>
    public static void SetMaxScore(int score)
    {
        PlayerPrefs.SetInt("maxscore", score);
        //PlayerPrefs.Save();

        //SaveGame();
    }
    /// <summary>
    /// Загрузить текущие очки
    /// </summary>
    /// <returns></returns>
    public static int GetMaxScore()
    {
        //LoadGame();

        return PlayerPrefs.HasKey("maxscore") ? PlayerPrefs.GetInt("maxscore") : 0;
    }

    /// <summary>
    /// Сохранить текущие очки
    /// </summary>
    /// <param name="score"></param>
    public static void SetScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
        //PlayerPrefs.Save();

        //SaveGame();
    }
    /// <summary>
    /// Загрузить текущие очки
    /// </summary>
    /// <returns></returns>
    public static int GetScore()
    {
        //LoadGame();

        return PlayerPrefs.HasKey("score") ? PlayerPrefs.GetInt("score") : 0;
    }
    /*
    /// <summary>
    /// Сохранить текущее количество слов
    /// </summary>
    /// <param name="score"></param>
    public static void SetScoreWords(int words)
    {
        PlayerPrefs.SetInt("maxwords", words);
        //PlayerPrefs.Save();

        //SaveGame();
    }
    /// <summary>
    /// Загрузить текущие очки
    /// </summary>
    /// <returns></returns>
    public static int GetScoreWords()
    {
        //LoadGame();

        return PlayerPrefs.HasKey("maxwords") ? PlayerPrefs.GetInt("maxwords") : 0;
    }
    */
    /// <summary>
    /// Сохранить количество бонусов
    /// </summary>
    /// <param name="score"></param>
    public static void SetBonusAmount(int bonus_01, int bonus_02, int bonus_03)
    {
        PlayerPrefs.SetInt("bonus_01", bonus_01);
        PlayerPrefs.SetInt("bonus_02", bonus_02);
        PlayerPrefs.SetInt("bonus_03", bonus_03);
        //PlayerPrefs.Save();

        //SaveGame();
    }
    /// <summary>
    /// Получить количество бонусов
    /// </summary>
    /// <returns></returns>
    public static void GetBonusAmount(out int bonus_01, out int bonus_02, out int bonus_03)
    {
        //LoadGame();

        bonus_01 = PlayerPrefs.HasKey("bonus_01") ? PlayerPrefs.GetInt("bonus_01") : 1;
        bonus_02 = PlayerPrefs.HasKey("bonus_02") ? PlayerPrefs.GetInt("bonus_02") : 1;
        bonus_03 = PlayerPrefs.HasKey("bonus_03") ? PlayerPrefs.GetInt("bonus_03") : 1;
    }

    /// <summary>
    /// Сохранение времени получения бонуса 01
    /// </summary>
    public static void SetBonus_01_Time()
    {
        string currentTime = DateTime.Now.ToString();
        PlayerPrefs.SetString("bonus_01_time", currentTime);
        //PlayerPrefs.Save();
    }
    /// <summary>
    /// Сохранение времени получения бонуса 02
    /// </summary>
    public static void SetBonus_02_Time()
    {
        string currentTime = DateTime.Now.ToString();
        PlayerPrefs.SetString("bonus_02_time", currentTime);
        //PlayerPrefs.Save();
    }
    public static void SetBonus_03_Time()
    {
        string currentTime = DateTime.Now.ToString();
        PlayerPrefs.SetString("bonus_03_time", currentTime);
        //PlayerPrefs.Save();
    }
    /// <summary>
    /// Загрузка времени получения бонусов
    /// </summary>
    /// <param name="bonus_01_time">время получения бонуса 01</param>
    /// <param name="bonus_02_time">время получения бонуса 02</param>
    /// <param name="bonus_03_time">время получения бонуса 03</param>
    public static void GetBonus_Time(out DateTime bonus_01_time, out DateTime bonus_02_time, out DateTime bonus_03_time)
    {
        if (PlayerPrefs.HasKey("bonus_01_time"))
        {
            string savedTimeString_01 = PlayerPrefs.GetString("bonus_01_time");
            bonus_01_time = DateTime.Parse(savedTimeString_01);
        }
        else
        {
            bonus_01_time = new DateTime(2000, 1, 1, 0, 0, 0);
        }

        if (PlayerPrefs.HasKey("bonus_02_time"))
        {
            string savedTimeString_02 = PlayerPrefs.GetString("bonus_02_time");
            bonus_02_time = DateTime.Parse(savedTimeString_02);
        }
        else
        {
            bonus_02_time = new DateTime(2000, 1, 1, 0, 0, 0);
        }

        if (PlayerPrefs.HasKey("bonus_03_time"))
        {
            string savedTimeString_03 = PlayerPrefs.GetString("bonus_03_time");
            bonus_03_time = DateTime.Parse(savedTimeString_03);
        }
        else
        {
            bonus_03_time = new DateTime(2000, 1, 1, 0, 0, 0);
        }
    }
    // Метод для сохранения был ли взят второй шанс за рекламу
    public static void SetSecondChance(bool autoStart)
    {
        PlayerPrefs.SetInt("secondChance", autoStart ? 1 : 0); // Сохраняем 1 для true и 0 для false
        //PlayerPrefs.Save(); // Сохраняем изменения
    }
    // Метод для загрузки переменной автостарта из PlayerPrefs
    public static bool GetSecondChance()
    {
        int autoStartValue = PlayerPrefs.GetInt("secondChance", 0); // Получаем сохраненное значение, по умолчанию 0
        return autoStartValue == 1; // Если значение равно 1, возвращаем true, иначе false
    }

    public static void SetGameStarted(bool started)
    {
        PlayerPrefs.SetInt("gameStarted", started ? 1 : 0); // Сохраняем 1 для true и 0 для false
        //PlayerPrefs.Save(); // Сохраняем изменения
    }

    public static bool GetGameStarted()
    {
        int started = PlayerPrefs.GetInt("gameStarted", 0); // Получаем сохраненное значение, по умолчанию 0
        return started == 1; // Если значение равно 1, возвращаем true, иначе false
    }

    public static void SetSoundMute(bool started)
    {
        PlayerPrefs.SetInt("soundMute", started ? 1 : 0); // Сохраняем 1 для true и 0 для false
        //PlayerPrefs.Save(); // Сохраняем изменения
    }

    public static bool GetSoundMute()
    {
        int started = PlayerPrefs.GetInt("soundMute", 1); // Получаем сохраненное значение, по умолчанию 0
        return started == 1; // Если значение равно 1, возвращаем true, иначе false
    }

    public static void SetMusicMute(bool started)
    {
        PlayerPrefs.SetInt("musicMute", started ? 1 : 0); // Сохраняем 1 для true и 0 для false
        //PlayerPrefs.Save(); // Сохраняем изменения
    }

    public static bool GetMusicMute()
    {
        int started = PlayerPrefs.GetInt("musicMute", 0); // Получаем сохраненное значение, по умолчанию 0
        return started == 1; // Если значение равно 1, возвращаем true, иначе false
    }

    public static void SaveGame()
    {
        if (YandexGame.auth)
        {
            //YandexGame.savesData.score = PlayerPrefs.HasKey("score") ? PlayerPrefs.GetInt("score") : 0;
            YandexGame.savesData.maxscore = PlayerPrefs.HasKey("maxscore") ? PlayerPrefs.GetInt("maxscore") : 0;
            //YandexGame.savesData.maxwords = PlayerPrefs.HasKey("maxwords") ? PlayerPrefs.GetInt("maxwords") : 0;

            YandexGame.savesData.bonus_01 = PlayerPrefs.HasKey("bonus_01") ? PlayerPrefs.GetInt("bonus_01") : 1;
            YandexGame.savesData.bonus_02 = PlayerPrefs.HasKey("bonus_02") ? PlayerPrefs.GetInt("bonus_02") : 1;
            YandexGame.savesData.bonus_03 = PlayerPrefs.HasKey("bonus_03") ? PlayerPrefs.GetInt("bonus_03") : 1;

            //YandexGame.savesData.secondChance = GetSecondChance();
            //YandexGame.savesData.gameStarted = GetGameStarted();

            YandexGame.SaveProgress();

            //Debug.Log("Игра сохранена");
        }
    }
    public static void LoadGame()
    {
        if (YandexGame.auth)
        {
            YandexGame.LoadProgress();

            //PlayerPrefs.SetInt("score", Math.Max(PlayerPrefs.GetInt("score"), YandexGame.savesData.score));
            PlayerPrefs.SetInt("maxscore", Math.Max(PlayerPrefs.GetInt("maxscore"), YandexGame.savesData.maxscore));
            //PlayerPrefs.SetInt("maxwords", Math.Max(PlayerPrefs.GetInt("maxwords"), YandexGame.savesData.maxwords));

            PlayerPrefs.SetInt("bonus_01", Math.Max(PlayerPrefs.GetInt("bonus_01"), YandexGame.savesData.bonus_01));
            PlayerPrefs.SetInt("bonus_02", Math.Max(PlayerPrefs.GetInt("bonus_02"), YandexGame.savesData.bonus_02));
            PlayerPrefs.SetInt("bonus_03", Math.Max(PlayerPrefs.GetInt("bonus_03"), YandexGame.savesData.bonus_03));

            //SetSecondChance(YandexGame.savesData.secondChance);
            //SetGameStarted(YandexGame.savesData.gameStarted);

        }
    }
}