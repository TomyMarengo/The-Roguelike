using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Pickup
{
    public int speedUp;

    protected override void Collect() 
    {
        /*PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);*/
    }
}
