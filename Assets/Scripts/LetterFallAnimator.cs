using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterFallAnimator : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject prefabToSpawn;
    public GameObject spawner;
    public Coroutine spawnCoroutine;
    int countSpawns = 0;

    void Start()
    {
        ActivateFalling();
    }

    IEnumerator SpawnObjects()
    {
        while (spawner.activeSelf)
        {
            
            List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

            int pointIndex;

            if (countSpawns % 2 == 0)
            {
                pointIndex = 0;
            }
            else
            {
                pointIndex = 1;
            }

            Transform selectedSpawnPoint = availableSpawnPoints[pointIndex];

            GameObject newLetter = Instantiate(prefabToSpawn, selectedSpawnPoint.position, Quaternion.identity, transform);


            countSpawns++;
            yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.3f, 0.5f));
        }
        yield break;
    }

    public void ActivateFalling ()
    {
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }
}