using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float currentHealth;

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        
        if(currentHealth <= 0) {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
