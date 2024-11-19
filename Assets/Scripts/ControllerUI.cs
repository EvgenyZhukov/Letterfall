using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;

public class ControllerUI : MonoBehaviour
{
    public AudioControllerScript audioControllerScript;
    public SaveLevelScript saveLevelScript;

    public AdsManager adsManager;
    public WordLine wordLine;
    public ScoreLine scoreLine;
    public GameField gameField;
    public BonusLogic bonusLogic;
    public LetterFallAnimator letterFallAnimator;
    public LetterBoxSpawner letterBoxSpawner;

    public GameObject buttonLike;
    public GameObject defeatScreen;
    public GameObject mainScreen;
    public GameObject gameDesk;

    public GameObject panelLockUI;

    public GameObject mainPage;
    public GameObject leaderListPage;
    public GameObject helpPage;
    public GameObject boostersPage;

    public GameObject buttonSecondChance;

    public GameObject fallAnimator;

    public GameObject scoreLB;
    public GameObject scoreAnonLB;

    public AudioMixerGroup soundMixerGroup; // Ссылка на группу миксеров для звуковых эффектов
    public AudioMixerGroup musicMixerGroup; // Ссылка на группу миксеров для музыки
    public Image soundButtonImage; // Ссылка на изображение для кнопки звука
    public Image musicButtonImage; // Ссылка на изображение для кнопки музыки
    public Sprite soundOnSprite; // Спрайт для включенного звука
    public Sprite soundOffSprite; // Спрайт для выключенного звука
    public Sprite musicOnSprite; // Спрайт для включенной музыки
    public Sprite musicOffSprite; // Спрайт для выключенной музыки

    public TMP_Text bonus_01_amount;
    public TMP_Text bonus_02_amount;
    public TMP_Text bonus_03_amount;

    //public TMP_Text textLB;

    public TMP_Text buttonBonus_03;
    public TMP_Text buttonContinuousGame;

    //public LeaderboardYG leaderboardYG_score;
    //public LeaderboardYG leaderboardYG_scoreAnon;

    //private string scoreText = "макс. очки";
    //private string wordsText = "всего слов";
    private string buttonBonus_03_text = "за рекламу";
    private string buttonContinuousGame_text = "продолжить";
    private string buttonAds_error_text = "ошибка";

    public GameObject backButtonBonusScreen;
    public GameObject startButtonBonusScreen;

    public int bonusAvailable;
    public TMP_Text bonusAvailableText;

    int helpSwitch = 0;

    public GameObject image_1;
    public GameObject image_2;
    public GameObject image_3;
    public GameObject image_4;

    float currentVolumeSound;
    bool isMutedSound;
    float currentVolumeMusic;
    bool isMutedMusic;

    public Button greenButton;

    public Text scoreText;

    private void Awake()
    {
        PlayerPrefsMethods.LoadGame();
    }

