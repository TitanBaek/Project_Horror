using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item
{
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float damage;
    [SerializeField] protected AudioClip[] weaponSoundClips;
    protected bool nowEquip = false;
    protected string originalItemName;
    public bool NowEquip { get { return nowEquip; } set { nowEquip = value; ChangeItemName();  } }

    private void Awake()
    {
        originalItemName = itemName;

    }
    private void Start()
    {
        originalItemName = itemName;
    }
    public void ChangeItemName()
    {
        if(NowEquip)
        {
            itemName = $"{originalItemName}(장비 중)";
        }
        else
        {
            ItemName = originalItemName;
        }
    }
    public virtual void Attack()
    {
        Debug.Log("공격함");
    }
}
