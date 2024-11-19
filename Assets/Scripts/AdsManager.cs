using UnityEngine;
using UnityEngine.UIElements;
using YG;

public class AdsManager : MonoBehaviour
{
    public AudioControllerScript audioControllerScript;
    public ControllerUI controllerUI;
    public BonusLogic bonusLogic;
    public SaveLevelScript saveLevelScript;

    public void StartRewardAd(int typeCase)
    {
        YandexGame.RewardVideoEvent += OnAdWatched;
        YandexGame.ErrorVideoEvent += OnAdFail;

        PauseAndMute();

        YandexGame.RewVideoShow(typeCase);
    }

    /// <summary>
    /// Метод, который вызывается после фейла просмотра рекламы
    /// </summary>
    private void OnAdFail()
    {

        YandexGame.RewardVideoEvent -= OnAdWatched;
        YandexGame.ErrorVideoEvent -= OnAdFail;

        controllerUI.RewardAdError();
    }
    /// <summary>
    /// Метод, который вызывается после успешного просмотра рекламы
    /// </summary>
    private void OnAdWatched(int id)
    {
        YandexGame.RewardVideoEvent -= OnAdWatched;
        YandexGame.ErrorVideoEvent -= OnAdFail;

        UnpauseAndUnmute();

        if (id == 1)    //продолжение игры
        {

            controllerUI.defeatScreen.SetActive(false);
            bonusLogic.bonus_02++;
            PlayerPrefsMethods.SetBonusAmount(bonusLogic.bonus_01, bonusLogic.bonus_02, bonusLogic.bonus_03);

            controllerUI.bonusLogic.Bonus_02();

            PlayerPrefsMethods.SetSecondChance(true);
            PlayerPrefsMethods.SetGameStarted(true);

            Invoke("SaveAfterAd", 1.5f);
        }
        if (id == 2)    //получение бонуса 3
        {
            bonusLogic.bonus_03++;
            PlayerPrefsMethods.SetBonusAmount(bonusLogic.bonus_01, bonusLogic.bonus_02, bonusLogic.bonus_03);

            //PlayerPrefsMethods.SaveGame();

            PlayerPrefsMethods.SetBonus_03_Time();
            audioControllerScript.soundBonusTaked.Play();
        }
    }

    public void StartFullscreenAd()
    {
        YandexGame.ErrorFullAdEvent += ReloadSceneAfterAd;
        YandexGame.CloseFullAdEvent += ReloadSceneAfterAd;

        PauseAndMute();

        YandexGame.FullscreenShow();
    }

    public void ReloadSceneAfterAd()
    {
        UnpauseAndUnmute();
        YandexGame.CloseFullAdEvent -= ReloadSceneAfterAd;
        YandexGame.ErrorFullAdEvent -= ReloadSceneAfterAd;
    }

    void PauseAndMute()
    {

    }
    void UnpauseAndUnmute()
    {

    }

    void SaveAfterAd()
    {
        saveLevelScript.SaveLevel();
        PlayerPrefsMethods.SaveGame();
    }
}
