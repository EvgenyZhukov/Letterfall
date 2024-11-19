using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LetterRandomizer
{
    private static Dictionary<char, float> letterProbabilities = new Dictionary<char, float>
    {
        {'�', 0.0834f},
        {'�', 0.0793f},
        {'�', 0.0723f},
        {'�', 0.0671f},
        {'�', 0.0648f},
        {'�', 0.0617f},
        {'�', 0.0522f},
        {'�', 0.0495f},
        {'�', 0.0447f},
        {'�', 0.0417f},
        {'�', 0.0335f},
        {'�', 0.0397f},
        {'�', 0.0393f},
        {'�', 0.0386f},
        {'�', 0.0239f},
        {'�', 0.0217f},
        {'�', 0.0209f},
        {'�', 0.019f},
        {'�', 0.02811f},
        {'�', 0.0277f},
        {'�', 0.0167f},
        {'�', 0.0165f},
        {'�', 0.0114f},
        {'�', 0.0109f},
        {'�', 0.0089f},
        {'�', 0.0079f},
        {'�', 0.0066f},
        {'�', 0.0033f},
        {'�', 0.0029f},
        {'�', 0.0029f},
        {'�', 0.001f},
        {'�', 0.0002f}
    };

    public static char GetRandomLetter()
    {
        // ����������� �����������
        float totalProbability = letterProbabilities.Sum(x => x.Value);
        foreach (var key in letterProbabilities.Keys.ToList())
        {
            letterProbabilities[key] /= totalProbability;
        }

        // ��������� ��������� �������������� �����������, ����� ����� ���� ����� ����� 1
        float additionalProbability = 1.0f - letterProbabilities.Sum(x => x.Value);
        letterProbabilities[letterProbabilities.Keys.Last()] += additionalProbability;

        // ��������� ������� �������� ����� � ������ ������������
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

        // ���� ���-�� ����� �� ���, ������� ������ ��������� �����
        return letterProbabilities.Keys.ElementAt(Random.Range(0, letterProbabilities.Count));
    }
}