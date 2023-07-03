using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { Weapon, subWeapon, Usable, KeyItem };
public class Item : MonoBehaviour, ICheckable
{
    private Player player;  
    [SerializeField] protected int itemCode;
    [SerializeField] private ItemCategory category;
    [SerializeField] protected string name;
    [SerializeField] protected string description;
    protected int itemEA = 1;
    public ItemCategory Category { get { return category; } }
    public int ItemEA { get { return itemEA; } set { itemEA = value; } }    



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log($"������ ���� : {player.name}");
    }
    public void GetItem()
    {
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
        Debug.Log($"{this.name}�� �տ� �ִ�");
        GetItem();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
