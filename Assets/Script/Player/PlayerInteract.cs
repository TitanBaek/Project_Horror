using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] Transform point;
    [SerializeField] float range;
    [SerializeField] float angle;

    private float delayTimer = 0;
    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(point.position, range);
        foreach (Collider collider in colliders)
        {           
            ICheckable interact = collider.GetComponent<ICheckable>();
            interact?.Check();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(point.position, range);
    }

    private void OnCheck(InputValue value)
    {
            Interact();
    }
}
