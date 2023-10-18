using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float spawnInterval = 2.0f;

    private GameObject player;
    private int totalEnemiesSpawned = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnEnemy", 0, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (totalEnemiesSpawned >= 12)
        {
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        totalEnemiesSpawned++;

        EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null && player != null)
        {
            enemyMovement.SetTarget(player.transform);
        }
    }
}