using UnityEngine;
using System.Collections.Generic;

public class SaveLevelScript : MonoBehaviour
{
    public LetterBoxSpawner letterBoxSpawner;

    public class LevelData
    {
        public List<Vector3> positions = new List<Vector3>();
        public List<char> letters = new List<char>();
        public List<int> spawnerNumbers = new List<int>();
    }

    LevelData levelData = new LevelData();

    /// <summary>
    /// Сохранение данных объектов уровня
    /// </summary>
    public void SaveLevel()
    {
        // Находим все объекты с тегом "LetterBox"
        GameObject[] letterBoxes = GameObject.FindGameObjectsWithTag("LetterBox");

        // Очищаем текущие данные
        levelData.positions.Clear();
        levelData.letters.Clear();
        levelData.spawnerNumbers.Clear();

        // Инициализируем массивы в зависимости от количества найденных объектов
        foreach (GameObject letterGameObject in letterBoxes)
        {
            if (letterGameObject != null)
            {
                LetterBox letterBoxComponent = letterGameObject.GetComponent<LetterBox>();

                if (letterBoxComponent != null)
                {
                    levelData.positions.Add(letterGameObject.transform.position);
                    levelData.letters.Add(char.ToLower(letterBoxComponent.letter));
                    levelData.spawnerNumbers.Add(letterBoxComponent.spawnerNumber);
                }
            }
        }

        // Сохранение данных в PlayerPrefs
        PlayerPrefs.SetInt("LevelData_Count", levelData.positions.Count);

        for (int i = 0; i < levelData.positions.Count; i++)
        {
            string prefix = "LevelData_" + i + "_";
            PlayerPrefs.SetFloat(prefix + "PosX", levelData.positions[i].x);
            PlayerPrefs.SetFloat(prefix + "PosY", levelData.positions[i].y);
            PlayerPrefs.SetFloat(prefix + "PosZ", levelData.positions[i].z);
            PlayerPrefs.SetString(prefix + "Letter", levelData.letters[i].ToString());
            PlayerPrefs.SetInt(prefix + "SpawnerNumber", levelData.spawnerNumbers[i]);
        }

        PlayerPrefs.Save();

        //Debug.Log("Level saved!");
    }

    /// <summary>
    /// Загрузка данных объектов уровня
    /// </summary>
    public void LoadLevel()
    {
        int count = PlayerPrefs.GetInt("LevelData_Count", 0);
        if (count == 0)
        {
            //Debug.LogWarning("No saved level data found!");
            return;
        }

        levelData.positions.Clear();
        levelData.letters.Clear();
        levelData.spawnerNumbers.Clear();

        for (int i = 0; i < count; i++)
        {
            string prefix = "LevelData_" + i + "_";
            float posX = PlayerPrefs.GetFloat(prefix + "PosX");
            float posY = PlayerPrefs.GetFloat(prefix + "PosY");
            float posZ = PlayerPrefs.GetFloat(prefix + "PosZ");
            levelData.positions.Add(new Vector3(posX, posY, posZ));
            levelData.letters.Add(PlayerPrefs.GetString(prefix + "Letter")[0]);
            levelData.spawnerNumbers.Add(PlayerPrefs.GetInt(prefix + "SpawnerNumber"));
        }

        for (int i = 0; i < levelData.positions.Count; i++)
        {
            // Создаем новый объект на сохраненной позиции
            Vector3 position = levelData.positions[i];
            GameObject newLetter = Instantiate(letterBoxSpawner.prefabToSpawn, position, Quaternion.identity, transform);

            // Устанавливаем букву для объекта
            LetterBox letterBox = newLetter.GetComponent<LetterBox>();
            if (letterBox != null)
            {
                letterBox.letter = char.ToUpper(levelData.letters[i]);
                letterBox.creatSoundLocker = true;
                letterBox.letterSelected = true;
                letterBox.spawnerNumber = levelData.spawnerNumbers[i];
            }
        }

        //Debug.Log("Level loaded!");
    }
}