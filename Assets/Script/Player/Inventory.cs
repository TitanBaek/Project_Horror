using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    private PlayerInput inputSystem;
    [SerializeField] private Dictionary<string,Item> pocket = new Dictionary<string,Item>();
    public Dictionary<string,Item> Pocket { get { return pocket; } set { pocket = value; }  }
    [SerializeField] public EquipItem[] equipment = new EquipItem[2];
    public GameObject inventoryUI;
    private bool isOpen;
    private bool firstOpen;
    private Player player;

    private void Awake()
    {
        Debug.Log("�κ��丮 ��ũ��Ʈ �����ũ");
        isOpen = false;
        firstOpen = true;
        inputSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void DoUse<T>(T useableItem) where T : UseItem
    {
        useableItem.UseThis(player);
        player.CurrentHp();
        RemoveItem(useableItem);
    }

    public bool DoEquip<T>(T equipItem) where T : EquipItem
    {
        bool returnData = false;
        // equipment �� ù��°�� �ķ���..
        // �ι�°�� ����...ResetEquips()

        player.ResetEquips();
        if (equipItem.Category == ItemCategory.Weapon)
        {
            // ���� �ٸ� �������� ������� ��� �ش� �������� ���������Ѵ�.
            if (equipment[0] != null && equipment[0] != equipItem)
            {
                SetNowEquip(equipment[0], false);
                equipment[0] = null;
            }

            //����ϰ��� �ϴ� �������� �̹� �����Ǿ� �ִ� ���
            if (equipItem.NowEquip)         
            {
                // �������� ����
                equipment[0] = null;
                SetNowEquip(equipItem, false);
                returnData = false;
            }
            else
            {
                //�����Ǿ����� ���� ��� ����
                equipment[0] = equipItem;
                SetNowEquip(equipItem,true);
                returnData = true;
            }
        } else if(equipItem.Category == ItemCategory.subWeapon)
        {
            // ���� �ٸ� �������� ������� ��� �ش� �������� ���������Ѵ�.
            if (equipment[1] != null && equipment[1] != equipItem)
            {
                SetNowEquip(equipment[1], false);
                equipment[1] = null;
            }
            //����ϰ��� �ϴ� �������� �̹� �����Ǿ� �ִ� ���
            if (equipItem.NowEquip)
            {
                equipment[1] = null;
                SetNowEquip(equipItem, false);
                returnData = false;
            }
            //�����Ǿ����� ���� ��� ����
            else
            {
                equipment[1] = equipItem;
                SetNowEquip(equipItem, true);
                returnData = true;
            }
        }
        player.SetEquips();
        //Ż���� �Լ�
        return returnData;
    }

    public void Test()
    {
        Debug.Log("�׽�Ʈ");
    }
    public void SetNowEquip(EquipItem item,bool state)
    {
        item.NowEquip = state;
    }

    public void AddItem<T>(T item) where T : Item
    {
        Debug.Log("�ִپ�����");
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
    }

    public void RemoveItem(Item item)
    {
        // �������� ���ưų� â��� �ű涧 ȣ��Ǵ� �Լ�
        Debug.Log($"�����ۿ� {item.ItemName} ���� ��");
        pocket.Remove(item.ItemName);
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
        ResetInventory();

        if (isOpen)
        {
            //inputSystem.enabled = true;
            RenderSettings.fog = true;
            Time.timeScale = 1f;
            inventoryUI.SetActive(false);
            isOpen = false;
        } else
        {
            //inputSystem.enabled = false;
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
