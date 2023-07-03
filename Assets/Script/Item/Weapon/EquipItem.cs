using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item
{
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float damage;
    [SerializeField] protected AudioClip[] weaponSoundClips;
    protected bool nowEquip = false;
    public bool NowEquip { get { return nowEquip; } set { nowEquip = value; } }

    public virtual void Attack()
    {
        Debug.Log("°ø°ÝÇÔ");
    }
}
