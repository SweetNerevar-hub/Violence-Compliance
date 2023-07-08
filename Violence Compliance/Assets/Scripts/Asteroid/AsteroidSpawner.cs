using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] asteroids;

    private void Start() {
        InvokeRepeating("SpawnAsteroids", 0.5f, 1f);
    }

    private void SpawnAsteroids() {
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        int randomAsteroid = Random.Range(0, asteroids.Length);

        GameObject asteroid = Instantiate(asteroids[randomAsteroid], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
        asteroid.GetComponent<Animator>().SetInteger("AsteroidType", randomAsteroid);
    }
}
