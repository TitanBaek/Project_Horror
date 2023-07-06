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
        Debug.Log("��ġ�� �̰� ��������");
         
        RaycastHit hit;// ������

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            Debug.Log($"{hit.collider.gameObject.name} �� ����");
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
