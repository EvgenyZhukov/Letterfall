using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxSpawner : MonoBehaviour
{
    public ControllerUI controllerUI;
    public SaveLevelScript saveLevelScript;
    public GameField gameField;
    public Transform[] spawnPoints;
    public GameObject prefabToSpawn;
    public int objectsToSpawn = 10;
    private float spawnDelayBetweenPoints = 0.15f;
    private float spawnDelaySamePoint = 0.5f;
    public bool spawning;
    private Coroutine spawnCoroutine;

    //public bool loadSavedGame;

    // Словарь для хранения количества созданных объектов на каждой линии спауна
    private Dictionary<Transform, int> objectsCountOnLine = new Dictionary<Transform, int>();

    void Start()
    {
        GameObject letterBoxes = GameObject.FindWithTag("LetterBox");
        if (letterBoxes == null)
        {
            //Debug.Log("Пусто на игровом поле!");
            spawning = true;
        }
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    /// <summary>
    /// Метод, отвечающий за спаун объектов
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (spawning)
            {
                // Сбрасываем счетчики перед каждой волной спауна
                ResetObjectCount();

                    // Перебираем объекты для спауна
                    for (int i = 0; i < objectsToSpawn; i++)
                    {
                        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

                        while (availableSpawnPoints.Count > 0)
                        {
                            int randomPointIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
                            Transform selectedSpawnPoint = availableSpawnPoints[randomPointIndex];

                            // Проверка соответствия номера спаунера фактической линии спауна
                            //Debug.Log("Random point index: " + randomPointIndex);

                            int objectsCount = GetObjectsCountOnLine(selectedSpawnPoint);

                            if (objectsCount < 2)
                            {
                                // Присвоение номера спаунера объекту LetterBox
                                float delay = objectsCount >= 1 ? spawnDelaySamePoint : spawnDelayBetweenPoints;
                                yield return new WaitForSeconds(delay);

                                GameObject newLetter = Instantiate(prefabToSpawn, selectedSpawnPoint.position, Quaternion.identity, transform);
                                LetterBox letterBox = newLetter.GetComponent<LetterBox>();
                                if (letterBox != null)
                                {
                                    // Передаем номер спаунера объекту LetterBox
                                    letterBox.spawnerNumber = Array.IndexOf(spawnPoints, selectedSpawnPoint); // Используем индекс точки спауна в массиве
                                }
                                AddObjectToLine(selectedSpawnPoint);
                                break; // Прерываем цикл, чтобы создать только один объект на текущей точке
                            }
                            else
                            {
                                availableSpawnPoints.RemoveAt(randomPointIndex);
                            }
                        }
                    }
                    gameField.CheckLines();
                    yield return new WaitForSeconds(1f);

                    saveLevelScript.SaveLevel();
                    PlayerPrefsMethods.SaveGame();
                
                gameField.CheckLines(); // Проверяем наполнение линии после волны спауна

                yield return new WaitForSeconds(0.25f);
                if (!gameField.defeat)
                {
                    controllerUI.greenButton.interactable = true;
                }
                spawning = false; // После завершения спауна отключаем флаг спауна
            }
            yield return null; // Ждем следующего кадра
        }
    }

    // Получить количество объектов на линии спауна
    private int GetObjectsCountOnLine(Transform spawnPoint)
    {
        if (objectsCountOnLine.ContainsKey(spawnPoint))
        {
            return objectsCountOnLine[spawnPoint];
        }
        else
        {
            return 0;
        }
    }

    // Увеличить счетчик объектов на линии спауна
    private void AddObjectToLine(Transform spawnPoint)
    {
        if (objectsCountOnLine.ContainsKey(spawnPoint))
        {
            objectsCountOnLine[spawnPoint]++;
        }
        else
        {
            objectsCountOnLine.Add(spawnPoint, 1);
        }
    }

    // Сбросить счетчики объектов на линиях спауна
    private void ResetObjectCount()
    {
        objectsCountOnLine.Clear();
    }
}