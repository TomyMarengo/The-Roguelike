using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class InventoryManager : MonoBehaviour
{
    public static int MAX_SLOTS = 6;
    public List<WeaponController> weaponSlots = new List<WeaponController>(MAX_SLOTS);
    public int[] weaponLevels = new int[MAX_SLOTS];
    public List<Image> weaponUISlots = new List<Image>(MAX_SLOTS);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(MAX_SLOTS);
    public int[] passiveItemLevels = new int[MAX_SLOTS];
    public List<Image> passiveItemUISlots = new List<Image>(MAX_SLOTS);

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
}
