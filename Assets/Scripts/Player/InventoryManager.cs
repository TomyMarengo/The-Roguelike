using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static int MAX_SLOTS = 6;
    public List<WeaponController> weaponSlots = new List<WeaponController>(MAX_SLOTS);
    public int[] weaponLevels = new int[MAX_SLOTS];
    public List<Image> weaponUISlots = new List<Image>(MAX_SLOTS);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(MAX_SLOTS);
    public int[] passiveItemLevels = new int[MAX_SLOTS];
    public List<Image> passiveItemUISlots = new List<Image>(MAX_SLOTS);

    #region Upgrade
    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<Pickup> extraUpgradeOptions = new List<Pickup>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();
    #endregion

    PlayerStats player;

    private void Start() {
        player = GetComponent<PlayerStats>();
    }


    public int AddWeapon(WeaponController weapon)
    {
        for (int i = 0; i < weaponSlots.Count; i++){
            if(weaponSlots[i] == null) {
                AddWeapon(i, weapon);
                return i;
            }
        }
        return -1;
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
    }

    public int AddPassiveItem(PassiveItem passiveItem)
    {
        for (int i = 0; i < passiveItemSlots.Count; i++){
            if(passiveItemSlots[i] == null) {
                AddPassiveItem(i, passiveItem);
                return i;
            }
        }
        return -1;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;  
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
    }

    public void LevelUpWeapon(int slotIndex)
    {
        if(weaponSlots[slotIndex] != null && weaponSlots[slotIndex].weaponData.NextLevelPrefab) {
            WeaponController weapon = weaponSlots[slotIndex];
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);  // Set the weapon to be a child of the player

            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;
        }
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if(passiveItemSlots[slotIndex] != null && passiveItemSlots[slotIndex].passiveItemData.NextLevelPrefab) {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);  // Set the weapon to be a child of the player

            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;
        }
    }

    public void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach(UpgradeUI upgradeOption in upgradeUIOptions) {
            bool optionDone = false;
            while (!optionDone && (availableWeaponUpgrades.Count > 0 || availablePassiveItemUpgrades.Count > 0)) {
                int upgradeType;
                if(availableWeaponUpgrades.Count == 0)
                    upgradeType = 2;
                else if(availablePassiveItemUpgrades.Count == 0)
                    upgradeType = 1;
                else
                    upgradeType = UnityEngine.Random.Range(1, 3);
                
                if(upgradeType == 1 && availableWeaponUpgrades.Count > 0) {
                    WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];

                    availableWeaponUpgrades.Remove(chosenWeaponUpgrade);
                    
                    bool newWeapon = true;
                    
                    for (int i = 0; i < weaponSlots.Count; i++) {
                        if(weaponSlots[i] != null && weaponSlots[i].weaponData.Type == chosenWeaponUpgrade.weaponData.Type) {
                            newWeapon = false;
                            if(weaponSlots[i].weaponData.NextLevelPrefab != null) {
                                optionDone = true;
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i));
                                upgradeOption.upgradeNameDisplay.text = weaponSlots[i].weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.WeaponName;
                                upgradeOption.upgradeDescriptionDisplay.text = weaponSlots[i].weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                            }
                            else {
                                weaponUpgradeOptions.Remove(chosenWeaponUpgrade);
                            }
                            break;
                        }
                    }

                    if(newWeapon) {
                        optionDone = true;
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.WeaponName;
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                    }
                }

                else if(upgradeType == 2 && availablePassiveItemUpgrades.Count > 0) {
                    PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];

                    availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);
                    
                    bool newPassiveItem = true;
                    
                    for (int i = 0; i < passiveItemSlots.Count; i++) {
                        if(passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData.Type == chosenPassiveItemUpgrade.passiveItemData.Type) {
                            newPassiveItem = false;
                            if(passiveItemSlots[i].passiveItemData.NextLevelPrefab != null) {
                                optionDone = true;
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i));
                                upgradeOption.upgradeNameDisplay.text = passiveItemSlots[i].passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.PassiveItemName;
                                upgradeOption.upgradeDescriptionDisplay.text = passiveItemSlots[i].passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                            }
                            else {
                                passiveItemUpgradeOptions.Remove(chosenPassiveItemUpgrade);
                            }
                            break;
                        }
                    }

                    if(newPassiveItem) {
                        optionDone = true;
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.PassiveItemName;
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                    }
                }
            }
            if(!optionDone) {
                Pickup chosenPickupUpgrade = extraUpgradeOptions[UnityEngine.Random.Range(0, extraUpgradeOptions.Count)];    
                upgradeOption.upgradeButton.onClick.AddListener(() => chosenPickupUpgrade.Collect());
                upgradeOption.upgradeNameDisplay.text = chosenPickupUpgrade.name;
                upgradeOption.upgradeDescriptionDisplay.text = "One " + chosenPickupUpgrade.name;
                upgradeOption.upgradeIcon.sprite = chosenPickupUpgrade.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void RemoveUpgradeOptions()
    {
        foreach (UpgradeUI upgradeOptions in upgradeUIOptions) {
            upgradeOptions.upgradeButton.onClick.RemoveAllListeners();
        }
    }
}
