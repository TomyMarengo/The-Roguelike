using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InventoryManager : MonoBehaviour
{
    const int MAX_SLOTS = 6;
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[MAX_SLOTS];
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[MAX_SLOTS];

    private int weaponIndex;
    private int passiveItemIndex;

    public void AddWeapon(WeaponController weapon)
    {
        if(weaponIndex >= MAX_SLOTS)
            Debug.LogError("Weapon inventory slots already full");

        weaponSlots[weaponIndex++] = weapon;
    }

    public void AddPassiveItem(PassiveItem passiveItem)
    {
        if(passiveItemIndex >= MAX_SLOTS)
            Debug.LogError("Passive Item inventory slots already full");

        passiveItemSlots[passiveItemIndex++] = passiveItem;
    }

    public void LevelUpWeapon(int slotIndex)
    {

    }

    public void LevelUpPassiveItem(int slotIndex)
    {

    }
}
