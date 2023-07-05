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
        Debug.Log($"�����ۿ� {item.ItemName} �߰� ��");
        if (pocket.ContainsKey(item.ItemName))
        {   // �̹� �ش� �������� �����Ѵٸ� EA�� ����������
            Debug.Log("������ ����");
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
        // �������� ���ưų� â��� �ű涧 ȣ��Ǵ� �Լ�
        Debug.Log($"�����ۿ� {item.ItemName} ���� ��");
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
            // ����
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
