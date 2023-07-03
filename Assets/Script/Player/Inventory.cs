using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Dictionary<string,Item> pocket = new Dictionary<string,Item>();
    public Dictionary<string,Item> Pocket { get { return pocket; } set { pocket = value; }  }
    public EquipItem[] equipment = new EquipItem[2];
    public GameObject inventoryUI;
    private bool isOpen;

    private void Awake()
    {
        isOpen = false;
        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
    }
    public void DoEquip(EquipItem equipItem)
    {
        // equipment �� ù��°�� �ķ���..
        // �ι�°�� ����...
        if(equipItem.Category == ItemCategory.Weapon)
        {
            if (equipItem.NowEquip)
                return;
            equipment[0] = equipItem;
        } else if(equipItem.Category == ItemCategory.subWeapon)
        {
            if (equipItem.NowEquip)
                return;
            equipment[1] = equipItem;
        }
        SetNowEquip(equipItem);
    }

    public void SetNowEquip(EquipItem item)
    {
        item.NowEquip = true;
    }

    public void AddItem<T>(T item) where T : Item
    {
        Debug.Log($"�����ۿ� {item.name} �߰� ��");
        if (pocket.ContainsKey(item.name))
        {   // �̹� �ش� �������� �����Ѵٸ� EA�� ����������
            Debug.Log("������ ����");
            pocket[item.name].ItemEA++;
        }
        else
        {
            pocket.Add(item.name, item);
        }
    }

    public void RemoveItem(Item item)
    {
        // �������� ���ưų� â��� �ű涧 ȣ��Ǵ� �Լ�
        Debug.Log($"�����ۿ� {item.name} ���� ��");
        pocket.Remove(item.name);
    }

    public void OnInventory(InputValue value)
    {
        if(isOpen)
        {
            inventoryUI.SetActive(false);
            isOpen = false;
        } else
        {
            inventoryUI.SetActive(true);
            isOpen = true;
        }
        foreach(Item item in pocket.Values)
        {
            Debug.Log($"{item.name} / {item.ItemEA}EA");
        }
    }
}
