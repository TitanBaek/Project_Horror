using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public abstract void Stun();
    public abstract void TakeHit(int dmg);
}