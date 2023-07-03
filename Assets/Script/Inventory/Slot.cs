using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    private void Awake()
    {
    }
    void Update()
    {
        transform.Rotate(Vector3.up, 0.25f, Space.Self);
    }
}
