using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float damage;
    [SerializeField] protected AudioClip[] weaponSoundClips;

    public virtual void Attack()
    {
        Debug.Log("°ø°ÝÇÔ");
    }
}
