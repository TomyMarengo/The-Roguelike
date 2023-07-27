using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // A list of groups of enemies to spawn in this wave
        public int waveQuota; // The total number of enemies to spawn in this wave
        public float spawnInterval; // The interval at thich to spawn enemies
        public int spawnedCount; // The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyQuota; // The number of enemies to spawn
        public int spawnedCount; // The number of enemies of this type already spawned
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWave; // Index of the current wave

    [Header("Spawner Attributes")]
    float spawnTimer; // Timer use to determine when to spawn the next enemy

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[currentWave].spawnInterval) {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (EnemyGroup enemyGroup in waves[currentWave].enemyGroups) {
            currentWaveQuota += enemyGroup.enemyQuota;
        }

        waves[currentWave].waveQuota = currentWaveQuota;
    }

    void SpawnEnemies()
    {
        // Check if the minimum number of enemies in the wave have been spawned
        if(waves[currentWave].spawnedCount < waves[currentWave].waveQuota) {
            // Spawn each type of enemy until the quota is filled
            foreach(EnemyGroup enemyGroup in waves[currentWave].enemyGroups) {
                // Check if the minimum number of enemies in this type have been spawned
                if(enemyGroup.spawnedCount < enemyGroup.enemyQuota) {
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    
                    enemyGroup.spawnedCount++;
                    waves[currentWave].spawnedCount++;
                }
            }
        }
    }
}