    private void Start()
    {
        CheckAuth();
        //ToggleMusic();
        CheckSoundAndMusic();
        //Debug.Log(PlayerPrefsMethods.GetScore());
        //Debug.Log(PlayerPrefsMethods.GetScoreWords());

        //AutostartCheck();

        //PlayerPrefsMethods.SetSecondChance(false);

        LoadingGame();

        LeaderBoardSetter();
        //Debug.Log(PlayerPrefsMethods.GetSecondChance());

    }
    private void Update()
    {
        LockerUI();
        BonusAmount();
        BonusAvailable();
    }
    public void ButtonAccept()
    {
        //audioControllerScript.soundClick.Play();
        greenButton.interactable = false;
        // Вызываем метод из компонента WordLine при нажатии на кнопку
        if (wordLine != null)
        {
            // Пример вызова метода из WordLine
            wordLine.AcceptWord();
        }
    }
    public void ButtonCancel()
    {
        //audioControllerScript.soundClick.Play();

        // Вызываем метод из компонента WordLine при нажатии на кнопку
        if (wordLine != null)
        {
            // Пример вызова метода из WordLine
            wordLine.CancelWord();
        }
    }
    public void CheckSoundAndMusic()
    {
        soundMixerGroup.audioMixer.GetFloat("VolumeSound", out currentVolumeSound);
        musicMixerGroup.audioMixer.GetFloat("VolumeMusic", out currentVolumeMusic);

        if (PlayerPrefsMethods.GetSoundMute())
        {
            isMutedSound = true;
        }
        else
        {
            isMutedSound = false;
        }
        if (PlayerPrefsMethods.GetMusicMute())
        {
            isMutedMusic = true;
        }
        else
        {
            isMutedMusic = false;
        }

        soundMixerGroup.audioMixer.SetFloat("VolumeSound", isMutedSound ? 0f : -80f);
        soundButtonImage.sprite = isMutedSound ? soundOnSprite : soundOffSprite;

        musicMixerGroup.audioMixer.SetFloat("VolumeMusic", isMutedMusic ? 0f : -80f);
        musicButtonImage.sprite = isMutedMusic ? musicOnSprite : musicOffSprite;
    }
        // Метод для выключения звука
        public void ToggleSound()
        {
        SoundClick();

        // Получаем текущее значение громкости
        soundMixerGroup.audioMixer.GetFloat("VolumeSound", out currentVolumeSound);

        // Проверяем, выключен ли звук
        if (currentVolumeSound == -80f)
        {
            isMutedSound = true;
            PlayerPrefsMethods.SetSoundMute(true);
        }
        else
        {
            isMutedSound = false;
            PlayerPrefsMethods.SetSoundMute(false);
        }

        // Устанавливаем новое значение громкости в зависимости от того, был ли звук выключен
        soundMixerGroup.audioMixer.SetFloat("VolumeSound", isMutedSound ? 0f : -80f); 

        // Изменяем спрайт кнопки звука в соответствии с состоянием звука
        soundButtonImage.sprite = isMutedSound ? soundOnSprite : soundOffSprite;

        SoundClick();

        }
    // Метод для выключения музыки
    public void ToggleMusic()
    {

        // Получаем текущее значение громкости
        musicMixerGroup.audioMixer.GetFloat("VolumeMusic", out currentVolumeMusic);

        // Проверяем, выключена ли музыка
        if (currentVolumeMusic == -80f)
        {
            isMutedMusic = true;
            PlayerPrefsMethods.SetMusicMute(true);
        }
        else
        {
            isMutedMusic = false;
            PlayerPrefsMethods.SetMusicMute(false);
        }

        // Устанавливаем новое значение громкости в зависимости от того, была ли музыка выключена
        musicMixerGroup.audioMixer.SetFloat("VolumeMusic", isMutedMusic ? 0f : -80f); 

        // Изменяем спрайт кнопки музыки в соответствии с состоянием музыки
        musicButtonImage.sprite = isMutedMusic ? musicOnSprite : musicOffSprite;
    }
    public void GoToMain()
    {
        SoundClick();

        mainScreen.SetActive(true); // Активируем главный экран
        fallAnimator.SetActive(true);
        letterFallAnimator.ActivateFalling();

        boostersPage.SetActive(false);
        mainPage.SetActive(true);

        leaderListPage.SetActive(false);
    }
    public void OpenBonusScreen()
    {
        SoundClick();

        mainScreen.SetActive(true); // Активируем главный экран
        mainPage.SetActive(false);
        boostersPage.SetActive(true);

        startButtonBonusScreen.SetActive(true);
        backButtonBonusScreen.SetActive(false);

        //adsManager.StartFullscreenAd();
    }
    public void StartGame()
    {
        PlayerPrefsMethods.SaveGame();

        PlayerPrefsMethods.SetGameStarted(true);

        //SoundClick();

        //letterBoxSpawner.spawning = true;
        mainScreen.SetActive(false);
        gameDesk.SetActive(true);
        fallAnimator.SetActive(false);

    }
    public void LeaderBoard()
    {
        //leaderboardYG_score.UpdateLB();
        SoundClick();

        mainPage.SetActive(false);
        leaderListPage.SetActive(true);
        //leaderboardYG_words.UpdateLB();
        //leaderboardYG_score.UpdateLB();

        if (!YandexGame.auth)
        {
            scoreText.text = PlayerPrefsMethods.GetMaxScoreAnon().ToString();

            //leaderboardYG_scoreAnon.players = leaderboardYG_score.players;
            //leaderboardYG_scoreAnon.NewScore(PlayerPrefsMethods.GetMaxScoreAnon());
        }
    }
    public void Help()
    {
        
        SoundClick();

        mainPage.SetActive(false);
        helpPage.SetActive(true);
        

        /*
        StartGame();
        image_1.SetActive(true);
        */
    }
    public void GetBonus()
    {
        //SoundClick();

        mainPage.SetActive(false);
        boostersPage.SetActive(true);

        startButtonBonusScreen.SetActive(false);
        backButtonBonusScreen.SetActive(true);

        //adsManager.StartFullscreenAd();
    }
    public void Back()
    {
        SoundClick();

        leaderListPage.SetActive(false);
        helpPage.SetActive(false);
        boostersPage.SetActive(false);
        mainPage.SetActive(true);
    }

