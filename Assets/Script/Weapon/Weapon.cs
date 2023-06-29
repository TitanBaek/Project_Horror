using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float damage;

    public virtual void Attack()
    {
        Debug.Log("°ø°ÝÇÔ");
    }
}
