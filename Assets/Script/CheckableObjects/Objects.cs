using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour, ICheckable
{ 
    [SerializeField] string[] scripts;
    bool nowOpen;
    [SerializeField] string sfxPath;
    public bool NowOpen { get { return nowOpen; }  set { nowOpen = value; } }

    private void Start()
    {
        nowOpen = false;
    }

    public virtual void Check()
    {
        if (!nowOpen)
        {
            if(sfxPath == "")
                GameManager.Sound.PlaySound(sfxPath, Audio.UISFX, this.gameObject);
            GameManager.Dialog.SetDialogScripts(this, scripts);
            nowOpen = true;
            return;
        }
        GameManager.Dialog.TabDialogKey();
    }
}
