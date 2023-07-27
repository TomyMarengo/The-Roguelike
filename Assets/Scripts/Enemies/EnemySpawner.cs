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
    public float waveInterval; // The amount of seconds between each wave
    public static int enemiesAlive;
    public int maxEnemiesAllowed; // The maximum number of enemies allowed on the map at once
    public bool maxEnemiesReached = false;

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
        if(currentWave < waves.Count && waves[currentWave].spawnedCount == 0) {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;
        if(spawnTimer >= waves[currentWave].spawnInterval) {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);

        if(currentWave < waves.Count - 1 ) {
            currentWave++;
            CalculateWaveQuota();
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
        if(waves[currentWave].spawnedCount < waves[currentWave].waveQuota && !maxEnemiesReached) {
            // Spawn each type of enemy until the quota is filled
            foreach(EnemyGroup enemyGroup in waves[currentWave].enemyGroups) {
                // Check if the minimum number of enemies in this type have been spawned
                if(enemyGroup.spawnedCount < enemyGroup.enemyQuota) {
                    // Limit the number of enemies can be spawned
                    if(enemiesAlive >= maxEnemiesAllowed) {
                        maxEnemiesReached = true;
                        return;
                    }

                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    
                    enemyGroup.spawnedCount++;
                    waves[currentWave].spawnedCount++;
                    enemiesAlive++;
                }
            }
        }

        // Reset the maxEnemiesReached flag if the number of enemies alive has dropped below the maximum
        if(enemiesAlive < maxEnemiesAllowed) {
            maxEnemiesReached = false;
        }
    }

    public static void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
