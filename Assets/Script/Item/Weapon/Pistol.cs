using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : EquipItem, IEquipable
{
    private void Awake()
    {
        this.category = ItemCategory.Weapon;
    }
    public override void Attack()
    {
        base.Attack();
        Debug.Log("그치만 이건 권총이지");
         
        RaycastHit hit;// 맞은것

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            Debug.Log($"{hit.collider.gameObject.name} 를 맞춤");
            IHitable hitable;            
            hitable = hit.transform.GetComponent<IHitable>();
            hitable?.Stun();
            hitable?.TakeHit(hit, 15);
        }
    }

    public void Equip()
    {
    }

    public void UnEquip()
    {
    }
}
