using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private GameObject[] enemies;

    private void Start() {
        InvokeRepeating("SpawnEnemy", 5, 5);
    }

    private void SpawnEnemy() {
        int chanceToSpawn = Random.Range(1, 100);
        
        if(chanceToSpawn <= UIManager.Instance.score) {
            EventManager.Instance.Event_EnemySpawned();

            int enemy = Random.Range(0, enemies.Length);
            int spawnPoint = Random.Range(0, enemySpawnPoints.Length);

            Instantiate(enemies[enemy], enemySpawnPoints[spawnPoint].position, Quaternion.identity);
        }
    }

    public void StopSpawningEnemy() => enabled = false;
}
