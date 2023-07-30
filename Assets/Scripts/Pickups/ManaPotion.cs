using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : Pickup
{
    public int manaToRestore;

    protected override void Collect() 
    {
        /*PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);*/
    }
}
