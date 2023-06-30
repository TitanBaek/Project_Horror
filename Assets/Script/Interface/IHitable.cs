using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    // Stun -> Hit �� 
    // TakeHit -> TakeDamage ��
    public abstract void Stun();
    public abstract void TakeHit(RaycastHit hit, int dmg);
    public abstract void TakeHit(int dmg);
}
