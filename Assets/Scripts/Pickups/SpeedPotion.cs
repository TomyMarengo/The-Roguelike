using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Pickup
{
    [SerializeField]
    private int speedUp;

    public override void Collect() 
    {
        /*PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);*/
    }
}