    public void Next()
    {
        switch (helpSwitch)
        {
            case 0:
                image_1.SetActive(false);
                image_2.SetActive(true);
                helpSwitch++;
                break;
            case 1:
                image_2.SetActive(false);
                image_3.SetActive(true);
                helpSwitch++;
                break;
            case 2:
                image_3.SetActive(false);
                image_4.SetActive(true);
                helpSwitch++;
                break;
            case 3:
                image_4.SetActive(false);
                image_1.SetActive(true);
                Back();
                helpSwitch = 0;
                break;
        }
    }

    public void RestartGame()
    {
        gameField.defeat = false;
        greenButton.interactable = true;

        PlayerPrefsMethods.SetSecondChance(false);
        PlayerPrefsMethods.SetGameStarted(true);

        SoundClick();

        gameField.ClearField();

        scoreLine.score = 0;
        PlayerPrefsMethods.SetScore(0);
        scoreLine.UpdateScoreText(scoreLine.score);

        adsManager.StartFullscreenAd();

        defeatScreen.SetActive(false);

        letterBoxSpawner.spawning = true;
        //PlayerPrefsMethods.SaveAutoStart(true);

        PlayerPrefsMethods.SaveGame();
    }
    public void WatchAd()
    {
        gameField.defeat = false;
        greenButton.interactable = true;

        //audioControllerScript.soundClick.Play();

        adsManager.StartRewardAd(1);
    }
    public void ButtonLike()
    {
        audioControllerScript.soundLikeButton.Play();

        YandexGame.ReviewShow(true);
    }
    /*
    public void ChangeLB()
    {
        SoundClick();

        if (scoreLB.activeSelf)
        {
            scoreLB.SetActive(false);
            wordsLB.SetActive(true);
            textLB.text = wordsText;
        }
        else 
        {
            scoreLB.SetActive(true);
            wordsLB.SetActive(false);
            textLB.text = scoreText;
        }
    }
    */
    /*
    private void AutostartCheck()
    {
        if (PlayerPrefsMethods.LoadAutoStart())
        {
            PlayerPrefsMethods.SaveAutoStart(false);
            StartGame();
        }
    }
    */
    public void ButtonBonus_01()
    {

        if (bonusLogic.bonus_01 > 0)
        {
            bonusLogic.Bonus_01();
        }
    }
    public void ButtonBonus_02()
    {

        if (bonusLogic.bonus_02 > 0)
        {
            bonusLogic.Bonus_02();
        }
    }
    public void ButtonBonus_03()
    {

        if (bonusLogic.bonus_03 > 0)
        {
            bonusLogic.Bonus_03();
        }
    }
    public void TakeButtonBonus_01()
    {

        if (bonusLogic.readyBonus_01)
        {
            bonusLogic.bonus_01++;
            PlayerPrefsMethods.SetBonusAmount(bonusLogic.bonus_01, bonusLogic.bonus_02, bonusLogic.bonus_03);

            PlayerPrefsMethods.SetBonus_01_Time();

            audioControllerScript.soundBonusTaked.Play();
        }
    }
    public void TakeButtonBonus_02()
    {

        if (bonusLogic.readyBonus_02)
        {
            bonusLogic.bonus_02++;
            PlayerPrefsMethods.SetBonusAmount(bonusLogic.bonus_01, bonusLogic.bonus_02, bonusLogic.bonus_03);

            PlayerPrefsMethods.SetBonus_02_Time();
            audioControllerScript.soundBonusTaked.Play();
        }
    }
    public void TakeButtonBonus_03()
    {
        if (bonusLogic.readyBonus_03)
        {
            adsManager.StartRewardAd(2);
        }
    }

