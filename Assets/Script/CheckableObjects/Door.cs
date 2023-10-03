using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ICheckable
{
    [SerializeField] bool needKey;
    [SerializeField] HospitalKey keyItem;
    Coroutine dialogCheckCoroutine;
    Objects dialogObject;
    bool nowOpen = false;
    public bool NowOpen { get { return nowOpen; } set { nowOpen = value; } }

    private void Awake()
    {
        if(gameObject.GetComponent<Objects>() != null)
        {
            dialogObject = gameObject.GetComponent<Objects>();
            dialogObject.enabled = false;
        }
    }

    public virtual void Check()
    {
        if (needKey) // 키가 필요한 상호작용 오브젝트인 경우
        {
            if (!GameManager.Data._player._Inventroy.HaveItem(keyItem))
            {
                dialogObject.enabled = true;
                dialogObject.Check();
                dialogCheckCoroutine = StartCoroutine(CheckDialogEnds());
                return;
            }
            GameManager.Sound.PlaySound("Unlock", Audio.UISFX, this.gameObject);
            GameManager.Sound.PlaySound("Open", Audio.UISFX, this.gameObject);
            GameManager.Scene.LoadScene("TitleScene");
        }
    }

    IEnumerator CheckDialogEnds()
    {
        yield return new WaitUntil(() => dialogObject.NowOpen == false );
        dialogObject.enabled = false;
    }

    private void OnDisable()
    {
        if(dialogCheckCoroutine != null)
            StopCoroutine(dialogCheckCoroutine);
    }

}
