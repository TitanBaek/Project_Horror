using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] EquipItem weapon;

    List<EquipItem> weaponList = new List<EquipItem>();
    public void Fire()
    {
        weapon.Attack();
    }

    public void GetWeapon(EquipItem weapon)
    {
        weaponList.Add(weapon);
    }

    public void Swap(int index)
    {
        weapon = weaponList[index];
    }
}
