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
        foreach(UpgradeUI upgradeOption in upgradeUIOptions) {
            int upgradeType = UnityEngine.Random.Range(1, 3);
            if(upgradeType == 1) {
                WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[UnityEngine.Random.Range(0, weaponUpgradeOptions.Count)];

                if(chosenWeaponUpgrade != null) {
                    bool newWeapon = true;
                    for (int i = 0; i < weaponSlots.Count; i++) {
                        if(weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData) {
                            newWeapon = false;
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i));
                            upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                            upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.WeaponName;
                            break;
                        }
                    }
                    if(newWeapon) { //Spawn a new weapon
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.WeaponName;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }

            else if(upgradeType == 2) {
                PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[UnityEngine.Random.Range(0, passiveItemUpgradeOptions.Count)];

                if(chosenPassiveItemUpgrade != null) {
                    bool newPassiveItem = true;
                    for (int i = 0; i < passiveItemSlots.Count; i++) {
                        if(passiveItemSlots[i] != null &&    passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData) {
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i));
                            newPassiveItem = false;
                            upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                            upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.PassiveItemName;
                            break;
                        }
                    }
                    if(newPassiveItem) { //Spawn a new passive item
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.PassiveItemName;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
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
