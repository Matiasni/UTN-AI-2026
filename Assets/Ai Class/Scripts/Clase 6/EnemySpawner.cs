using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public  Action<GameObject> OnEnemySpawned;

    private Func<int, Transform> selectSpawnPoint;

    private void Start()
    {
        // Primero
        //selectSpawnPoint = index => spawnPoints[0];

        // Último
        //selectSpawnPoint = index => spawnPoints[spawnPoints.Length - 1];

        //selectSpawnPoint = index => spawnPoints[spawnPoints.Length - 1];


        // Aleatorio
        selectSpawnPoint = index => spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
    }

    public void Spawn(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform spawnPoint = selectSpawnPoint(i);

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            OnEnemySpawned?.Invoke(enemy);
        }
    }
}