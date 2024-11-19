using TMPro;
using UnityEngine;

public class TextLine : MonoBehaviour
{
    public TextAnimator textAnimator;

    public TMP_Text textLine;

    private string greetings = "начинаем!";

    private string bonus_01 = "замена букв!";
    private string bonus_02 = "ба-бах!!!";
    private string bonus_03 = "думаю...";

    private string score_11 = "неплохо! ×1.1";
    private string score_125 = "хорошо! ×1.25";
    private string score_15 = "отлично! ×1.5";
    private string score_2 = "замечательно! ×2";
    private string score_3 = "превосходно! ×3";

    private string defeat = "переполнено!";

    private string mistake = "не знаю этого...";

    private string bonus_03_error = "у меня нет слов";


    private string moreLetters = "новые буквы!";

    public void ShowText(int textSwitch)
    {
        switch (textSwitch)
        {
            case 0: textLine.text = greetings; break;

            case 1: textLine.text = bonus_01; break;
            case 2: textLine.text = bonus_02; break;
            case 3: textLine.text = bonus_03; break;

            case 4: textLine.text = score_11; break;
            case 5: textLine.text = score_125; break;
            case 6: textLine.text = score_15; break;
            case 7: textLine.text = score_2; break;
            case 8: textLine.text = score_3; break;

            case 9: textLine.text = defeat; break;

            case 10: textLine.text = mistake; break;

            case 11: textLine.text = bonus_03_error; break;

            case 12: textLine.text = moreLetters; break;
            default: break;
        }
        textAnimator.isAnimating = true;
    }
}
