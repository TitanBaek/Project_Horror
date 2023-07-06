using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Phill : UseItem, IUseable
{
    private void Awake()
    {
        this.category = ItemCategory.Usable;
    }
}
