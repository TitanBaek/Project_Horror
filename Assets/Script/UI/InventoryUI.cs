using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : BaseUI
{
    [SerializeField] private GameObject Slots_Center;
    private Player player;
    private float radius = 70f;
    private int numOfChild;
    private int inventoryMaxnum = 8;
    private Vector3 dir;
    private Item selectedItem;
    Coroutine naviCoroutine;

    private void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
    }

    public void SetSelectedItem(Item item)
    {
        Debug.Log($"아이템 선택 {item.ItemName}");
    }
    public void ResetInventory()
    {
        numOfChild = Slots_Center.transform.childCount;

        for (int i = 0; i < numOfChild; i++)
        {
            GameManager.Resource.Destroy(Slots_Center.transform.GetChild(i).gameObject);            
        }
    }

    public void SetInventory()
    {
        Debug.Log("SetInventory");
        int curItemEA = 0;
        foreach (Item value in player._Inventroy.Pocket.Values)
        {
            Debug.Log($"SetInventory : {value.ItemName}");
            GameObject insertData;            
            insertData = GameManager.Instantiate<GameObject>(value.Slot, Slots_Center.transform.position, Quaternion.Euler(0, 0, 0));
            SetSlotParent(insertData);
        }

        numOfChild = Slots_Center.transform.childCount;

        for (int i = 0; i < numOfChild; i++)
        {
            //float angle = i * (Mathf.PI * 2.0f) / numOfChild;
            float angle = Mathf.PI * 0.5f - i * (Mathf.PI * 2.0f) / numOfChild;
            GameObject child = Slots_Center.transform.GetChild(i).gameObject;

            child.transform.position
                = Slots_Center.transform.position + (new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle))) * radius;
        }
    }

    private void SetSlotParent(GameObject slot)
    {
        slot.transform.SetParent(Slots_Center.transform);
    }

    private void OnNavigation(InputValue value)
    {
        float rotateFloat = (360 / player._Inventroy.Pocket.Count);
        dir = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
        float y = Slots_Center.transform.rotation.y;
        if (dir.x > 0)
        {
            // 오른쪽
            //Slots_Center.transform.rotation = Quaternion.Slerp(Slots_Center.transform.rotation,Quaternion.Euler(new Vector3(0,rotateFloat,0)),5f);
            Slots_Center.transform.Rotate(new Vector3(0, +rotateFloat, 0));
            naviCoroutine = StartCoroutine(Rotate(rotateFloat));
        } else if(dir.x < 0)
        {
            // 왼쪽
            //Slots_Center.transform.rotation = Quaternion.Slerp(Slots_Center.transform.rotation, Quaternion.Euler(new Vector3(0, - rotateFloat, 0)), 5f);
            Slots_Center.transform.Rotate(new Vector3(0,-rotateFloat, 0));
            naviCoroutine = StartCoroutine(Rotate(rotateFloat * -1));
        }
    }

    //TODO 오늘 하자..

    IEnumerator Rotate(float rotateFloat,float duration = 1f)
    {
        float goalRotate = Slots_Center.transform.rotation.y + rotateFloat;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Slots_Center.transform.Rotate(new Vector3(0, 1 * Time.deltaTime, 0));
        }
        yield return null;
    }




}     


