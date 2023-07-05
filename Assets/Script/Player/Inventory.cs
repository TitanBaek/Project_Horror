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
    private bool firstOpen;

    private void Awake()
    {
        isOpen = false;
        firstOpen = true;
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
        Debug.Log($"아이템에 {item.ItemName} 추가 됨");
        if (pocket.ContainsKey(item.ItemName))
        {   // 이미 해당 아이템이 존재한다면 EA를 증감시켜줌
            Debug.Log("아이템 증감");
            pocket[item.ItemName].ItemEA++;
        }
        else
        {
            pocket.Add(item.ItemName, item);
        }
        ResetInventory();
    }

    public void RemoveItem(Item item)
    {
        // 아이템이 사용됐거나 창고로 옮길때 호출되는 함수
        Debug.Log($"아이템에 {item.ItemName} 삭제 됨");
        pocket.Remove(item.ItemName);
        ResetInventory();
    }

    public void ResetInventory()
    {
        if (!firstOpen)
            GameManager.UI.inventoryUI.ResetInventory();
    }

    public void OnInventory(InputValue value)
    {

        if (firstOpen)
        {
            // 생성
            GameManager.UI.CreateInventoryUI();
            inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
            firstOpen = false;
        }


        if (isOpen)
        {
            RenderSettings.fog = true;
            Time.timeScale = 1f;
            inventoryUI.SetActive(false);
            isOpen = false;
            ResetInventory();
        } else
        {
            GameManager.UI.inventoryUI.SetInventory();
            RenderSettings.fog = false;
            Time.timeScale = 0f;
            inventoryUI.SetActive(true);
            isOpen = true;
        }
        foreach (Item item in pocket.Values)
        {
            Debug.Log($"{item.ItemName} / {item.ItemEA}EA");
        }
    }
}
