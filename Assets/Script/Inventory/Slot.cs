using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    Item item;
    private void Awake()
    {    
        item = GetComponentInChildren<Item>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 0.5f, Space.Self);
    }
}
