using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Current stats
    public float currentMoveSpeed;
    public float currentHealth;
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    void Awake() 
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }

    private void Update() {
        if(Vector2.Distance(transform.position, player.position) >= despawnDistance) {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg)
    {  
        currentHealth -= dmg;
        if (currentHealth <= 0){
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player")) {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy() 
    {
        EnemySpawner.OnEnemyKilled();
    }

    private void ReturnEnemy() 
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        
        //float newDistance = Vector2.Distance(player.position, enemySpawner.relativeSpawnPoints[Random.Range(0, enemySpawner.relativeSpawnPoints.Count)].position);
        //transform.position = player.position + (Vector3)(playerMovement.moveDir * newDistance);
        
        transform.position = player.position + (Vector3)(playerMovement.moveDir * despawnDistance);
    }
}
