using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    List<Weapon> weaponList = new List<Weapon>();

    public void Fire()
    {
        weapon.Attack();
    }

    public void GetWeapon(Weapon weapon)
    {
        weaponList.Add(weapon);
    }

    public void Swap(int index)
    {
        weapon = weaponList[index];
    }
}
