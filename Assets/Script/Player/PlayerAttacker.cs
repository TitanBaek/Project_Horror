using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private float reloadTime;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private GameObject tpsCam;
    [SerializeField] private GameObject aimCam;
    [SerializeField] private GameObject aimPoint;
    private Animator anim;
    private bool reloading;
    private bool isAim = false;
    // 주준중일때 moveSpeed 깎아주는 거 구현해야함



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Fire()
    {
        weaponHolder.Fire();
        anim.SetTrigger("Fire");
    }

    private void OnFire(InputValue value)
    {

        if (!isAim) // 조준중이 아닐땐 공격이 안된다.
            return;
        if (reloading)  // 장전중일 땐 공격이 안된다.
            return;

        Fire();
    }

    private void OnReload(InputValue value)
    {
        StartCoroutine(ReloadRoutine());
    }

    private void OnAim(InputValue value)
    {
        if (value.isPressed)
        {
            tpsCam.SetActive(false);
            aimCam.SetActive(true);
            aimPoint.SetActive(true);
            anim.SetBool("Aim", true);
            anim.SetLayerWeight(1, 1);
            isAim = true;
        }
        else
        {
            aimCam.SetActive(false);
            tpsCam.SetActive(true);
            aimPoint.SetActive(false);
            anim.SetLayerWeight(1, 0);
            anim.SetBool("Aim", false);
            isAim = false;
        }
    }

    IEnumerator ReloadRoutine()
    {
        anim.SetLayerWeight(1, 1);
        anim.SetTrigger("Reload");
        aimRig.weight = 0f;
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        aimRig.weight = 0.6f;
        anim.SetLayerWeight(1, 0);
        reloading = false;
    }
}
