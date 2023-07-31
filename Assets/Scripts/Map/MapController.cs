using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDuration;
    

    // Start is called before the first frame update
    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if(!currentChunk)
            return;

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);
        CheckAndSpawnChunk(directionName);

        string[] directions = new string[] {"Left", "Right", "Up", "Down"};
        foreach (string dir in directions) {
            if (directionName.Contains(dir)) {
                CheckAndSpawnChunk(dir);
            }
        }
    }

    void CheckAndSpawnChunk(string dir)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(dir).position, checkerRadius, terrainMask)) {
            SpawnChunk(currentChunk.transform.Find(dir).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            // Moving horizontally more than vertically
            if(direction.y > 0.5f) {
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if(direction.y < -0.5f) {
                return direction.x > 0 ? "Right Down" : "Left Down";
            }
            else {
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else {
            if(direction.x > 0.5f) {
                return direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if(direction.x < -0.5f) {
                return direction.y > 0 ? "Left Up" : "Left Down";
            }
            else {
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;
        if(optimizerCooldown <= 0f) {
            optimizerCooldown = optimizerCooldownDuration;
        }
        else {
            return;
        }

        foreach (GameObject chunk in spawnedChunks) {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(opDist > maxOpDist) {
                chunk.SetActive(false);
            }
            else {
                chunk.SetActive(true);
            }
        }
        
    }
}
