using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : Pickup
{
    [SerializeField]
    private int manaToRestore;

    public override void Collect() 
    {
        /*PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);*/
    }
}
