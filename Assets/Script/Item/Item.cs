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
        Debug.Log("����");
        Debug.Log("��");

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
        // ���̾�α� �Ŵ����� �����ؼ� �ش� �������� description�� ȭ�鿡 ǥ��
        Debug.Log("ShowItemInfo");
    }

    public void Check()
    {
        // �������� ���� ��� ������ ���̾�α� �Ŵ��� �����ϸ� ����ֱ�
        Debug.Log($"{this.itemName}�� �տ� �ִ�");
        GetItem();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
