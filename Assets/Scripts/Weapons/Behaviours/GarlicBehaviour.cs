using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy") && !markedEnemies.Contains(other.gameObject)) {
            base.OnTriggerEnter2D(other);
            markedEnemies.Add(other.gameObject);
        }
        else if(other.CompareTag("Prop") && !markedEnemies.Contains(other.gameObject)) {
            if(other.gameObject.TryGetComponent(out BreakableProps breakable)) {
                breakable.TakeDamage(currentDamage);
                markedEnemies.Add(other.gameObject);
            }
        }
    }

}
