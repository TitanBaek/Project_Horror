using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : Item, IUseable
{
    [SerializeField] protected int health_Plus;

    public void UseThis(Player player)
    {
        player.CurHp = (player.CurHp + health_Plus) > player.MaxHp ? player.MaxHp : player.CurHp + health_Plus;
        player._Inventroy.RemoveItem(this);
    }
}
