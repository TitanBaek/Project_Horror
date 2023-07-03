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
        // equipment 의 첫번째는 후레쉬..
        // 두번째는 무기...
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
        Debug.Log($"아이템에 {item.name} 추가 됨");
        if (pocket.ContainsKey(item.name))
        {   // 이미 해당 아이템이 존재한다면 EA를 증감시켜줌
            Debug.Log("아이템 증감");
            pocket[item.name].ItemEA++;
        }
        else
        {
            pocket.Add(item.name, item);
        }
    }

    public void RemoveItem(Item item)
    {
        // 아이템이 사용됐거나 창고로 옮길때 호출되는 함수
        Debug.Log($"아이템에 {item.name} 삭제 됨");
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
