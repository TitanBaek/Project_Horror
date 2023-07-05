using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryUI : BaseUI
{
    [SerializeField] private GameObject Slots_Center;
    [SerializeField] private TMP_Text itemText;
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
    }

    public void SetSelectedItem(Item item)
    {
        Debug.Log($"������ ���� {item.ItemName}");
    }
    public void ResetInventory()
    {
        numOfChild = Slots_Center.transform.childCount;

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

        numOfChild = Slots_Center.transform.childCount;

        for (int i = 0; i < numOfChild; i++)
        {
            float angle = Mathf.PI * 0.5f - i * (Mathf.PI * 2.0f) / numOfChild;
            GameObject child = Slots_Center.transform.GetChild(i).gameObject;

            child.transform.position
                = Slots_Center.transform.position + (new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle))) * radius;
        }

        SetItemText();
    }

    private void SetItemText(string text = "")
    {
        itemText.text = $"{text}  {selectedItem.ItemName}";
    }

    private void SetSlotParent(GameObject slot)
    {
        slot.transform.SetParent(Slots_Center.transform);
    }


    private void OnNavigation(InputValue value)
    {
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
        CoroutineStop();
    }


    IEnumerator Rotate(float rotateFloat, float dir)
    {
        bool rotateBool = true;
        int loopcount = 0;
        float y = dir > 0 ? 1 : -1; // �����̸� ����, �����̸� ����
        float destination = 0;
        if (dir > 0)
        {
            Debug.Log($" Dir {dir} , Y {Slots_Center.transform.rotation.eulerAngles.y} , ��ǥ Y {Slots_Center.transform.rotation.eulerAngles.y + rotateFloat}");
            destination = Slots_Center.transform.rotation.eulerAngles.y + rotateFloat;
        }
        else
        {
            Debug.Log($" Dir {dir} , Y {Slots_Center.transform.rotation.eulerAngles.y} , ��ǥ Y {Slots_Center.transform.rotation.eulerAngles.y - rotateFloat}");
            destination = Slots_Center.transform.rotation.eulerAngles.y - rotateFloat;
        }
        while (rotateBool)
        {
            loopcount++;
            yield return new WaitForSecondsRealtime(0.05f);
            Slots_Center.transform.Rotate(new Vector3(Slots_Center.transform.rotation.x, Slots_Center.transform.rotation.y + y, Slots_Center.transform.rotation.z));

            if(loopcount >= rotateFloat)
            {
                rotateBool = false;
            }
        }
        nowSpin = false;
        CoroutineStop();
    }

    private void CoroutineStop()
    {
        StopCoroutine(naviCoroutine);
    }




}     


