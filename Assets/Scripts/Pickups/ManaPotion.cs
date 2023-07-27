using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : Pickup, ICollectible
{
    public int manaToRestore;

    public void Collect()
    {
        /*PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);*/
    }
}
