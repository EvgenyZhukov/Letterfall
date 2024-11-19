using UnityEngine;

public class DifficultManager : MonoBehaviour
{
    public LetterBoxSpawner letterBoxSpawner;
    public GameField gameField;

    public int turn = 0;

    public int lettersCount;
    private int mistakeLetterCount = 10;

    private int minLetersOnField = 16;

    public void SpawnWave(bool success)
    {
        if (success) 
        {
            letterBoxSpawner.objectsToSpawn = CalculateLettersSpawnCount();
        }
        else
        {
            letterBoxSpawner.objectsToSpawn = mistakeLetterCount;
        }
        turn++;
        letterBoxSpawner.spawning = true;
    }

    public void SpawnWave(int number)
    {
        letterBoxSpawner.objectsToSpawn = number;
        letterBoxSpawner.spawning = true;
    }

    public int CalculateLettersSpawnCount()
    {
        int result;
        lettersCount = gameField.GetTotalObjectsCount(); //Получает количество букв на поле

        if (lettersCount < minLetersOnField)             //если на поле слишком мало букв, то добавляем до минимального количества
        {
            result = minLetersOnField - lettersCount;

            if (result < Mathf.CeilToInt(turn / 3f))
            {
                result = Mathf.CeilToInt(turn / 3f);
            }
        }
        else
        {
            result = Mathf.CeilToInt(turn / 3f);
        }

        result = Mathf.Clamp(result, 1, 9);
        return result;
    }
}
