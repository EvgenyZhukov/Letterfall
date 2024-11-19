using System;
using TMPro;
using UnityEngine;

public class BonusTimerUI : MonoBehaviour
{
    public TMP_Text timerText_bonus_01;
    public TMP_Text timerText_bonus_02;
    public TMP_Text timerText_bonus_03;
    public BonusLogic bonusLogic;
    private string takeBonusText = "забрать";
    private string takeForAdsBonusText = "за рекламу";

    void Start()
    {
        UpdateTimerText();
    }

    void Update()
    {
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        DateTime specificDate = new DateTime(2000, 1, 1, 0, 0, 0);

        PlayerPrefsMethods.GetBonus_Time(out DateTime lastUseTime_01, out DateTime lastUseTime_02, out DateTime lastUseTime_03);

        TimeSpan timeSinceLastUse_01 = DateTime.Now - lastUseTime_01;
        TimeSpan bonusCooldown_01 = TimeSpan.FromMinutes(1);

        if (timeSinceLastUse_01 >= bonusCooldown_01)
        {
            bonusLogic.readyBonus_01 = true;
            timerText_bonus_01.text = takeBonusText;
        }
        else if (lastUseTime_01 == specificDate)
        {
            bonusLogic.readyBonus_01 = true;
            timerText_bonus_01.text = takeBonusText;
        }
        else
        {
            TimeSpan timeRemaining = bonusCooldown_01 - timeSinceLastUse_01;
            timerText_bonus_01.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
            bonusLogic.readyBonus_01 = false;
        }


        TimeSpan timeSinceLastUse_02 = DateTime.Now - lastUseTime_02;
        TimeSpan bonusCooldown_02 = TimeSpan.FromMinutes(5);

        if (timeSinceLastUse_02 >= bonusCooldown_02)
        {
            bonusLogic.readyBonus_02 = true;
            timerText_bonus_02.text = takeBonusText;
        }
        else if (lastUseTime_02 == specificDate)
        {
            bonusLogic.readyBonus_02 = true;
            timerText_bonus_02.text = takeBonusText;
        }
        else
        {
            TimeSpan timeRemaining = bonusCooldown_02 - timeSinceLastUse_02;
            timerText_bonus_02.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
            bonusLogic.readyBonus_02 = false;
        }


        TimeSpan timeSinceLastUse_03 = DateTime.Now - lastUseTime_03;
        TimeSpan bonusCooldown_03 = TimeSpan.FromMinutes(10);

        if (timeSinceLastUse_03 >= bonusCooldown_03)
        {
            bonusLogic.readyBonus_03 = true;
            timerText_bonus_03.text = takeForAdsBonusText;
        }
        else if (lastUseTime_03 == specificDate)
        {
            bonusLogic.readyBonus_03 = true;
            timerText_bonus_03.text = takeForAdsBonusText;
        }
        else
        {
            TimeSpan timeRemaining = bonusCooldown_03 - timeSinceLastUse_03;
            timerText_bonus_03.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
            bonusLogic.readyBonus_03 = false;
        }
    }
}