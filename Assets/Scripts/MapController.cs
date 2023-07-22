using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    Vector3 noTerrainPosition;
    public GameObject currentChunk;
    PlayerMovement playerMovement;

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
        playerMovement = FindObjectOfType<PlayerMovement>();
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

        if(playerMovement.moveDir.x > 0 && playerMovement.moveDir.y == 0) { // Right
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x < 0 && playerMovement.moveDir.y == 0) { // Left
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x == 0 && playerMovement.moveDir.y > 0) { // Up
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x == 0 && playerMovement.moveDir.y < 0) { // Down
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x > 0 && playerMovement.moveDir.y > 0) { // Right Up
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x > 0 && playerMovement.moveDir.y < 0) { // Right Down
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x < 0 && playerMovement.moveDir.y > 0) { // Left Up
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;
                SpawnChunk();
            }
        }
        else if(playerMovement.moveDir.x < 0 && playerMovement.moveDir.y < 0) { // Left Down
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask)) {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
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
