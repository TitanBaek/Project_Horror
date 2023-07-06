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
     * 딕셔너리 하나 더 만들어..?
     * <Item,Bool> 로 해서 .. 
     * ㄴㄴ 리스트로 만들어야해 ..
     */
    private void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       nowSpin = false;
       selectedItemIndex = 0;
    }

    private void Start()
    {
        Debug.Log("인벤토리UI스크립트");
        Debug.Log(player.name);
    }

    public void SetSelectedItem(Item item)
    {
        Debug.Log($"아이템 선택 {item.ItemName}");
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

        //itemList.Clear(); // 리셋할때 딕셔너리 초기화 ... 
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
            // 가장 첫 아이템을 셀렉티드로
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
        Debug.Log($"나우스핀은 {nowSpin} 슬롯츠 센터의 자식 개체의 총 개수는요 : {Slots_Center.transform.childCount} 이고 포켓 내부의 아이템 개수는{player._Inventroy.Pocket.Count} 그리고 지금 셀렉티드 인덱스는{selectedItemIndex}");
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

        Debug.Log($"선택된 아이템의 인덱스 {selectedItemIndex}");

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
            // 인덱스 바꿔주기
            if (itemList.Count <= selectedItemIndex)
                selectedItemIndex = 0;
        }
        else if(dir.x < 0)
        {
            selectedItemIndex--;
            if (0 > selectedItemIndex)
                selectedItemIndex = itemList.Count - 1;
            // 인덱스 바꿔주기
        }
        Debug.Log($"{itemList.Count} 크기의 리스트에서 현재 인덱스는 {selectedItemIndex} ");
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
                Debug.Log("회전이 끝났다");
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
        Debug.Log($"디스트로이 전의 Slots_Center의 자식 개체 수: {Slots_Center.transform.childCount}");

        for (int i = 0; i < numOfChild; i++)
        {
            // 사용하고자 한 아이템의 인덱스와 같은 Slots_Center의 자식개체를 Destroy한다.
            if (i == selectedItemIndex)
            {
                GameManager.Resource.Destroy(Slots_Center.transform.GetChild(i).gameObject);
            }
        }
         
        itemList.RemoveAt(selectedItemIndex); // 소모형 아이템을 사용했기 때문에 해당 아이템을 제거한다.
        selectedItemIndex = selectedItemIndex - 1 < 0 ? 0 : selectedItemIndex - 1; // 셀렉티드아이템인덱스 초기화
        selectedItem = itemList[selectedItemIndex];

        inventoryResetCoroutine = StartCoroutine(ChildPositionSet());

        float rotateFloat = (360 / player._Inventroy.Pocket.Count);
        naviCoroutine = StartCoroutine(LookAtRoutine(Quaternion.Euler(new Vector3(0, -1 * selectedItemIndex * rotateFloat, 0))));
    }

    private IEnumerator ChildPositionSet()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"디스트로이 후의 Slots_Center의 자식 개체 수: {Slots_Center.transform.childCount}");
        SetObjectPosition(); // Slots의 자식개체들 포지션 지정해주는 함수
        yield return new WaitForEndOfFrame();
        StopCoroutine(inventoryResetCoroutine);
    }



    private Item FindItemWithIndex(int index)
    {
        int count = 0;
        foreach (Item value in player._Inventroy.Pocket.Values)     // 포켓내부를 돌며.. 매개변수로 전달받은 인덱스에 맞는 값을 리턴한다.
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
        Debug.Log($"2. 현재 포켓 내의 아이템 개수는 : {player._Inventroy.Pocket.Count}");
        
        foreach (Item value in player._Inventroy.Pocket.Values)
        {
            // 아이템 오브젝트 생성 및 부모객체 설정
            GameObject insertData;
            insertData = GameManager.Instantiate<GameObject>(value.Slot, Slots_Center.transform.position, Quaternion.Euler(0, 0, 0));
            SetSlotParent(insertData);
        }

        SetObjectPosition(); // Slots의 자식개체들 포지션 지정해주는 함수
        naviCoroutine = StartCoroutine(LookAtRoutine(Quaternion.Euler(new Vector3(0, -1 * selectedItemIndex * rotateFloat, 0))));
        SetItemText();
    }

}     


