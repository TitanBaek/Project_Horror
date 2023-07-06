using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { Weapon, subWeapon, Usable, KeyItem };
public class Item : MonoBehaviour, ICheckable
{
    [SerializeField] private Player player;  
    [SerializeField] protected int itemCode;
    [SerializeField] protected ItemCategory category; 
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;
    [SerializeField] protected GameObject slot;
    [SerializeField] protected GameObject render;
    protected int itemEA = 1;
    public ItemCategory Category { get { return category; } }
    public int ItemEA { get { return itemEA; } set { itemEA = value; } }    
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string Description { get { return description; } set { description = value; } }
    public GameObject Slot {  get { return slot; } set {  slot = value; } }
    public GameObject Render { get { return render; } set {  render = value; } }

    private void Awake()
    {
    }

    private void Start()
    {
        Init();
        Debug.Log("시작");
        Debug.Log("끝");

    }

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void GetItem()
    {
        if (player == null)
            Init();
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
