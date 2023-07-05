using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { Weapon, subWeapon, Usable, KeyItem };
public class Item : MonoBehaviour, ICheckable
{
    private Player player;  
    [SerializeField] protected int itemCode;
    [SerializeField] private ItemCategory category;
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;
    [SerializeField] protected GameObject slot;
    protected int itemEA = 1;
    public ItemCategory Category { get { return category; } }
    public int ItemEA { get { return itemEA; } set { itemEA = value; } }    
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string Description { get { return description; } set { description = value; } }
    public GameObject Slot {  get { return slot; } set {  slot = value; } }


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log($"아이템 생성 : {player.name}");
    }
    public void GetItem()
    {
        Debug.Log("GetItem");
        player._Inventroy.AddItem(this); 
        Disable();
    }

    public void ShowItemInfo()
    {
        // 다이얼로그 매니저와 연동해서 해당 아이템의 description을 화면에 표시
        Debug.Log("ShowItemInfo");
    }

    public void Check()
    {
        // 가져가냐 마냐 라는 선택지 다이얼로그 매니저 구현하면 집어넣기
        Debug.Log($"{this.itemName}가 앞에 있다");
        GetItem();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
