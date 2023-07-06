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
        Debug.Log("인벤토리 스크립트 어웨이크");
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
        // equipment 의 첫번째는 후레쉬..
        // 두번째는 무기...ResetEquips()

        player.ResetEquips();
        if (equipItem.Category == ItemCategory.Weapon)
        {
            // 현재 다른 아이템을 장비중인 경우 해당 아이템을 장착해제한다.
            if (equipment[0] != null && equipment[0] != equipItem)
            {
                SetNowEquip(equipment[0], false);
                equipment[0] = null;
            }

            //장비하고자 하는 아이템이 이미 장착되어 있는 경우
            if (equipItem.NowEquip)         
            {
                // 장착해제 실행
                equipment[0] = null;
                SetNowEquip(equipItem, false);
                returnData = false;
            }
            else
            {
                //장착되어있지 않은 경우 장착
                equipment[0] = equipItem;
                SetNowEquip(equipItem,true);
                returnData = true;
            }
        } else if(equipItem.Category == ItemCategory.subWeapon)
        {
            // 현재 다른 아이템을 장비중인 경우 해당 아이템을 장착해제한다.
            if (equipment[1] != null && equipment[1] != equipItem)
            {
                SetNowEquip(equipment[1], false);
                equipment[1] = null;
            }
            //장비하고자 하는 아이템이 이미 장착되어 있는 경우
            if (equipItem.NowEquip)
            {
                equipment[1] = null;
                SetNowEquip(equipItem, false);
                returnData = false;
            }
            //장착되어있지 않은 경우 장착
            else
            {
                equipment[1] = equipItem;
                SetNowEquip(equipItem, true);
                returnData = true;
            }
        }
        player.SetEquips();
        //탈부착 함수
        return returnData;
    }

    public void Test()
    {
        Debug.Log("테스트");
    }
    public void SetNowEquip(EquipItem item,bool state)
    {
        item.NowEquip = state;
    }

    public void AddItem<T>(T item) where T : Item
    {
        Debug.Log("애다아이템");
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
    }

    public void RemoveItem(Item item)
    {
        // 아이템이 사용됐거나 창고로 옮길때 호출되는 함수
        Debug.Log($"아이템에 {item.ItemName} 삭제 됨");
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
            // 생성
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
