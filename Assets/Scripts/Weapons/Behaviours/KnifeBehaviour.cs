using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    public static KnifeController knifeController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        knifeController = FindObjectOfType<KnifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * knifeController.speed * Time.deltaTime;
    }
}
