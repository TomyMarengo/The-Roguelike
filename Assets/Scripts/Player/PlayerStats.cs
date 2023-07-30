using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    #region Current Stats
    float currentHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set 
        {
            if (currentHealth != value ) {
                currentHealth = value;
                if (GameManager.instance != null)
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                // Add any additional logic here
            }
        }
    }

    float currentRecovery;

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set 
        {
            if (currentRecovery != value ) {
                currentRecovery = value;
                if (GameManager.instance != null)
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                // Add any additional logic here
            }
        }
    }

    float currentMoveSpeed;

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set 
        {
            if (currentMoveSpeed != value ) {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                // Add any additional logic here
            }
        }
    }

    float currentMight;

    public float CurrentMight
    {
        get { return currentMight; }
        set 
        {
            if (currentMight != value ) {
                currentMight = value;
                if (GameManager.instance != null)
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                // Add any additional logic here
            }
        }
    }

    float currentProjectileSpeed;

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set 
        {
            if (currentProjectileSpeed != value ) {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                // Add any additional logic here
            }
        }
    }

    float currentMagnet;

    public float CurrentMagnet
    
    {
        get { return currentMagnet; }
        set 
        {
            if (currentMagnet != value ) {
                currentMagnet = value;
                if (GameManager.instance != null)
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                // Add any additional logic here
            }
        }
    }
    #endregion

    //Experience and level
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class for defining a level range and the corresponding experience cap increase for that range.
    [System.Serializable]
    public class LevelRange 
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    InventoryManager inventory;

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    //TEST
    public GameObject secondWeaponTest, firstPassiveItemTest, secondPassiveItemTest;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);
    }

    void Update() {
        if(invincibilityTimer > 0) {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible) {
            isInvincible = false;
        }
        
        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        int levelsToIncrease = 0;

        while(experience >= experienceCap) {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges) {
                if(level >= range.startLevel && level <= range.endLevel) {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            levelsToIncrease++;
        }

        StartCoroutine(LevelUpCoroutine(levelsToIncrease));
    }

    private IEnumerator LevelUpCoroutine(int levelsToIncrease)
{
    for (int i = 0; i < levelsToIncrease; i++)
    {
        Debug.Log("Level up");
        GameManager.instance.StartLevelUp();

        // Pausar la ejecución de la función hasta que se ejecute EndLevelUp
        yield return new WaitUntil(() => !GameManager.instance.choosingUpgrades);

        // Aquí la función se reanudará después de que la pantalla de subir niveles haya sido cerrada
    }
}

    public void TakeDamage(float dmg)
    {
        if(!isInvincible) {
            CurrentHealth -= dmg;
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(CurrentHealth <= 0) {
                Kill();
            }
        }
        
    }

    public void Kill()
    {
        if(!GameManager.instance.isGameOver) {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth+amount, characterData.MaxHealth);
    }

    void Recover()
    {
        RestoreHealth(CurrentRecovery * Time.deltaTime);
    }

    public void SpawnWeapon(GameObject weapon)
    {
        //Spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(spawnedWeapon.GetComponent<WeaponController>());
    }

        public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Spawn the starting weapon
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(spawnedPassiveItem.GetComponent<PassiveItem>());
    }

}
