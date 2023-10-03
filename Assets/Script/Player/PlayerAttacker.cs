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
    [SerializeField] private Transform muzzleSetTransform;
    private GameObject muzzleEffect;
    private AudioSource[] audioSource;              // ���ؼҸ� , �߻����� ź�� �������� �Ҹ��� ��� ��ų ����� �ҽ� �迭(2��)
    private Animator anim;
    private bool reloading;
    private bool isAim = false;
    private Player player;
    private bool equipWeapon;
    // �������϶� moveSpeed ����ִ� �� �����ؾ���

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = weaponHolder.GetComponents<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Fire()
    {
        weaponHolder.Fire();
        anim.SetTrigger("Fire"); 
        int randNum = Random.Range(1, 5);
        audioSource[0].clip = GameManager.Resource.Load<AudioClip>($"Sound/Beretta92_Shot00{randNum}");
        audioSource[0].Play();

        muzzleEffect = GameManager.Resource.Instantiate<GameObject>("Effect/MuzzleFlash", muzzleSetTransform.position, muzzleSetTransform.rotation, true);
        GameManager.Resource.Destroy(muzzleEffect, 0.3f);
    }

    private void OnFire(InputValue value)
    {

        if (!isAim) // �������� �ƴҶ� ������ �ȵȴ�.
            return;
        if (reloading)  // �������� �� ������ �ȵȴ�.
            return;
        Fire();
    }

    private void OnReload(InputValue value)
    {
        if (reloading)
            return;
        StartCoroutine(ReloadRoutine());
    }

    private void OnAim(InputValue value)
    {
        if (player._Inventroy.equipment[0] == null)
            return;
        if (value.isPressed)
        {
            int randNum = Random.Range(1, 3);
            audioSource[0].clip = GameManager.Resource.Load<AudioClip>($"Sound/Beretta92_Aim_00{randNum}");
            if (!audioSource[0].isPlaying)
                audioSource[0].Play();

            tpsCam.SetActive(false);
            aimCam.SetActive(true);
            aimPoint.SetActive(true);
            anim.SetBool("Aim", true);
            anim.SetLayerWeight(1, 1);
            isAim = true;
        }
        else
        {
            /*
            int randNum = Random.Range(1, 5);
            audioSource[0].clip = GameManager.Resource.Load<AudioClip>($"Sound/Beretta92_Handling00{randNum}");
            if (!audioSource[0].isPlaying)
                audioSource[0].Play();
            */
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
