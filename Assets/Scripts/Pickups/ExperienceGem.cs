using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup
{
    [SerializeField]
    private int experienceGranted;

    public override void Collect() 
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
