using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    //Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake() 
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirX < 0 && dirY == 0) { // Left
            scale.x *= -1;
            scale.y *= -1;
        }
        else if(dirX == 0 && dirY < 0) { // Down
            scale.y *= -1;
        }
        else if(dirX == 0 && dirY > 0) { // Up
            scale.x *= -1;
        }
        else if(dirX > 0 && dirY > 0) { // Right Up
            rotation.z = 0f;
        }
        else if(dirX > 0 && dirY < 0) { // Right Down
            rotation.z = -90f;
        }
        else if(dirX < 0 && dirY > 0) { // Left Up
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if(dirX < 0 && dirY < 0) { // Left Down
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Enemy")) {
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
    }

    void ReducePierce()
    {
        currentPierce--;
        if(currentPierce <= 0) {
            Destroy(gameObject);
        }
    }

}
