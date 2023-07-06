using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;

public class InventoryUI : BaseUI
{
    [SerializeField] private GameObject Slots_Center;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    private Player player;
    private float radius = 50f;
    private int numOfChild;
    private int inventoryMaxnum = 8;
    private Vector3 dir;
    private bool nowSpin;
    //private Dictionary<Item, bool> itemList;
    [SerializeField] private List<Item> itemList;
    private Item selectedItem;
    private int selectedItemIndex;
    Coroutine naviCoroutine;
    Coroutine inventoryResetCoroutine;
    /*
     * ��ųʸ� �ϳ� �� �����..?
     * <Item,Bool> �� �ؼ� .. 
     * ���� ����Ʈ�� �������� ..
     */
    private void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       nowSpin = false;
       selectedItemIndex = 0;
    }

    private void Start()
    {
        Debug.Log("�κ��丮UI��ũ��Ʈ");
        Debug.Log(player.name);
    }

    public void SetSelectedItem(Item item)
    {
        Debug.Log($"������ ���� {item.ItemName}");
    }

    public void ResetInventory()
    {
        if (player._Inventroy.Pocket.Count == 0)
            return;

        numOfChild = Slots_Center.transform.childCount;
        nowSpin = false;
        for (int i = 0; i < numOfChild; i++)
        {
            GameManager.Resource.Destroy(Slots_Center.transform.GetChild(i).gameObject);            
        }

        //itemList.Clear(); // �����Ҷ� ��ųʸ� �ʱ�ȭ ... 
        itemList = null;
        selectedItem = null;
        selectedItemIndex = 0;
        Slots_Center.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void SetInventory()
    {
        Debug.Log("SetInventory");
        bool isFirst = true;
        itemList = new List<Item>();

        if (player._Inventroy.Pocket.Count == 0)
            return;
        foreach (Item value in player._Inventroy.Pocket.Values)
        {
            // ���� ù �������� ����Ƽ���
            if (isFirst)
            {
                selectedItem = value;
                selectedItemIndex = 0;
                isFirst = false;
            }
            itemList.Add(value);
            Debug.Log($"SetInventory : {value.ItemName}");
            GameObject insertData;            
            insertData = GameManager.Instantiate<GameObject>(value.Slot, Slots_Center.transform.position, Quaternion.Euler(0, 0, 0));
            SetSlotParent(insertData);
        }
        SetObjectPosition();
        SetItemText();
    }

    private void SetObjectPosition()
    {
        Slots_Center.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        numOfChild = Slots_Center.transform.childCount;
        Debug.Log($"���콺���� {nowSpin} ������ ������ �ڽ� ��ü�� �� �����¿� : {Slots_Center.transform.childCount} �̰� ���� ������ ������ ������{player._Inventroy.Pocket.Count} �׸��� ���� ����Ƽ�� �ε�����{selectedItemIndex}");
        for (int i = 0; i < numOfChild; i++)
        {
            float angle = Mathf.PI * 0.5f - i * (Mathf.PI * 2.0f) / numOfChild;
            GameObject child = Slots_Center.transform.GetChild(i).gameObject;

            child.transform.position
                = Slots_Center.transform.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))) * radius;
        }
    }

    private void SetItemText(string text = "")
    {
        itemText.text = $"{text}  {selectedItem.ItemName}";
        itemInfoText.text = selectedItem.Description;
    }

    private void SetSlotParent(GameObject slot)
    {
        slot.transform.SetParent(Slots_Center.transform);
    }

    private void OnChoose(InputValue value) {

        Debug.Log($"���õ� �������� �ε��� {selectedItemIndex}");

        if(selectedItem.Category == ItemCategory.Weapon || selectedItem.Category == ItemCategory.subWeapon)
        {
            player._Inventroy.DoEquip((EquipItem) selectedItem);
            SetItemText();
        }
        else if (selectedItem.Category == ItemCategory.Usable)
        {
            player._Inventroy.DoUse((UseItem) selectedItem);
            ResetItemList();
        }
    }

    private void OnNavigation(InputValue value)
    {
        if (player._Inventroy.Pocket.Count == 0)
            return;

        if (nowSpin)
            return;
        Debug.Log(selectedItem.ItemName);
        float rotateFloat = (360 / player._Inventroy.Pocket.Count);
        dir = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
        if(dir.x > 0)
        {
            selectedItemIndex++;
            // �ε��� �ٲ��ֱ�
            if (itemList.Count <= selectedItemIndex)
                selectedItemIndex = 0;
        }
        else if(dir.x < 0)
        {
            selectedItemIndex--;
            if (0 > selectedItemIndex)
                selectedItemIndex = itemList.Count - 1;
            // �ε��� �ٲ��ֱ�
        }
        Debug.Log($"{itemList.Count} ũ���� ����Ʈ���� ���� �ε����� {selectedItemIndex} ");
        selectedItem = itemList[selectedItemIndex];

        naviCoroutine = StartCoroutine(LookAtRoutine(Quaternion.Euler(new Vector3(0, -1 * selectedItemIndex * rotateFloat, 0))));
        SetItemText();

    }
    IEnumerator LookAtRoutine(Quaternion rotatePosition)
    {
        bool nowRotate = true;
        nowSpin = true;
        while (nowRotate)
        {
            Slots_Center.transform.rotation = Quaternion.Lerp(Slots_Center.transform.rotation, rotatePosition, 0.1f);
            float angle = Quaternion.Angle(Slots_Center.transform.rotation, rotatePosition);

            if (angle <= 0)
            {
                nowRotate = false;
                Debug.Log("ȸ���� ������");
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        nowSpin = false;
        CoroutineStop(naviCoroutine);
    }


    private void CoroutineStop(Coroutine co)
    {
        StopCoroutine(co);
    }

    private void ResetItemList()
    {
        nowSpin = false;
        numOfChild = Slots_Center.transform.childCount;
        Debug.Log($"��Ʈ���� ���� Slots_Center�� �ڽ� ��ü ��: {Slots_Center.transform.childCount}");

        for (int i = 0; i < numOfChild; i++)
        {
            // ����ϰ��� �� �������� �ε����� ���� Slots_Center�� �ڽİ�ü�� Destroy�Ѵ�.
            if (i == selectedItemIndex)
            {
                GameManager.Resource.Destroy(Slots_Center.transform.GetChild(i).gameObject);
            }
        }
         
        itemList.RemoveAt(selectedItemIndex); // �Ҹ��� �������� ����߱� ������ �ش� �������� �����Ѵ�.
        selectedItemIndex = selectedItemIndex - 1 < 0 ? 0 : selectedItemIndex - 1; // ����Ƽ��������ε��� �ʱ�ȭ
        selectedItem = itemList[selectedItemIndex];

        inventoryResetCoroutine = StartCoroutine(ChildPositionSet());

        float rotateFloat = (360 / player._Inventroy.Pocket.Count);
        naviCoroutine = StartCoroutine(LookAtRoutine(Quaternion.Euler(new Vector3(0, -1 * selectedItemIndex * rotateFloat, 0))));
    }

    private IEnumerator ChildPositionSet()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"��Ʈ���� ���� Slots_Center�� �ڽ� ��ü ��: {Slots_Center.transform.childCount}");
        SetObjectPosition(); // Slots�� �ڽİ�ü�� ������ �������ִ� �Լ�
        yield return new WaitForEndOfFrame();
        StopCoroutine(inventoryResetCoroutine);
    }



    private Item FindItemWithIndex(int index)
    {
        int count = 0;
        foreach (Item value in player._Inventroy.Pocket.Values)     // ���ϳ��θ� ����.. �Ű������� ���޹��� �ε����� �´� ���� �����Ѵ�.
        {
            if(index == count)
            {
                return value;
            }
            count++;
        }
        return null;
    }

    private void InInventoryReset()
    {
        float rotateFloat = (360 / player._Inventroy.Pocket.Count);
        Debug.Log($"2. ���� ���� ���� ������ ������ : {player._Inventroy.Pocket.Count}");
        
        foreach (Item value in player._Inventroy.Pocket.Values)
        {
            // ������ ������Ʈ ���� �� �θ�ü ����
            GameObject insertData;
            insertData = GameManager.Instantiate<GameObject>(value.Slot, Slots_Center.transform.position, Quaternion.Euler(0, 0, 0));
            SetSlotParent(insertData);
        }

        SetObjectPosition(); // Slots�� �ڽİ�ü�� ������ �������ִ� �Լ�
        naviCoroutine = StartCoroutine(LookAtRoutine(Quaternion.Euler(new Vector3(0, -1 * selectedItemIndex * rotateFloat, 0))));
        SetItemText();
    }

}     


