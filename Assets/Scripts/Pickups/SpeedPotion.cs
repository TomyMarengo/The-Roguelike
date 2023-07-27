using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Pickup, ICollectible
{
    public int speedUp;

    public void Collect()
    {
        /*PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);*/
    }
}
