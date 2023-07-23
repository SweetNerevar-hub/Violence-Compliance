using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] Transform[] enemySpawnPoints;
    [SerializeField] GameObject[] enemies;

    private void Start() {
        InvokeRepeating("SpawnEnemy", 5, 5);
    }

    private void SpawnEnemy() {
        int chanceToSpawn = Random.Range(0, 100);
        
        if(chanceToSpawn <= UIManager.Instance.score) {
            int enemy = Random.Range(0, enemies.Length);
            int spawnPoint = Random.Range(0, enemySpawnPoints.Length);

            Instantiate(enemies[enemy], enemySpawnPoints[spawnPoint].position, Quaternion.identity);
        }
    }

    public void StopSpawningEnemy() => enabled = false;
}
