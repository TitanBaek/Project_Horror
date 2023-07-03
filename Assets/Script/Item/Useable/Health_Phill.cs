using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Phill : Item, IUseable
{
    [SerializeField] private int health_Plus;

    public void UseItem(Player player)
    {
       player.CurHp = (player.CurHp + health_Plus) > player.MaxHp ? player.MaxHp : player.CurHp + health_Plus;
        player._Inventroy.RemoveItem(this);
    }
}
