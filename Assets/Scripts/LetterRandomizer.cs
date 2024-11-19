using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LetterRandomizer
{
    private static Dictionary<char, float> letterProbabilities = new Dictionary<char, float>
    {
        {'О', 0.0834f},
        {'Е', 0.0793f},
        {'А', 0.0723f},
        {'Н', 0.0671f},
        {'И', 0.0648f},
        {'Т', 0.0617f},
        {'С', 0.0522f},
        {'Л', 0.0495f},
        {'В', 0.0447f},
        {'Р', 0.0417f},
        {'К', 0.0335f},
        {'Д', 0.0397f},
        {'М', 0.0393f},
        {'У', 0.0386f},
        {'П', 0.0239f},
        {'Я', 0.0217f},
        {'Ь', 0.0209f},
        {'Ы', 0.019f},
        {'Г', 0.02811f},
        {'Б', 0.0277f},
        {'Ч', 0.0167f},
        {'З', 0.0165f},
        {'Ж', 0.0114f},
        {'Й', 0.0109f},
        {'Ш', 0.0089f},
        {'Х', 0.0079f},
        {'Ю', 0.0066f},
        {'Э', 0.0033f},
        {'Ц', 0.0029f},
        {'Щ', 0.0029f},
        {'Ф', 0.001f},
        {'Ъ', 0.0002f}
    };

    public static char GetRandomLetter()
    {
        // Нормализуем вероятности
        float totalProbability = letterProbabilities.Sum(x => x.Value);
        foreach (var key in letterProbabilities.Keys.ToList())
        {
            letterProbabilities[key] /= totalProbability;
        }

        // Добавляем небольшую дополнительную вероятность, чтобы сумма была точно равна 1
        float additionalProbability = 1.0f - letterProbabilities.Sum(x => x.Value);
        letterProbabilities[letterProbabilities.Keys.Last()] += additionalProbability;

        // Случайным образом выбираем букву с учетом вероятностей
        float randomValue = Random.value;
        float cumulativeProbability = 0;
        foreach (var kvp in letterProbabilities)
        {
            cumulativeProbability += kvp.Value;
            if (randomValue <= cumulativeProbability)
            {
                return kvp.Key;
            }
        }

        // Если что-то пошло не так, вернуть просто случайную букву
        return letterProbabilities.Keys.ElementAt(Random.Range(0, letterProbabilities.Count));
    }
}