    void LockerUI()
    {
        if (letterBoxSpawner.spawning)
        {
            panelLockUI.SetActive(true);
        }
        else
        {
            panelLockUI.SetActive(false);
        }
    }

    void BonusAmount()
    {
        if (bonusLogic.bonus_01 <= 9)
        {
            bonus_01_amount.text = "×" + bonusLogic.bonus_01.ToString();
        }
        else
        {
            bonus_02_amount.text = "×9+";
        }

        if (bonusLogic.bonus_02 <= 9)
        {
            bonus_02_amount.text = "×" + bonusLogic.bonus_02.ToString();
        }
        else
        {
            bonus_02_amount.text = "×9+";
        }

        if (bonusLogic.bonus_03 <= 9)
        {
            bonus_03_amount.text = "×" + bonusLogic.bonus_03.ToString();
        }
        else
        {
            bonus_03_amount.text = "×9+";
        }
    }
    void BonusAvailable()
    {
        switch (bonusAvailable)
        {
            case 0:
                bonusAvailableText.text = "×0";
                break;
            case 1:
                bonusAvailableText.text = "×1";
                break;
            case 2:
                bonusAvailableText.text = "×2";
                break;
            case 3:
                bonusAvailableText.text = "×3";
                break;
        }
    }
        public void RewardAdError()
    {
        audioControllerScript.soundError.Play();

        buttonBonus_03.text = buttonAds_error_text;
        buttonContinuousGame.text = buttonAds_error_text;

        Invoke("RewardAdErrorRefreshText", 1.2f);
    }

    void RewardAdErrorRefreshText()
    {
        buttonBonus_03.text = buttonBonus_03_text;
        buttonContinuousGame.text = buttonContinuousGame_text;
    }


    void SoundClick()
    {
        audioControllerScript.soundClick.Play();
    }

    void CheckAuth() 
    { 
        if (YandexGame.auth)
        {
            buttonLike.SetActive(true);
        }
        else
        {
            buttonLike.SetActive(false);
        }
    }

    void LoadingGame()
    {
        //PlayerPrefsMethods.SetGameStarted(false);
        if (PlayerPrefsMethods.GetGameStarted())
        {
            saveLevelScript.LoadLevel();
            StartGame();

            //letterBoxSpawner.loadSavedGame = true;
            scoreLine.score = PlayerPrefsMethods.GetScore();
        }
        else
        {
            PlayerPrefsMethods.SetScore(0);
        }
    }

    void LeaderBoardSetter()
    {
        if (!YandexGame.auth)
        {
            scoreLB.SetActive(false);
            scoreAnonLB.SetActive(true);

            //leaderboardYG_scoreAnon.players = leaderboardYG_score.players;
            //leaderboardYG_scoreAnon.NewScore(PlayerPrefsMethods.GetMaxScoreAnon());
        }
        else
        {
            if (PlayerPrefsMethods.GetMaxScoreAnon() > 0)
            {
                YandexGame.NewLeaderboardScores("maxScore", Mathf.Max(PlayerPrefsMethods.GetMaxScoreAnon(), PlayerPrefsMethods.GetMaxScore()));
            }

            scoreLB.SetActive(true);
            scoreAnonLB.SetActive(false);
        }
    }
}
