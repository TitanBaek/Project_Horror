using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void LateUpdate()
    {
        this.gameObject.transform.parent.LookAt(player.transform);
    }
}
