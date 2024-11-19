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

    // ������� ��� �������� ���������� ��������� �������� �� ������ ����� ������
    private Dictionary<Transform, int> objectsCountOnLine = new Dictionary<Transform, int>();

    void Start()
    {
        GameObject letterBoxes = GameObject.FindWithTag("LetterBox");
        if (letterBoxes == null)
        {
            //Debug.Log("����� �� ������� ����!");
            spawning = true;
        }
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    /// <summary>
    /// �����, ���������� �� ����� ��������
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (spawning)
            {
                // ���������� �������� ����� ������ ������ ������
                ResetObjectCount();

                    // ���������� ������� ��� ������
                    for (int i = 0; i < objectsToSpawn; i++)
                    {
                        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

                        while (availableSpawnPoints.Count > 0)
                        {
                            int randomPointIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
                            Transform selectedSpawnPoint = availableSpawnPoints[randomPointIndex];

                            // �������� ������������ ������ �������� ����������� ����� ������
                            //Debug.Log("Random point index: " + randomPointIndex);

                            int objectsCount = GetObjectsCountOnLine(selectedSpawnPoint);

                            if (objectsCount < 2)
                            {
                                // ���������� ������ �������� ������� LetterBox
                                float delay = objectsCount >= 1 ? spawnDelaySamePoint : spawnDelayBetweenPoints;
                                yield return new WaitForSeconds(delay);

                                GameObject newLetter = Instantiate(prefabToSpawn, selectedSpawnPoint.position, Quaternion.identity, transform);
                                LetterBox letterBox = newLetter.GetComponent<LetterBox>();
                                if (letterBox != null)
                                {
                                    // �������� ����� �������� ������� LetterBox
                                    letterBox.spawnerNumber = Array.IndexOf(spawnPoints, selectedSpawnPoint); // ���������� ������ ����� ������ � �������
                                }
                                AddObjectToLine(selectedSpawnPoint);
                                break; // ��������� ����, ����� ������� ������ ���� ������ �� ������� �����
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
                
                gameField.CheckLines(); // ��������� ���������� ����� ����� ����� ������

                yield return new WaitForSeconds(0.25f);
                if (!gameField.defeat)
                {
                    controllerUI.greenButton.interactable = true;
                }
                spawning = false; // ����� ���������� ������ ��������� ���� ������
            }
            yield return null; // ���� ���������� �����
        }
    }

    // �������� ���������� �������� �� ����� ������
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

    // ��������� ������� �������� �� ����� ������
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

    // �������� �������� �������� �� ������ ������
    private void ResetObjectCount()
    {
        objectsCountOnLine.Clear();
    }
}