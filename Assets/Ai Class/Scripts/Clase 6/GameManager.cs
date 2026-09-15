using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner.OnEnemySpawned += OnEnemySpawned;
        enemySpawner.OnEnemySpawned.Invoke(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemySpawner.Spawn(5);
        }
    }

    private void OnEnemySpawned(GameObject enemy)
    {
        Debug.Log($"Enemigo creado: {enemy.name}");
    }

    private void OnDestroy()
    {
        enemySpawner.OnEnemySpawned -= OnEnemySpawned;
    }
}